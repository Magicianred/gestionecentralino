using System.IO;
using System.Text;

namespace gestionecentralino.Core
{
    public class StreamSender
    {
        private readonly Stream _stream;

        public StreamSender(Stream stream)
        {
            _stream = stream;
        }

        public void SendLine(string data)
        {
            Send(Encode.Line(data));
        }

        public void Send(string data)
        {
            var bytes = Encode.Bytes(data);
            _stream.Write(bytes, 0, bytes.Length);
        }

        public void Send(char c)
        {
            _stream.Write(Encode.Char(c), 0, 1);
        }
    }
}