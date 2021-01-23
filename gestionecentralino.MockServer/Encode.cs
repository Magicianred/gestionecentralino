using System.Text;

namespace gestionecentralino.MockServer
{
    public static class Encode
    {
        public static byte[] Bytes(string data) => Encoding.ASCII.GetBytes($"{data}");

        public static string Line(string data) => $"{data}\r";

        public static byte[] Char(char c) => new[] { (byte)c };
    }
}