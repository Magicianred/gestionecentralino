using System;
using gestionecentralino.Core.Lines.Data;
using LanguageExt;

namespace gestionecentralino.Core.Lines
{
    public class CallData
    {
        public string OriginalLine { get; }
        public DateTime DateTime { get; }

        public InternalNumber InternalNumber { get; }


        public CoCode CoCode { get; }

        public Option<CdCode> CdCode { get; }

        public Duration Duration { get; }

        public Cost Cost { get; }

        public CallData(
            string originalLine, 
            DateTime dateTime,
            InternalNumber internalNumber,
            CoCode coCode,
            Option<CdCode> cdCode,
            Duration duration,
            Cost cost)
        {
            OriginalLine = originalLine;
            DateTime = dateTime;
            InternalNumber = internalNumber;
            CoCode = coCode;
            CdCode = cdCode;
            Duration = duration;
            Cost = cost;
        }

        public override string ToString()
        {
            return $"{nameof(DateTime)}: {DateTime}, {nameof(InternalNumber)}: {InternalNumber}, {nameof(CoCode)}: {CoCode}, {nameof(CdCode)}: {CdCode}, {nameof(Duration)}: {Duration}, {nameof(Cost)}: {Cost}";
        }

        protected bool Equals(CallData other)
        {
            return DateTime.Equals(other.DateTime) && Equals(InternalNumber, other.InternalNumber) && Equals(CoCode, other.CoCode) && CdCode.Equals(other.CdCode) && Equals(Duration, other.Duration) && Equals(Cost, other.Cost);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CallData) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DateTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (InternalNumber != null ? InternalNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CoCode != null ? CoCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ CdCode.GetHashCode();
                hashCode = (hashCode * 397) ^ (Duration != null ? Duration.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Cost != null ? Cost.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}