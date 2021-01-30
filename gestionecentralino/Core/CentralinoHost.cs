using System.IO;
using System.Text;
using LanguageExt;
using LanguageExt.Common;

namespace gestionecentralino.Core
{
    public class CentralinoHost
    {
        public string Host { get; }
        public int Port { get; }

        public CentralinoHost(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}