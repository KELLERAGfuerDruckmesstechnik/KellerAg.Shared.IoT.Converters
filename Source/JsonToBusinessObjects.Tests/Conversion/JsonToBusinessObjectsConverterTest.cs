namespace JsonToBusinessObjects.Tests.Conversion
{
    using System.Linq;
    using FluentAssertions;
    using JsonToBusinessObjects.Conversion;
    using Xunit;

    public class JsonToBusinessObjectsConverterTest
    {
        public JsonToBusinessObjectsConverterTest()
        {
            this.testee = new JsonToBusinessObjectsConverter();
        }

        private readonly JsonToBusinessObjectsConverter testee;

        [Fact]
        public void WhenGivenAValidConfigurationJson_ThenNoErrorsAreReported()
        {
            ConversionResult result = this.testee.Convert(TestConfigMessage);

            result.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void WhenGivenAValidConfigurationJson_ThenAlarmingDataIscorrect()
        {
            ConversionResult result = this.testee.Convert(TestConfigMessage);

            result.BusinessObjectRoot.Configuration.MeasurementSettings.MeasureAndSaveCH0_7.Should().Be(247);
            result.BusinessObjectRoot.Configuration.MeasurementSettings.MeasureAndSaveCH8_15.Should().Be(3);
            result.BusinessObjectRoot.Configuration.MeasurementSettings.AlarmChannelNumber.Should().Be(7);
            result.BusinessObjectRoot.Configuration.MeasurementSettings.AlarmType.Should().Be(1);
            result.BusinessObjectRoot.Configuration.MeasurementSettings.SendAlarmXTimes.Should().Be(2);

            result.BusinessObjectRoot.Configuration.FloatingPointMeasurementSettings.AlarmOn.Should().Be(26);
            result.BusinessObjectRoot.Configuration.FloatingPointMeasurementSettings.AlarmOff.Should().Be(35);
            result.BusinessObjectRoot.Configuration.FloatingPointMeasurementSettings.AlarmDelta.Should().Be(0.5f);
        }

        [Fact]
        public void WhenGivenAValidJson_ThenNoErrorsAreReported()
        {
            ConversionResult result = this.testee.Convert(TestMeasurementMessage);

            result.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void WhenGivenJsonHasPropertyNameLongerThanOneCharacter_ThenAFatalErrorIsReported()
        {
            ConversionResult result = this.testee.Convert(@"{""INVALID"": { ""e"": ""0"" } }");

            result.HasErrors.Should().BeTrue();
            result.ConversionMessages.FatalErrors.Single().Message.Should().Be("The given root object contains a property that has a length other than one character: 'INVALID' (Length 7)");
        }

        [Fact]
        public void WhenGivenJsonHasPropertyNameShorterThanOneCharacter_ThenAFatalErrorIsReported()
        {
            ConversionResult result = this.testee.Convert(@"{"""": { ""e"": ""0"" } }");

            result.HasErrors.Should().BeTrue();
            result.ConversionMessages.FatalErrors.Single().Message.Should().Be("The given root object contains a property that has a length other than one character: '' (Length 0)");
        }

        [Fact]
        public void WhenGivenJsonHasPropertyNameWithOneCharacterLengthButIsUnknown_ThenAnErrorIsReported()
        {
            ConversionResult result = this.testee.Convert(@"{""$"": { ""e"": ""0"" } }");

            result.HasWarnings.Should().BeTrue();
            result.ConversionMessages.Warnings.Single().Message.Should().Be(@"No command rule found to handle command with commandCharacter '$' (Data:'{""e"":""0""}')");
        }

        private const string TestConfigMessage = @"
{
  ""F"": {
    ""d"": ""0""
  },
  ""T"": {
    ""s"": ""549557930"",
    ""p"": ""17.05.31,16:59:30+08""
  },
  ""M"": {
    ""a"": ""-0.9665307-0.0004150+OFL+29.422731+OFL+0.9661099+30.579997+0.0009155+0.0016021"",
    ""c"": ""+1+1"",
    ""d"": ""-0.9665307-0.0004150+OFL+OFL+29.422731+OFL+0.9661099+30.579997+0.0009155+0.0016021""
  },
  ""I"": {
    ""n"": ""112233"",
    ""s"": ""17"",
    ""b"": ""99"",
    ""e"": ""9.20"",
    ""f"": ""17.21"",
    ""h"": ""36"",
    ""v"": ""+3.576""
  },
  ""a"": {
    ""a"": ""gprs.swisscom.ch"",
    ""b"": ""gprs"",
    ""c"": ""gprs"",
    ""d"": ""000.000.000.000"",
    ""e"": ""ARC-1"",
    ""f"": ""datamanager_103@gsmdata.ch"",
    ""g"": ""secretPassword"",
    ""h"": ""datamanager_103@gsmdata.ch"",
    ""i"": ""secretPassword"",
    ""j"": ""pop.gsmdata.ch"",
    ""k"": ""110"",
    ""l"": ""smtp.gsmdata.ch"",
    ""m"": ""25"",
    ""n"": ""p.schlegel@keller-druck.ch""
  },
  ""b"": {
    ""a"": ""p.schlegel@keller-druck.ch"",
    ""b"": ""p.schlegel@keller-druck.ch"",
    ""c"": ""p.schlegel@keller-druck.ch"",
    ""g"": ""SMS Pass"",
    ""j"": ""1234"",
    ""k"": """",
    ""m"": ""+41792657769"",
    ""n"": ""+41792657769"",
    ""o"": ""+41792657769"",
    ""q"": ""+41794999000"",
    ""r"": ""Winterthur KellerAG"",
    ""s"": ""+41792657766"",
    ""t"": ""ARC-1 112233"",
    ""u"": ""This is a Measurement TEXT"",
    ""v"": ""This is a Alarm TEXT"",
    ""w"": ""This is a Check TEXT"",
    ""0"": ""8.7475439"",
    ""1"": ""47.4984244"",
    ""2"": ""450.000""
  },
  ""c"": {
    ""a"": ""549557902"",
    ""b"": ""549643500"",
    ""c"": ""549643500"",
    ""d"": ""549643500"",
    ""e"": ""549556500"",
    ""g"": ""60"",
    ""h"": ""86400"",
    ""i"": ""86400"",
    ""j"": ""86400"",
    ""k"": ""86400"",
    ""m"": ""247"",
    ""n"": ""0"",
    ""o"": ""5"",
    ""p"": ""3"",
    ""q"": ""5"",
    ""r"": ""7"",
    ""s"": ""1"",
    ""t"": ""2"",
    ""v"": ""6"",
    ""w"": ""4"",
    ""x"": ""8"",
    ""y"": ""15"",
    ""z"": ""240"",
    ""0"": ""0"",
    ""1"": ""0"",
    ""2"": ""0"",
    ""3"": ""0"",
    ""4"": ""4"",
    ""5"": ""10"",
    ""6"": ""5"",
    ""7"": ""0"",
    ""8"": ""0"",
    ""9"": ""0""
  },
  ""f"": {
    ""a"": ""549556500"",
    ""g"": ""86400"",
    ""h"": ""86400"",
    ""m"": ""0"",
    ""n"": ""0"",
    ""o"": ""0"",
    ""q"": ""5"",
    ""z"": ""8"",
    ""3"": ""3""
  },
  ""d"": {
    ""a"": ""+26.000000"",
    ""b"": ""+35.000000"",
    ""c"": ""+0.5000000"",
    ""f"": ""+1.0000000"",
    ""g"": ""+1.0000000"",
    ""i"": ""+0.0000000"",
    ""j"": ""+0.0000000"",
    ""k"": ""+0.0000000"",
    ""m"": ""+0.0000000"",
    ""n"": ""+50.000000"",
    ""o"": ""+400.00000"",
    ""p"": ""+0.0000000"",
    ""q"": ""+998.20000"",
    ""r"": ""+0.0000000"",
    ""s"": ""+90.000000"",
    ""t"": ""+0.0000000"",
    ""u"": ""+0.0000000"",
    ""v"": ""+0.0000000"",
    ""w"": ""+0.0000000"",
    ""0"": ""+8.7475440"",
    ""1"": ""+47.498425"",
    ""2"": ""+450.00000""
  },
  ""k"": {
    ""a"": ""ftp.gsmdata.ch"",
    ""b"": ""datamanager_103@gsmdata.ch"",
    ""c"": ""secretPassword"",
    ""d"": ""ARC-1"",
    ""e"": ""21"",
    ""f"": ""21"",
    ""g"": ""0"",
    ""h"": ""ARC-1""
  },
  ""O"": {
    ""g"": ""0""
  },
  ""E"": {
    ""e"": """"
  }
}
";   
            
            
            private const string TestMeasurementMessage =
            @"{
  ""F"": {
    ""e"": ""0""
  },
  ""C"": {
    ""a"": ""2148"",
    ""b"": ""2""
  },
  ""T"": {
    ""s"": ""549559786"",
    ""p"": ""17.05.31,17:30:25+08""
  },
  ""M"": {
    ""a"": ""-0.9658963-0.0001001+OFL+29.549683+OFL+0.9657700+31.139997+0.0013732+0.0012207"",
    ""c"": ""+1+1""
  },
  ""I"": {
    ""n"": ""112233"",
    ""s"": ""17"",
    ""b"": ""99"",
    ""e"": ""9.20"",
    ""f"": ""17.21"",
    ""h"": ""35"",
    ""v"": ""+3.555""
  },
  ""B"": {
    ""a"": ""CGIgwZ2jAAAg////QEHsZVD///9gP3c8cEH5HoA6tACQOqAA//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8=""
  },
  ""E"": {
    ""e"": """"
  },
  ""X"": {
    ""a"": ""61416""
  }
}";
    }
}