using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using gestionecentralino.Core;
using gestionecentralino.Core.Lines;
using gestionecentralino.Db;
using LanguageExt;
using LanguageExt.Common;
using log4net;
using log4net.Config;

namespace gestionecentralino
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var log = ConfigureLogging();
                log.Info("Starting ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                try
                {
                    FromConfigReadCentralinoAndWriteInStorage(args, log);
                }
                catch (Exception e)
                {
                    log.Error($"Unexpected error: {e.Message}", e);
                }
                log.Info("Ending ####################################################################");
            }
            else
            {
                CentralinoConfiguration.FromCommanLine(new[] { "--help" });
            }
        }

        private static void FromConfigReadCentralinoAndWriteInStorage(string[] args, ILog log)
        {
            CentralinoConfiguration
                .FromCommanLine(args)
                .Map(configuration => ReadFromCentralinoAndWrite(new CentralinoReader(configuration), log, configuration));
        }

        private static ILog ConfigureLogging()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            ILog log = LogManager.GetLogger(typeof(Program));
            return log;
        }

        private static Unit ReadFromCentralinoAndWrite(CentralinoReader centralinoReader, ILog log, CentralinoConfiguration centralinoConfiguration)
        {
            try
            {
                log.Info($"Configuration: {centralinoConfiguration}");

                var allLines = ReadFromCentralino(centralinoReader, log, centralinoConfiguration);
                WriteIntoTheStorage(log, centralinoConfiguration, allLines);
            }
            catch (Exception e)
            {
                log.Error($"Unexpected error: {e.Message}", e);
            }

            return Unit.Default;
        }

        private static void WriteIntoTheStorage(ILog log, CentralinoConfiguration centralinoConfiguration, CentralinoLines allLines)
        {
            log.Info($"Starting to write all lines on storage {centralinoConfiguration.DbConfiguration.ConnectionString}");

            (IEnumerable<Seq<Error>> errors, IEnumerable<ICentralinoLine> lines) = allLines.Lines.Partition();

            DbSerializer dbSerializer = new DbSerializer(centralinoConfiguration.DbConfiguration, centralinoConfiguration.Sede);
            dbSerializer.WriteAll(lines);

            foreach (Seq<Error> error in errors)
            {
                log.Warn(ErrorToString(error));
            }
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

        private static string ErrorToString(Seq<Error> errors) => errors.Aggregate("|", (acc, error) => $"{acc}{error.Message}");
    }
}
