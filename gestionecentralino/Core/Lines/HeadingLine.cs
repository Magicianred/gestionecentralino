namespace gestionecentralino.Core.Lines
{
    public class HeadingLine : ICentralinoLine
    {
        private readonly string _inputLine;

        public HeadingLine(string inputLine)
        {
            _inputLine = inputLine;
        }

        public LineTypeEnum LineType => LineTypeEnum.Heading;
        public void Apply(ICentralinoLineConsumer consumer)
        {
            consumer.Read(this);
        }

        protected bool Equals(HeadingLine other)
        {
            return _inputLine == other._inputLine;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HeadingLine) obj);
        }

        public override int GetHashCode()
        {
            return (_inputLine != null ? _inputLine.GetHashCode() : 0);
        }
    }
}