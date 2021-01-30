using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using LanguageExt;
using LanguageExt.Common;

namespace gestionecentralino.Core
{
    public class CentralinoReader
    {
        private readonly CentralinoHost _host;
        private readonly UserAccount _account;

        public CentralinoReader(CentralinoHost host, UserAccount account)
        {
            _host = host;
            _account = account;
        }

        public CentralinoReader(CentralinoConfiguration configuration)
        {
            _host = new CentralinoHost(configuration.Host, configuration.Port);
            _account = new UserAccount(configuration.Username, configuration.Password);
        }

        public async Task<CentralinoLines> ReadAllLines() => await ReadLines(0);

        public async Task<CentralinoLines> ReadLines(int linesMaxLimit)
        {
            using var centralinoCommunication = await CentralinoChannel.Start(_host);

            AuthenticatedChannel authenticatedChannel = await centralinoCommunication.Authenticate(_account.Username, _account.Password);
            return await authenticatedChannel.ReadLines(linesMaxLimit);
        }
    }
}