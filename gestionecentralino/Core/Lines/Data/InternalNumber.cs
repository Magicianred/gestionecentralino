namespace gestionecentralino.Core.Lines.Data
{
    public class InternalNumber
    {
        public string Value { get; }

        public InternalNumber(string value)
        {
            Value = value;
        }

        protected bool Equals(InternalNumber other)
        {
            return Value == other.Value;
        }

        public override string ToString() => $"{Value}";

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InternalNumber) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}