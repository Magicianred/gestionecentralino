using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ILog _log;
        private readonly CentralinoDbContext _db;

        public DbSerializer(DbConfiguration configuration)
        {
            _db = new CentralinoDbContext(configuration.ConnectionString);
            _log = LogManager.GetLogger(GetType());
        }

        public void Serialize(ICentralinoLine centralinoLine)
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
            _db.Add(callLine);
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
                Incoming = incoming
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
            _db.Database.Migrate();
            _db.SaveChanges();
            _db.Dispose();
        }

        public void WriteAll(IEnumerable<ICentralinoLine> lines)
        {
            var centralinoLines = lines.ToArray();
            foreach (ICentralinoLine centralinoLine in centralinoLines)
            {
                Serialize(centralinoLine);
                _log.Info($"Processed line {centralinoLine}");
            }
            WriteAll();
        }
    }
}