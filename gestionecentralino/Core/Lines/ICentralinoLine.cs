namespace gestionecentralino.Core.Lines
{
    public interface ICentralinoLine
    {
        LineTypeEnum LineType { get; }

        void Apply(ICentralinoLineConsumer consumer);
    }
}