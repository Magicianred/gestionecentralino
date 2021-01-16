using gestionecentralino.Core.Lines.Data;

namespace gestionecentralino.Core.Lines
{
    public interface ICentralinoLineConsumer
    {
        void Read(OutgoingCall call);
        void Read(InternalCall call);
        void Read(IntermissionLine call);
        void Read(HeadingLine call);
        void Read(ForwardLine call);
        void Read(ExternalCall call);
        void Read(IncomingCall call);
    }
}