namespace gestionecentralino.Core.Lines.Data
{
    public static class ExternalNumber
    {
        const string IncomingMarker = "<I>";

        public static ITargetNumber Of(string numberValue)
        {
            return numberValue.StartsWith(IncomingMarker)
                ? (ITargetNumber) new IncomingNumber(numberValue.Replace(IncomingMarker, ""))
                : new OutgoingNumber(numberValue);
        }
    }
}