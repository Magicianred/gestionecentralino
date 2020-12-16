using System;
using System.Globalization;
using System.Linq;
using gestionecentralino.Core.Lines.Data;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace gestionecentralino.Core.Lines
{
    public static class CallLine
    {
        public static Validation<Error, ICentralinoLine> Of(string line)
        {
            return LineDto.Of(line).ToValidation().Bind(lineDto =>
            {
                var internalNumber = new InternalNumber(lineDto.InternalNumberStr);
                var externalNumber = ExternalNumber.Of(lineDto.ExternalNumberStr);
                var coCode = new CoCode(lineDto.CoCodeStr);
                var cdCode = string.IsNullOrWhiteSpace(lineDto.CdCodeStr) ? None : Some(new CdCode(lineDto.CdCodeStr));

                Validation<Error, DateTime> dateTimeValue = ReadDate(lineDto.DateTimeStr);
                Validation<Error, Cost> costValue = ReadCost(lineDto.CostStr);
                Validation<Error, Duration> durationValue = ReadDuration(lineDto.DurationStr);

                return (dateTimeValue, costValue, durationValue).Apply((dateTime, cost, duration) => 
                    (ICentralinoLine)externalNumber
                    .CreateCall(new CallData(
                        dateTime,
                        internalNumber,
                        coCode,
                        cdCode,
                        duration,
                        cost)));
            });
        }

        
        private static Validation<Error, DateTime> ReadDate(string valueStr)
        {
            bool parsed = DateTime.TryParseExact(
                valueStr,
                "dd/MM/yy hh:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateTimeValue);

            return parsed
                ? Success<Error, DateTime>(dateTimeValue)
                : Fail<Error, DateTime>(Error.New($"date time ${valueStr} is not a valid date"));
        }

        private static Validation<Error, Eur> ReadEur(string valueStr)
        {
            bool parsed = decimal.TryParse(valueStr.Substring(2), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var costValue);
            return parsed
                ? Success<Error, Eur>(new Eur(costValue))
                : Fail<Error, Eur>(Error.New($"cost ${costValue} is not a valid currency format"));
        }

        private static Validation<Error, Cost> ReadCost(string valueStr) => ReadEur(valueStr).Map(eur => new Cost(eur));

        private static Validation<Error, Duration> ReadDuration(string valueStr)
        {
            TimeSpan? durationValue = ParseTime(valueStr);

            return durationValue.HasValue
                ? Success<Error, Duration>(new Duration(durationValue.Value))
                : Fail<Error, Duration>(Error.New($"duration ${valueStr} is not a valid duration format that should be hh:mm'ss"));
        }


        private static TimeSpan? ParseTime(string timeStr)
        {
            try
            {
                var timeTokens = timeStr.Split(new[] {':', '\''}).Select(int.Parse).ToArray();
                if (timeTokens.Length < 3) return null;
                return new TimeSpan(0, timeTokens[0], timeTokens[1], timeTokens[2]);
            }
            catch (FormatException e)
            {
                return null;
            }
        }
    }
}