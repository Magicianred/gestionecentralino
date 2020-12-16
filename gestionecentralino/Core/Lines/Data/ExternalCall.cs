namespace gestionecentralino.Core.Lines.Data
{
    public class ExternalCall: ICallLine
    {
        private readonly ICallLine _call;

        public ExternalCall(ICallLine call)
        {
            _call = call;
        }

        protected bool Equals(ExternalCall other)
        {
            return Equals(_call, other._call);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExternalCall) obj);
        }

        public override int GetHashCode()
        {
            return (_call != null ? _call.GetHashCode() : 0);
        }

        public LineTypeEnum LineType => _call.LineType;
    }
}