namespace gestionecentralino.Core.Lines
{
    public class ForwardLine : ICentralinoLine
    {
        protected bool Equals(ForwardLine other)
        {
            return _line == other._line;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ForwardLine) obj);
        }

        public override int GetHashCode()
        {
            return (_line != null ? _line.GetHashCode() : 0);
        }

        private readonly string _line;

        public ForwardLine(string line)
        {
            _line = line;
        }

        public LineTypeEnum LineType => LineTypeEnum.Forward;
        public void Apply(ICentralinoLineConsumer consumer)
        {
            consumer.Read(this);
        }

        public override string ToString()
        {
            return $"{nameof(LineType)}: {LineType}, {nameof(_line)}: {_line}";
        }
    }
}