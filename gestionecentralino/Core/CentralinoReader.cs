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

        private CentralinoReader(CentralinoHost host, UserAccount account)
        {
            _host = host;
            _account = account;
        }

        public static Either<Error, CentralinoReader> Of(CentralinoConfiguration config) => 
            CentralinoHost.Of(config.Host, config.Port)
                .Map(centralinoHost => new CentralinoReader(
                    centralinoHost, 
                    new UserAccount(config.Username, config.Password)));

        public async Task<CentralinoLines> ReadAllLines()
        {
            using (var centralinoCommunication = await CentralinoChannel.Start(_host))
            {
                AuthenticatedChannel authenticatedChannel = await centralinoCommunication.Authenticate(_account.Username, _account.Password);
                return await authenticatedChannel.ReadLines();
            }
        }
    }
}