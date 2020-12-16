namespace gestionecentralino.Core.Lines.Data
{
    public static class CallSource
    {
        public static ICallLine CreateCall(ITargetNumber phoneNumber, ICallLine call) => phoneNumber.Value.Length > 3
            ? (ICallLine) new ExternalCall(call)
            : new InternalCall(call);
    }
}