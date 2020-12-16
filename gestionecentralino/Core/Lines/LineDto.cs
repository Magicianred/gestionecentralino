using LanguageExt;
using LanguageExt.Common;

namespace gestionecentralino.Core.Lines
{
    public class LineDto
    {
        public static Either<Error, LineDto> Of(string line)
        {
            var minimumLength = 70;
            if (line.Length < minimumLength)
            {
                return Prelude.Left(Error.New($"line ${line} should be at least ${minimumLength} characters long"));
            }

            var lineDto = new LineDto
            (
                dateTimeStr: ReadLineToken(line, 0, 14),
                internalNumberStr: ReadLineToken(line, 14, 7),
                coCodeStr: ReadLineToken(line, 21, 3),
                externalNumberStr: ReadLineToken(line, 24, 21),
                durationStr: ReadLineToken(line, 45, 9),
                costStr: ReadLineToken(line, 54, 11),
                cdCodeStr: ReadLineToken(line, 65, 15)
            );

            return Prelude.Right(lineDto);
        }

        private static string ReadLineToken(string line, int startIndex, int length)
        {
            return line.Substring(startIndex, length).Trim();
        }

        private LineDto(string cdCodeStr, string costStr, string durationStr, string externalNumberStr, string coCodeStr, string internalNumberStr, string dateTimeStr)
        {
            CdCodeStr = cdCodeStr;
            CostStr = costStr;
            DurationStr = durationStr;
            ExternalNumberStr = externalNumberStr;
            CoCodeStr = coCodeStr;
            InternalNumberStr = internalNumberStr;
            DateTimeStr = dateTimeStr;
        }

        public string CdCodeStr { get; }

        public string CostStr { get; }

        public string DurationStr { get; }

        public string ExternalNumberStr { get; }

        public string CoCodeStr { get; }

        public string InternalNumberStr { get; }

        public string DateTimeStr { get; }
    }
}