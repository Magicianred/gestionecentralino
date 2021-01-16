using System;
using System.Collections.Generic;
using System.Globalization;
using gestionecentralino.Core.Lines;
using gestionecentralino.Core.Lines.Data;
using LanguageExt.Common;
using LanguageExt.UnitTesting;
using static LanguageExt.Prelude;
using Xunit;

namespace gestionecentralino.Tests.Unit
{
    public class LinesTests
    {
        public static IEnumerable<object[]> ExpectedDataLines =>
            new List<object[]>
            {
                new object[] { 
                    "07/12/20 11:00    401 05 <I>234               00:00'12 EU00000.00               ", 
                    new InternalCall(new IncomingCall(
                        new DateTime(2020, 12, 7, 11, 0, 0), 
                        new InternalNumber("401"),
                        new IncomingNumber("234"),
                        new CoCode("05"),
                        None,
                        new Duration(TimeSpan.FromSeconds(12)),
                        new Cost(new Eur(0))
                    )) },
                
                new object[] {
                    "07/12/20 11:22    401 05 234                  00:00'00 EU10000.00            NA ",
                    new InternalCall(new OutgoingCall(
                        new DateTime(2020, 12, 7, 11, 22, 0),
                        new InternalNumber("401"),
                        new OutgoingNumber("234"),
                        new CoCode("05"),
                        new CdCode("NA"),
                        new Duration(TimeSpan.FromSeconds(0)),
                        new Cost(new Eur(10000))
                    )) },

                new object[] {
                    "07/12/20 11:18    405 21 <I>0364591759        00:00'33 EU00200.00               ",
                    new ExternalCall(new IncomingCall(
                        new DateTime(2020, 12, 7, 11, 18, 0),
                        new InternalNumber("405"),
                        new IncomingNumber("0364591759"),
                        new CoCode("21"),
                        None,
                        new Duration(TimeSpan.FromSeconds(33)),
                        new Cost(new Eur(200))
                    )) },

                new object[] {
                    "07/12/20 11:18    405 21 0364591759           00:00'33 EU00200.00               ",
                    new ExternalCall(new OutgoingCall(
                        new DateTime(2020, 12, 7, 11, 18, 0),
                        new InternalNumber("405"),
                        new OutgoingNumber("0364591759"),
                        new CoCode("21"),
                        None,
                        new Duration(TimeSpan.FromSeconds(33)),
                        new Cost(new Eur(200))
                    )) }
            };

        [Theory]
        [InlineData("04/12/20 17:18    401 01 <I>3488677149        00:00'12 EU00000.00               ", LineTypeEnum.Call)]
        [InlineData("\f  Date     Time   Ext CO Dial Number          Duration Cost       Acc code   CD ", LineTypeEnum.Heading)]
        [InlineData("--------------------------------------------------------------------------------", LineTypeEnum.Intermission)]
        [InlineData("04/12/20 16:00    401 05 <I>234               00:00'12 EU00000.00               ", LineTypeEnum.Call)]
        [InlineData("07/12/20 11:01    401 06 234                  00:00'13 EU00000.00               ", LineTypeEnum.Call)]
        [InlineData("07/12/20 11:22    401 05 <I>234               00:00'00 EU00000.00            NA ", LineTypeEnum.Call)]
        [InlineData("10/12/20 16:09    402    EXT401                                                 ", LineTypeEnum.Forward)]
        public void GIVEN_aLine_WHEN_parsed_THEN_itShouldBeIdentifiedAsTheCorrectType(string inputLine, LineTypeEnum expectedType)
        {
            CentralinoLine.Parse(inputLine).ShouldBeRight(line =>
            {
                Assert.Equal(expectedType, line.LineType);
            });
        }

        [Fact]
        public void GIVEN_anInvalidDataLine_WHEN_parsed_THEN_itShouldReturn_AnError()
        {
            CentralinoLine.Parse("banana").ShouldBeLeft();
        }

        [Theory, MemberData(nameof(ExpectedDataLines))]
        public void GIVEN_aDataLine_WHEN_parsed_THEN_itShouldBePossible_ToParse_AllData(string inputLine, ICentralinoLine expectedLine)
        {
            CallLine.Of(inputLine).ShouldBeSuccess(line => Assert.Equal(expectedLine, line));
        }

        [Theory]
        [InlineData("012345678901234")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("07/12/20 11:00    401 05 <I>234               00:bla 2 EU00000.00               ")]
        [InlineData("07/12/20 11:00    401 05 <I>234               00:00'12 EUxx000.00               ")]
        [InlineData("07/12/20 11:00    401 05 <I>234               00100'12 EUxx000.00               ")]
        [InlineData("07/12/20 11:00    401 05 <I>234               00:00212 EUxx000.00               ")]
        public void GIVEN_invalidFormats_WHEN_parsed_THEN_itShouldSignal_Errors(string inputLine)
        {
            CallLine.Of(inputLine).ShouldBeFail();
        }

        [Fact]
        public void GIVEN_anInvalidLine_WithManyError_WHEN_parsed_THEN_itShouldReturn_AllTheErrors()
        {

            CallLine.Of("07/1xx20 11:00    401 05 <I>234               00:bla 2 EU00000.00               ").ShouldBeFail(
                errors =>
                {
                    Assert.Equal(2, errors.Count);
                });
        }
    }
}
