using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gestionecentralino.Core;
using gestionecentralino.Core.Lines;
using gestionecentralino.Db;
using gestionecentralino.MockServer;
using LanguageExt;
using LanguageExt.Common;
using log4net;

namespace gestionecentralino
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ILog log = LogManager.GetLogger(typeof(Program));

            //var configuration = new CentralinoConfiguration("192.168.0.102", 2300, "SMDR", "SMDR");
            var configuration = new CentralinoConfiguration("127.0.0.1", 2300, "SMDR", "SMDR")
            {
                MockServerSourceFile = @"Resources\costa_centralino_mock_source.txt"
            };
            //using Task<OpenCentralinoMock> mock = StartMockServer(configuration);

            var reader = CentralinoReader.Of(configuration);
            reader.Match(centralinoReader =>
            {
                DbSerializer dbSerializer = new DbSerializer(@"Data Source=(LocalDB)\gestioneriparazioni;Initial Catalog=centralino;Integrated Security=True");
                
                //CentralinoLines allLines = await centralinoReader.ReadAllLines();
                CentralinoLines allLines = CentralinoLines.Parse(File.ReadAllLines(configuration.MockServerSourceFile));
                foreach (var line in allLines.Lines)
                {
                    WriteInDb(line, dbSerializer, log);
                }
                dbSerializer.WriteAll();

            }, error =>
            {
                log.Error($"There is an error in the configuration: {error.Message}");
            });

            //await mock;
        }

        private static async Task<OpenCentralinoMock> StartMockServer(CentralinoConfiguration configuration)
        {
            CentralinoMockServer mockServer = new CentralinoMockServer(configuration.Host, configuration.Port, configuration.MockServerSourceFile);
            return await mockServer.StartServer();
        }

        private static void WriteInDb(Either<Seq<Error>, ICentralinoLine> line, DbSerializer dbSerializer, ILog log)
        {
            line.Match(centralinoLine =>
            {
                dbSerializer.Serialize(centralinoLine);
                log.Info($"Processed line {centralinoLine}");
            }, errors =>
            {
                log.Warn(ErrorToString(errors));
            });
        }

        private static string ErrorToString(Seq<Error> errors) => errors.Aggregate("|", (acc, error) => $"{acc}{error.Message}");
    }
}
