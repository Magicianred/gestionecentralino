using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using gestionecentralino.Core;
using gestionecentralino.MockServer;
using LanguageExt.UnitTesting;
using Xunit;
using static LanguageExt.Prelude;

namespace gestionecentralino.Tests.Acceptance
{

    public class CanReadFromCentralino
    {
        private const string Host = "127.0.0.1";
        private static string _serverSourceFile = @"Acceptance\Resources\costa_centralino_mock_source.txt";
        private readonly string[] _expectedLines = File.ReadAllLines(_serverSourceFile);

        [Fact]
        public async Task GIVEN_ARunningCentralinoServer_ICan_ReadAllTheLines()
        {
            const int port = 2300;
            using Task<OpenCentralinoMock> centralino = new CentralinoMockServer(Host, port, _serverSourceFile).StartServer();

            var reader = CentralinoReader.Of(
                new CentralinoConfiguration(Host, port, "SMDR", "SMDR"));
            reader.ShouldBeRight(centralinoReader =>
            {;
                var readLinesTask = centralinoReader.ReadLines(); readLinesTask.Wait();
                var actualLines = readLinesTask.Result;
                Assert.Equal(CentralinoLines.Parse(_expectedLines), actualLines);
            });
                
            await centralino;
        }

        [Fact]
        public async Task GIVEN_ARunningCentralinoServer_IWant_ToLimit_TheNumberOfLinesThatIRead()
        {
            const int port = 2400;
            using Task<OpenCentralinoMock> centralino = new CentralinoMockServer(Host, port, _serverSourceFile).StartServer();

            var reader = CentralinoReader.Of(
                new CentralinoConfiguration(Host, port, "SMDR", "SMDR"));
            reader.ShouldBeRight(centralinoReader =>
            {
                var readLinesTask = centralinoReader.ReadLines(2); readLinesTask.Wait();
                var actualLines = readLinesTask.Result;
                var expected = CentralinoLines.Parse(_expectedLines.Take(2).ToArray());
                Assert.Equal(expected, actualLines);
            });

            await centralino;
        }
    }
}
