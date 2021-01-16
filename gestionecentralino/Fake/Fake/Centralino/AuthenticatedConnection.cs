using System;
using System.IO;

namespace gestionecentralino.Fake.Fake.Centralino
{
    public class AuthenticatedConnection
    {
        private readonly Centralino _centralino;

        public AuthenticatedConnection(Centralino centralino)
        {
            _centralino = centralino;
        }

        public void SendAllData(string filename)
        {
            String[] fileLines = File.ReadAllLines(filename);
            foreach (string line in fileLines)
            {
                _centralino.SendLine(line);
            }
        }
    }
}