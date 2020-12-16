using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;

namespace gestionecentralino.Core
{
    public class AuthenticatedChannel
    {
        private readonly StreamReader _streamReader;

        public AuthenticatedChannel(NetworkStream stream)
        {
            _streamReader = new StreamReader(stream);
        }

        public async Task<CentralinoLines> ReadLines()
        {
            List<string> lines = new List<string>();
            string line = null;
            do
            {
                AddNotNullLine(line, lines);
                line = await ReadALineWithTimeout(timeout: 1000);
            } while (!TimedOut(line));

            return CentralinoLines.Parse(lines.ToArray());
        }

        private bool TimedOut(string line) => line == null;

        private async Task<string> ReadALineWithTimeout(int timeout)
        {
            var read = _streamReader.ReadLineAsync();
            await Task.WhenAny(read, Task.Delay(timeout));
            return read.IsCompleted ? read.Result : null;
        }

        private void AddNotNullLine(string line, List<string> lines)
        {
            if (line != null)
            {
                lines.Add(line);
            }
        }
    }
}