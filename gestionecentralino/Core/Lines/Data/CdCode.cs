namespace gestionecentralino.Core.Lines.Data
{
    public class CdCode
    {
        public string Value { get; }

        public CdCode(string value)
        {
            Value = value;
        }

        protected bool Equals(CdCode other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CdCode) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}