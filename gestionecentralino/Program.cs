using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using gestionecentralino.Core;
using gestionecentralino.Core.Lines;
using gestionecentralino.Db;
using gestionecentralino.MockServer;
using LanguageExt;
using LanguageExt.Common;
using log4net;
using log4net.Config;

namespace gestionecentralino
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            ILog log = LogManager.GetLogger(typeof(Program));

            log.Info("Starting ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            try
            {
                var configuration = new CentralinoConfiguration("127.0.0.1", 2300, "SMDR", "SMDR");
                configuration.DbConfiguration.ConnectionString =  @"Data Source=(LocalDB)\gestioneriparazioni;Initial Catalog=centralino;Integrated Security=True";

                //var configuration = new CentralinoConfiguration("192.168.0.102", 2300, "SMDR", "SMDR");
                var reader = CentralinoReader.Of(configuration);

                reader.Match(
                    centralinoReader => { ReadFromCentralinoAndWrite(centralinoReader, log, configuration); }, 
                    error => { log.Error($"There is an error in the configuration: {error.Message}"); });
            }
            catch (Exception e)
            {
                log.Error($"Unexpected error: {e.Message}", e);
            }
            log.Info("Ending ####################################################################");
        }

        private static void ReadFromCentralinoAndWrite(CentralinoReader centralinoReader, ILog log, CentralinoConfiguration centralinoConfiguration)
        {
            try
            {
                var allLines = ReadFromCentralino(centralinoReader, log, centralinoConfiguration);
                WriteIntoTheStorage(log, centralinoConfiguration, allLines);
            }
            catch (Exception e)
            {
                log.Error($"Unexpected error: {e.Message}", e);
            }
        }

        private static void WriteIntoTheStorage(ILog log, CentralinoConfiguration centralinoConfiguration, CentralinoLines allLines)
        {
            log.Info($"Starting to write all lines on storage {centralinoConfiguration.DbConfiguration.ConnectionString}");
            DbSerializer dbSerializer = new DbSerializer(centralinoConfiguration.DbConfiguration.ConnectionString);
            foreach (var line in allLines.Lines)
            {
                WriteInDb(line, dbSerializer, log);
            }

            dbSerializer.WriteAll();
            log.Info($"End of writing to the storage {centralinoConfiguration.DbConfiguration.ConnectionString}");
        }

        private static CentralinoLines ReadFromCentralino(CentralinoReader centralinoReader, ILog log, CentralinoConfiguration centralinoConfiguration)
        {
            log.Info($"Starting to read lines from centralino {centralinoConfiguration.Host}:{centralinoConfiguration.Port}");
            var task = centralinoReader.ReadAllLines();
            task.Wait();
            CentralinoLines allLines = task.Result;
            log.Info($"Read {allLines.Lines.Length} lines from centralino {centralinoConfiguration.Host}:{centralinoConfiguration.Port}");
            return allLines;
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
