namespace gestionecentralino.Core.Lines.Data
{
    public class ExternalCall: ICallLine
    {
        public ICallLine Call { get; }

        public ExternalCall(ICallLine call)
        {
            Call = call;
        }

        public LineTypeEnum LineType => Call.LineType;
        public void Apply(ICentralinoLineConsumer consumer)
        {
            consumer.Read(this);
        }

        public ITargetNumber TargetNumber => Call.TargetNumber;
        public CallData CallData => Call.CallData;

        protected bool Equals(ExternalCall other)
        {
            return Equals(Call, other.Call);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExternalCall)obj);
        }

        public override int GetHashCode()
        {
            return (Call != null ? Call.GetHashCode() : 0);
        }

        public override string ToString() => $"{GetType().Name}: {Call}";
    }
}