namespace gestionecentralino.Core.Lines.Data
{
    public class Eur
    {
        public decimal Value { get; }

        public Eur(decimal value)
        {
            Value = value;
        }

        protected bool Equals(Eur other)
        {
            return Value == other.Value;
        }

        public override string ToString() => $"{Value} Euro";

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Eur) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}