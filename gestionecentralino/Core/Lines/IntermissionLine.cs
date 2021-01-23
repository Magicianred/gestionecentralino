namespace gestionecentralino.Core.Lines
{
    public class IntermissionLine : ICentralinoLine
    {
        private readonly string _line;

        protected bool Equals(IntermissionLine other)
        {
            return _line == other._line;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IntermissionLine) obj);
        }

        public override int GetHashCode()
        {
            return (_line != null ? _line.GetHashCode() : 0);
        }

        public IntermissionLine(string line)
        {
            _line = line;
        }

        public LineTypeEnum LineType => LineTypeEnum.Intermission;
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