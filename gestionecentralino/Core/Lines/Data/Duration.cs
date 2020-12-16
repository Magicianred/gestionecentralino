using System;

namespace gestionecentralino.Core.Lines.Data
{
    public class Duration
    {
        public TimeSpan Value { get; }

        public Duration(TimeSpan value)
        {
            Value = value;
        }

        protected bool Equals(Duration other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Duration) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}