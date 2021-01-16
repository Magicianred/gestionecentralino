using System.Threading.Tasks;

namespace gestionecentralino.Fake.Fake.Centralino
{
    public class CentralinoMockServer
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _filename;

        public CentralinoMockServer(string host, int port, string filename)
        {
            _host = host;
            _port = port;
            _filename = filename;
        }

        public async Task<OpenCentralinoMock> StartServer()
        {
            var mockServer = OpenCentralinoMock.Open(_host, _port);
            return await Task.Run(() =>
            {
                using (Centralino listener = mockServer.StartListening())
                {
                    listener
                        .WaitAuthentication()
                        .SendAllData(_filename);
                }
                return mockServer;
            });
        }
    }
}