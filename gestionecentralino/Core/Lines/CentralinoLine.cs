using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace gestionecentralino.Core.Lines
{
    public static class CentralinoLine
    {
        public static Either<Seq<Error>, ICentralinoLine> Parse(string line)
        {
            ICentralinoLine maybeANonDataLine = IdentifyANonCallLine(line);
            return maybeANonDataLine != null 
                ? Right(maybeANonDataLine) 
                : CallLine.Of(line).ToEither();
        }

        private static ICentralinoLine IdentifyANonCallLine(string line)
        {
            if (line.StartsWith("\f"))
            {
                return new HeadingLine(line);
            }

            if (line.StartsWith("-"))
            {
                return new IntermissionLine(line);
            }

            if (line.Contains("EXT"))
            {
                return new ForwardLine(line);
            }

            return null;
        }
    }
}