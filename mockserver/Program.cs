using System;
using System.Threading.Tasks;
using gestionecentralino.MockServer;

namespace mockserver
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            CentralinoMockServer mockServer = new CentralinoMockServer(
                "127.0.0.1", 
                2300, 
                "costa_centralino_mock_source.txt");

            await mockServer.StartServer();
        }
    }
}
