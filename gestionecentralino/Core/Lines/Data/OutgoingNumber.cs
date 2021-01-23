namespace gestionecentralino.Core.Lines.Data
{
    public class OutgoingNumber: ITargetNumber
    {
        public string Value { get; }

        public OutgoingNumber(string value)
        {
            Value = value;
        }

        public ICallLine CreateCall(CallData callData) => CallSource.CreateCall(this, new OutgoingCall(
            callData.DateTime,
            callData.InternalNumber,
            this,
            callData.CoCode,
            callData.CdCode,
            callData.Duration,
            callData.Cost));

        public override string ToString() => $"{GetType().Name}: {Value}";

        protected bool Equals(OutgoingNumber other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OutgoingNumber) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}