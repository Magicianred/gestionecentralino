using gestionecentralino.Core.Lines.Data;

namespace gestionecentralino.Core.Lines
{
    public interface ICallLine: ICentralinoLine
    {
        ITargetNumber TargetNumber { get; }

        CallData CallData { get; }
    }
}