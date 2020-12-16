using System;
using System.Collections.Generic;
using System.Linq;
using gestionecentralino.Core.Lines;
using LanguageExt;
using LanguageExt.Common;

namespace gestionecentralino.Core
{
    public class CentralinoLines
    {
        public Either<Seq<Error>, ICentralinoLine>[] Lines { get; }

        public static CentralinoLines Parse(string[] lines)
        {
            IEnumerable<Either<Seq<Error>, ICentralinoLine>> parsedLines = lines.Select(CentralinoLine.Parse);
            return new CentralinoLines(parsedLines.ToArray());
        }

        private CentralinoLines(Either<Seq<Error>, ICentralinoLine>[] lines)
        {
            Lines = lines;
        }

        public override string ToString()
        {
            return $"{nameof(Lines)}: {string.Join(Environment.NewLine, Lines.Map(Print))}";
        }

        private string Print(Either<Seq<Error>, ICentralinoLine> line) =>
            line
                .Map(centralinoLine => centralinoLine.ToString())
                .IfLeft(PrintError);

        private string PrintError(Seq<Error> errors) => errors.Aggregate("|", (acc, error) => $"{acc}{error.Message}");

        private bool Equals(CentralinoLines other)
        {
            return Lines.SequenceEqual(other.Lines);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CentralinoLines) obj);
        }

        public override int GetHashCode()
        {
            return (Lines != null ? Lines.GetHashCode() : 0);
        }
    }
}