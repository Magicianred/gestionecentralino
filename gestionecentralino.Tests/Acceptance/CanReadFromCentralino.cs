using System;
using System.IO;
using System.Threading.Tasks;
using gestionecentralino.Core;
using gestionecentralino.Tests.Fake.Centralino;
using LanguageExt.UnitTesting;
using Xunit;
using static LanguageExt.Prelude;

namespace gestionecentralino.Tests.Acceptance
{

    public class CanReadFromCentralino
    {
        private const string Host = "127.0.0.1";
        private const int Port = 2300;
        private static string _serverSourceFile = @"Acceptance\Resources\costa_centralino_mock_source.txt";
        private readonly string[] _expectedLines = File.ReadAllLines(_serverSourceFile);

        private readonly CentralinoMockServer _mockServer = new CentralinoMockServer(Host, Port, _serverSourceFile);

        [Fact]
        public async Task Test()
        {
            Task<OpenCentralinoMock> centralino = _mockServer.StartServer();

            try
            {
                var reader = CentralinoReader.Of(
                    new CentralinoConfiguration(Host, Port, "SMDR", "SMDR"));
                reader.ShouldBeRight(async centralinoReader =>
                {
                    var actualLines = await centralinoReader.ReadAllLines();
                    Assert.Equal(CentralinoLines.Parse(_expectedLines), actualLines);
                });
            }
            finally
            {
                (await centralino).Dispose();
            }
        }
    }
}
