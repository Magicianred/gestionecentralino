namespace gestionecentralino.Core.Lines.Data
{
    public class Cost
    {
        public Eur Value { get; }

        public Cost(Eur value)
        {
            Value = value;
        }

        protected bool Equals(Cost other)
        {
            return Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Cost) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}