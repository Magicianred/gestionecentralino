namespace gestionecentralino.Core.Lines.Data
{
    public class IncomingNumber: ITargetNumber
    {
        public string Value { get; }

        public IncomingNumber(string value)
        {
            Value = value;
        }

        public ICallLine CreateCall(CallData callData) => CallSource.CreateCall(this, new IncomingCall(
            callData.DateTime,
            callData.InternalNumber,
            this,
            callData.CoCode,
            callData.CdCode,
            callData.Duration,
            callData.Cost));

        protected bool Equals(IncomingNumber other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IncomingNumber) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}