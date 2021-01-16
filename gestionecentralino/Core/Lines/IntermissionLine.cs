namespace gestionecentralino.Core.Lines
{
    public class IntermissionLine : ICentralinoLine
    {
        private readonly string _line;

        public IntermissionLine(string line)
        {
            _line = line;
        }

        public LineTypeEnum LineType => LineTypeEnum.Intermission;
        public void Apply(ICentralinoLineConsumer consumer)
        {
            consumer.Read(this);
        }
    }
}