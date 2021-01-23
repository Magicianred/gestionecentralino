using System;
using System.Threading.Tasks;
using gestionecentralino.MockServer;

namespace mockserver
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            const int port = 2300;
            var phoneCallsFile = @"Resources\costa_centralino_mock_source.txt";
            CentralinoMockServer mockServer = new CentralinoMockServer(
                "127.0.0.1", 
                port, 
                phoneCallsFile);

            Console.WriteLine($"Starting server with file {phoneCallsFile} on port {port}...");
            await mockServer.StartServer();
        }
    }
}
