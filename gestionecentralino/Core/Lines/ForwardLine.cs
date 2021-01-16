namespace gestionecentralino.Core.Lines
{
    public class ForwardLine : ICentralinoLine
    {
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
    }
}