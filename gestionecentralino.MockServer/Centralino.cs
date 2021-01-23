using System;
using System.Net.Sockets;

namespace gestionecentralino.MockServer
{
    public class Centralino: IDisposable
    {
        private const char NewLine = '\r';
        private const string StartCommunication = "\r-";
        private const string AskForPAssword = "Enter Password:";
        private readonly Socket _listener;

        public Centralino(Socket listener)
        {
            _listener = listener;
        }

        public AuthenticatedConnection WaitAuthentication()
        {
            Send(StartCommunication);
            string userName = WaitInputAndMirror();
            Send(AskForPAssword);
            string password = WaitInputAndMirror();

            return new AuthenticatedConnection(this);
        }

        public void Send(string data)
        {
            _listener.Send((byte[]) Encode.Bytes(data));
        }

        public void Dispose()
        {
            _listener.Disconnect(false);
            _listener.Dispose();
        }

        private char ReceiveOne()
        {
            byte[] receiveBuffer = new byte[1];
            _listener.Receive(receiveBuffer, 1, SocketFlags.None);
            return (char)receiveBuffer[0];
        }

        private void Send(char c)
        {
            _listener.Send((byte[]) Encode.Char(c));
        }

        public void SendLine(string data)
        {
            Send((string) Encode.Line(data));
        }

        private string WaitInputAndMirror()
        {
            string inputFromSocket = "";
            char c = '\0';
            do
            {
                c = ReceiveOne();
                Send(c);
                inputFromSocket = $"{inputFromSocket}{c}";
            } while (c != NewLine);

            return inputFromSocket;
        }
    }
}