namespace gestionecentralino.Core.Lines.Data
{
    public class CoCode
    {
        public string Value { get; }

        public CoCode(string value)
        {
            Value = value;
        }

        protected bool Equals(CoCode other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CoCode) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}