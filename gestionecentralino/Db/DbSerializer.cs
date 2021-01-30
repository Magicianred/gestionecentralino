using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using gestionecentralino.Core;
using gestionecentralino.Core.Lines;
using gestionecentralino.Core.Lines.Data;
using LanguageExt;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace gestionecentralino.Db
{
    public class DbSerializer: ICentralinoLineConsumer
    {
        private readonly SedeEnum _sede;
        private readonly ILog _log;
        private readonly CentralinoDbContext _db;

        public static DbSerializer From(CentralinoConfiguration configuration)
        {
            CentralinoDbContext db;
            switch (configuration.DbConfiguration.Db)
            {
                case DbEnum.SqlServer:
                    db = new SqlServerDbContext(configuration.DbConfiguration);
                    break;

                case DbEnum.MySql:
                    db = new MySqlDbContext(configuration.DbConfiguration);
                    break;


                default:
                    db = new SqlServerDbContext(configuration.DbConfiguration);
                    break;
            }

            return new DbSerializer(db, configuration.Sede);
        }

        private DbSerializer(CentralinoDbContext db, SedeEnum sede)
        {
            _sede = sede;
            _db = db;
            _log = LogManager.GetLogger(GetType());
        }

        public void CollectIntoDb(ICentralinoLine centralinoLine)
        {
            centralinoLine.Apply(this);
        }

        public void Read(OutgoingCall call)
        {
            AddToDb(call, false);
        }

        public void Read(InternalCall call)
        {
            Ignore(call);
        }

        public void Read(IntermissionLine call)
        {
            Ignore(call);
        }

        public void Read(HeadingLine call)
        {
            Ignore(call);
        }

        public void Read(ForwardLine call)
        {
            Ignore(call);
        }

        public void Read(ExternalCall call)
        {
            call.Call.Apply(this);
        }

        public void Read(IncomingCall call)
        {
            AddToDb(call, true);
        }

        private void AddToDb(ICallLine call, bool incoming)
        {
            Log(call);
            PhoneCallLine callLine = ToPhoneCallLine(call, incoming: incoming);
            bool lineAlreadyProcessed = _db.Calls.Any(call => callLine.Hash == call.Hash);
            if (!lineAlreadyProcessed)
            {
                _db.Add(callLine);
            }
            else
            {
                _log.Warn($"Ignoring line beacause already processed {call.CallData.OriginalLine}");
            }
        }

        private void Log(ICentralinoLine call)
        {
            _log.Debug($"serializing {call}");
        }

        private void Ignore(ICentralinoLine call)
        {
            _log.Debug($"ignoring {call}");
        }

        private PhoneCallLine ToPhoneCallLine(ICallLine call, bool incoming)
        {
            CallData callData = call.CallData;
            return new PhoneCallLine
            {
                ExternalNumber = call.TargetNumber.Value,
                InternalNumber = callData.InternalNumber.Value,
                Cost = callData.Cost.Value.Value,
                CoCode = callData.CoCode.Value,
                DateTime = callData.DateTime,
                Duration = callData.Duration.Value,
                CdCode = callData.CdCode.Map(code => code.Value).IfNone(() => string.Empty),
                Incoming = incoming,
                Sede = _sede,
                OriginalLine = callData.OriginalLine,
                Hash = GetHash(SHA256.Create(), callData.OriginalLine)
            };
        }

        public async Task WriteAllAsync()
        {
            await _db.Database.MigrateAsync();
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();
        }
        public void WriteAll()
        {
            _db.SaveChanges();
            _db.Dispose();
        }

        public void WriteAll(IEnumerable<ICentralinoLine> lines)
        {
            _db.Migrate();
            var centralinoLines = lines.ToArray();
            foreach (ICentralinoLine centralinoLine in centralinoLines)
            {
                CollectIntoDb(centralinoLine);
                _log.Info($"Processed line {centralinoLine}");
            }
            WriteAll();
        }

        private string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}