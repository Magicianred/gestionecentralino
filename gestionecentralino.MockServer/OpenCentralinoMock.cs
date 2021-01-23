using System;
using System.Net;
using System.Net.Sockets;

namespace gestionecentralino.MockServer
{
    public class OpenCentralinoMock: IDisposable
    {
        private readonly Socket _socket;

        public static OpenCentralinoMock Open(string host, int port)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            socket.Listen(10);
            return new OpenCentralinoMock(socket);
        }

        private OpenCentralinoMock(Socket socket)
        {
            _socket = socket;
        }

        public MockServer.Centralino StartListening()
        {
            Socket listener = _socket.Accept();
            return new MockServer.Centralino(listener);
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }
    }
}