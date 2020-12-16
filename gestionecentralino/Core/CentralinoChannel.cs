using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;

namespace gestionecentralino.Core
{
    public class CentralinoChannel: IDisposable
    {
        private readonly TcpClient _tcpClient;
        private readonly NetworkStream _stream;
        private readonly StreamSender _streamSender;
        private readonly StreamReader _streamReader;

        public static async Task<CentralinoChannel> Start(CentralinoHost centralinoHost)
        {
            TcpClient tcpClient = new TcpClient(centralinoHost.Host, centralinoHost.Port);
            NetworkStream networkStream = tcpClient.GetStream();
            
            await SkipFirstHyphen(networkStream);

            return new CentralinoChannel(tcpClient, networkStream);
        }

        private CentralinoChannel(TcpClient tcpClient, NetworkStream networkStream)
        {
            _tcpClient = tcpClient;
            _stream = networkStream;
            _streamSender = new StreamSender(_stream);
            _streamReader = new StreamReader(_stream);
        }

        public void Dispose()
        {
            _tcpClient?.Dispose();
            _stream?.Dispose();
        }

        public async Task<AuthenticatedChannel> Authenticate(string username, string password)
        {
            await SendEndReadEcho(username);
            await FetchPasswordRequestLine();
            await SendEndReadEcho(password);

            return new AuthenticatedChannel(_stream);
        }

        private static async Task SkipFirstHyphen(NetworkStream networkStream)
        {
            char[] communicationStart = new char[2];
            await new StreamReader(networkStream).ReadBlockAsync(communicationStart, 0, 2);
        }

        private async Task FetchPasswordRequestLine()
        {
            var x2 = new char[15];
            await _streamReader.ReadAsync(x2, 0, x2.Length);
        }

        private async Task SendEndReadEcho(string data)
        {
            _streamSender.SendLine(data);
            char[] echoBuffer = new char[data.Length + 1];
            await _streamReader.ReadBlockAsync(echoBuffer, 0, echoBuffer.Length);
        }
    }
}