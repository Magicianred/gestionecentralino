using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gestionecentralino.Core;
using gestionecentralino.Core.Lines;
using LanguageExt;
using LanguageExt.Common;
using log4net;

namespace gestionecentralino
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ILog _log = LogManager.GetLogger(typeof(Program));

            var configuration = new CentralinoConfiguration("192.168.0.102", 2300, "SMDR", "SMDR");
            var reader = CentralinoReader.Of(configuration);
            reader.Match(async centralinoReader =>
            {
                CentralinoLines allLines = await centralinoReader.ReadAllLines();
                foreach (var allLinesLine in allLines.Lines)
                {
                    DoSomethingWithLine(allLinesLine, _log);
                }

            }, error =>
            {
                _log.Error($"There is an error in the configuration: {error.Message}");
            });
        }

        private static void DoSomethingWithLine(Either<Seq<Error>, ICentralinoLine> line, ILog log)
        {
            line.Match(centralinoLine =>
            {
                log.Warn($"Processed line {centralinoLine}");
            }, errors =>
            {
                log.Warn(ErrorToString(errors));
            });
        }

        private static string ErrorToString(Seq<Error> errors) => errors.Aggregate("|", (acc, error) => $"{acc}{error.Message}");
    }
}
