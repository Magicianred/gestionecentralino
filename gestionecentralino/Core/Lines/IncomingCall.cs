using System;
using gestionecentralino.Core.Lines.Data;
using LanguageExt;

namespace gestionecentralino.Core.Lines
{
    public class IncomingCall: ICallLine
    {
        public CallData CallData { get; }

        public IncomingNumber ExternalNumber { get; }

        public ITargetNumber TargetNumber => ExternalNumber;

        public IncomingCall(
            DateTime dateTime,
            InternalNumber internalNumber,
            IncomingNumber externalNumber,
            CoCode coCode,
            Option<CdCode> cdCode,
            Duration duration,
            Cost cost)
        {
            CallData = new CallData(dateTime, internalNumber, coCode, cdCode, duration, cost);
            ExternalNumber = externalNumber;
        }

        public LineTypeEnum LineType => LineTypeEnum.Call;
        public void Apply(ICentralinoLineConsumer consumer)
        {
            consumer.Read(this);
        }

        public override string ToString() => $"{GetType().Name}. {CallData}, {nameof(TargetNumber)}: {TargetNumber}";

        protected bool Equals(IncomingCall other)
        {
            return Equals(CallData, other.CallData) && Equals(ExternalNumber, other.ExternalNumber);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IncomingCall) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((CallData != null ? CallData.GetHashCode() : 0) * 397) ^ (ExternalNumber != null ? ExternalNumber.GetHashCode() : 0);
            }
        }
    }
}