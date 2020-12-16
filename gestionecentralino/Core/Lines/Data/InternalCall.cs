namespace gestionecentralino.Core.Lines.Data
{
    public class InternalCall: ICallLine
    {
        private readonly ICallLine _call;

        public InternalCall(ICallLine call)
        {
            _call = call;
        }

        public LineTypeEnum LineType => _call.LineType;

        protected bool Equals(InternalCall other)
        {
            return Equals(_call, other._call);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InternalCall) obj);
        }

        public override int GetHashCode()
        {
            return (_call != null ? _call.GetHashCode() : 0);
        }
    }
}