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
    }
}