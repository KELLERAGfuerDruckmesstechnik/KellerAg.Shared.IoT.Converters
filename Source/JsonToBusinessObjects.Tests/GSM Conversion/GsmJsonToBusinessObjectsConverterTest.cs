namespace JsonToBusinessObjects.Tests.GSM_Conversion
{
    using System.Linq;
    using FluentAssertions;
    using JsonToBusinessObjects.Conversion;
    using Xunit;
    using JsonToBusinessObjects.Conversion.GsmArc;

    public class GsmJsonToBusinessObjectsConverterTest
    {
        public GsmJsonToBusinessObjectsConverterTest()
        {
            this.testee = new JsonToBusinessObjectsConverterForGsm();
        }

        private readonly JsonToBusinessObjectsConverterForGsm testee;

        [Fact]
        public void WhenGivenAValidConfigurationJson_ThenNoErrorsAreReported()
        {
            ConversionResult result = this.testee.Convert(TestConfigMessage);

            result.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void WhenGivenAValidConfigurationJson_ThenAlarmingDataIsCorrect()
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
        public void WhenGivenAnInvalidJsonWithBrokenN_ThenNoErrorsAreReported2()
        {
            ConversionResult result = this.testee.Convert(TestMeasurementMessage2);

            result.HasErrors.Should().BeTrue();
        }

        [Fact]
        public void WhenGivenJsonHasPropertyNameLongerThanOneCharacter_ThenAFatalErrorIsReported()
        {
            ConversionResult result = this.testee.Convert(@"{""INVALID"": { ""e"": ""0"" } }");

            result.HasErrors.Should().BeTrue();
            result.ConversionMessages.FatalErrors.Single().Message.Should()
                .Be("The given root object contains a property that has a length other than one character: 'INVALID' (Length 7)");
        }

        [Fact]
        public void WhenGivenJsonHasPropertyNameShorterThanOneCharacter_ThenAFatalErrorIsReported()
        {
            ConversionResult result = this.testee.Convert(@"{"""": { ""e"": ""0"" } }");

            result.HasErrors.Should().BeTrue();
            result.ConversionMessages.FatalErrors.Single().Message.Should()
                .Be("The given root object contains a property that has a length other than one character: '' (Length 0)");
        }

        [Fact]
        public void WhenGivenJsonHasPropertyNameWithOneCharacterLengthButIsUnknown_ThenAnErrorIsReported()
        {
            ConversionResult result = this.testee.Convert(@"{""$"": { ""e"": ""0"" } }");

            result.HasWarnings.Should().BeTrue();
            result.ConversionMessages.Warnings.Single().Message.Should().Be(@"No command rule found to handle command with commandCharacter '$' (Data:'{""e"":""0""}')");
        }


        [Fact]
        public void WhenGivenAHugeJsonWithRequestedData_ThenAnNoErrorIsReported()
        {
            ConversionResult result = this.testee.Convert(@"{""F"":{""f"":""0""},""C"":{""a"":""6839"",""b"":""30""},""T"":{""s"":""633153659"",""p"":""20.01.24,07:00:32+12""},""I"":{""n"":""410"",""s"":""13"",""b"":""95"",""e"":""9.20"",""f"":""19.05"",""h"":""38"",""v"":""+3.887""},""O"":{""g"":""0""},""B"":{""a"":""LlklsJr6AAAQQjDXMP///0BBacXwAAEsEEIw1zD///9AQWXf8AABLBBCMNcw////QEFifPAAASwQQjDYMP///y5ZJbCefgAAQEFekvAAASwQQjDZMP///0BBWu3wAAEsEEIw2zD///9AQVeG8AABLBBCMNsw////QEFTmf////8uWSWwoy4AABBCMNww////QEFP7vAAASwQQjDcMP///0BBTMnwAAEsEEIw3DD///9AQUkc8AABLBBCMN0w////LlklsKayAABAQUVq8AABLBBCMN0w////QEFB//AAASwQQjDfMP///0BBPkvwAAEsEEIw3zD///9AQTqa/////y5ZJbCrYgAAEEIw3zD///9AQTds8AABLBBCMN8w////QEEzc/AAASwQQjDgMP///0BBMIfwAAEsEEIw4DD///8uWSWwruYAAEBBLVbwAAEsEEIw4DD///9AQSlY8AABLBBCMOAw////QEEmJvAAASwQQjDhMP///0BBIq3/////LlklsLOWAAAQQjDhMP///0BBH3nwAAEsEEIw4jD///9AQRwB8AABLBBCMOIw////QEEZDvAAASwQQjDiMP///y5ZJbC3GgAAQEEV1vAAASwQQjDjMP///0BBEybwAAEsEEIw4jD///9AQQ+o8AABLBBCMOMw////QEENPP////8uWSWwu8oAABBCMOMw////QEEJuvAAASwQQjDjMP///0BBB9fwAAEsEEIw4zD///9AQQSa8AABLBBCMOQw////LlklsL9OAABAQQK38AABLBBCMOQw////QEEAjPAAASwQQjDlMP///0BA/dTwAAEsEEIw5TD///9AQPmG/////y5ZJbDD/gAAEEIw5jD///9AQPWy8AABLBBCMOYw////QEDydPAAASwQQjDnMP///0BA7qrwAAEsEEIw5zD///8uWSWwx4IAAEBA62LwAAEsEEIw5zD///9AQOZ+8AABLBBCMOcw////QEDiIvAAASwQQjDnMP///0BA3t7/////LlklsMwyAAAQQjDoMP///0BA2w7wAAEsEEIw6DD///9AQNc68AABLBBCMOgw////QEDTaPAAASwQQjDpMP///y5ZJbDPtgAAQEDQIPAAASwQQjDpMP///0BAzErwAAEsEEIw6TD///9AQMfu8AABLBBCMOow////QEDFMv////8uWSWw1GYAABBCMOsw////QEDB5PAAASwQQjDpMP///0BAvg7wAAEsEEIw6jD///9AQLvY8AABLBBCMOkw////LlklsNfqAABAQLmo8AABLBBCMOkw////QEC4AvAAASwQQjDqMP///0BAtlrwAAEsEEIw6TD///9AQLOg/////y5ZJbDcmgAAEEIw6TD///9AQLMS8AABLBBCMOow////QECx+vAAASwQQjDqMP///0BAsOLwAAEsEEIw6jD///8uWSWw4B4AAEBArzjwAAEsEEIw6TD///9AQK0C8AABLBBCMOow////QECrXvAAASwQQjDrMP///0BAqSb/////LlklsOTOAAAQQjDqMP///0BAp37wAAEsEEIw6jD///9AQKS88AABLBBCMOow////QECiiPAAASwQQjDqMP///y5ZJbDoUgAAQECfyPAAASwQQjDpMP///0BAnQbwAAEsEEIw6jD///9AQJrS8AABLBBCMOkw////QECXfP////8uWSWw7QIAABBCMOkw////QECVSvAAASwQQjDpMP///0BAk57wAAEsEEIw6DD///9AQJBK8AABLBBCMOcw////LlklsPCGAABAQI2G8AABLBBCMOYw////QECKwvAAASwQQjDlMP///0BAiIjwAAEsEEIw5TD///9AQIZS/////y5ZJbD1NgAAEEIw4zD///9AQIL88AABLBBCMOMw////QECBUPAAASwQQjDiMP///0BAfQzwAAEsEEIw4jD///8uWSWw+LoAAEBAd4DwAAEsEEIw4TD///9AQHH88AABLBBCMOEw////QEBupPAAASwQQjDhMP///0BAaQz/////LlklsP1qAAAQQjDgMP///0BAZJDwAAEsEEIw3zD///9AQGAk8AABLBBCMN4w////QEBcyPAAASwQQjDeMP///y5ZJbEA7gAAQEBYUPAAASwQQjDdMP///0BAVPjwAAEsEEIw3jD///9AQFT48AABLBBCMN0w////QEBPYP////8uWSWxBZ4AABBCMN4w////QEBQiPAAASwQQjDcMP///0BASuzwAAEsEEIw3DD///9AQEi08AABLBBCMNww////LlklsQkiAABAQEZw8AABLBBCMNww////QEBEOPAAASwQQjDbMP///0BAQfjwAAEsEEIw2jD///9AQD6c/////y5ZJbEN0gAAEEIw2zD///9AQDxk8AABLBBCMNsw////QEA6KPAAASwQQjDaMP///0BANsTwAAEsEEIw2jD///8uWSWxEVYAAEBAM2jwAAEsEEIw2TD///9AQDEs8AABLBBCMNow////QEAu9PAAASwQQjDaMP///0BALLj/////""},""E"":{""e"":""""},""X"":{""a"":""31543""}}");

            result.HasWarnings.Should().BeTrue();
            result.HasErrors.Should().BeFalse();
        }


        private const string TestMeasurementMessage2 = @"{
    ""F"": {
        ""d"": ""0""
    },
    ""T"": {
        ""s"": ""587487199"",
        ""p"": ""18.08.13,16:52:56+08""
    },
    ""M"": {
        ""a"": ""-0.0003749+27.850710"",
        ""c"": ""+1+1"",
        ""d"": ""-0.0003749+0.9685400+0.9682630+27.850710+27.974607+27.554687+0.9689149+29.250000""
    },
    ""I"": {
        ""n"": """"
    },
    ""a"": {
        ""a"": ""gprs.swisscom.ch"",
        ""b"": ""gprs"",
        ""c"": ""gprs"",
        ""d"": ""000.000.000.000"",
        ""e"": ""GSM2"",
        ""f"": ""datamanager_39@gsmdata.ch"",
        ""g"": ""secretPassword2"",
        ""h"": ""datamanager_39@gsmdata.ch"",
        ""i"": ""secretPassword2"",
        ""j"": ""pop.gsmdata.ch"",
        ""k"": ""110"",
        ""l"": ""smtp.gsmdata.ch"",
        ""m"": ""587"",
        ""n"": ""gsm_12@gsmdata.ch""
    },
    ""b"": {
        ""a"": ""datamanager_12@gsmdata.ch"",
        ""b"": ""datamanager_12@gsmdata.ch"",
        ""c"": ""datamanager_12@gsmdata.ch"",
        ""g"": ""bsaue"",
        ""j"": ""1717"",
        ""k"": ""+41796131929"",
        ""m"": ""+41796131929"",
        ""n"": ""+41796131929"",
        ""o"": ""+41796131929"",
        ""q"": ""+41794999000"",
        ""r"": ""BSAUE"",
        ""s"": ""+41794444713"",
        ""t"": ""0000001655"",
        ""u"": ""Messung"",
        ""v"": ""Alarmtext"",
        ""w"": ""messe"",
        ""0"": ""0"",
        ""1"": ""0"",
        ""2"": ""0.000""
    },
    ""c"": {
        ""a"": ""587487600"",
        ""b"": ""306712800"",
        ""c"": ""306712800"",
        ""d"": ""587539800"",
        ""e"": ""306712800"",
        ""g"": ""3600"",
        ""h"": ""86400"",
        ""i"": ""86400"",
        ""j"": ""86400"",
        ""k"": ""86400"",
        ""m"": ""9"",
        ""n"": ""0"",
        ""o"": ""1"",
        ""p"": ""0"",
        ""q"": ""72"",
        ""r"": ""1"",
        ""s"": ""1"",
        ""t"": ""1"",
        ""v"": ""6"",
        ""w"": ""5"",
        ""x"": ""8"",
        ""y"": ""9"",
        ""z"": ""98"",
        ""0"": ""0"",
        ""1"": ""0"",
        ""2"": ""0"",
        ""3"": ""0"",
        ""4"": ""0"",
        ""5"": ""7"",
        ""6"": ""3"",
        ""7"": ""0"",
        ""8"": ""1"",
        ""9"": ""1""
    },
    ""f"": {
        ""a"": ""312328800"",
        ""g"": ""60"",
        ""h"": ""60"",
        ""m"": ""0"",
        ""n"": ""1"",
        ""o"": ""1"",
        ""q"": ""1"",
        ""z"": ""11"",
        ""3"": ""0""
    },
    ""d"": {
        ""a"": ""+15.000000"",
        ""b"": ""+14.000000"",
        ""c"": ""+0.0000000"",
        ""f"": ""+1.0000000"",
        ""g"": ""+1.0000000"",
        ""i"": ""+0.0030000"",
        ""j"": ""+0.0010000"",
        ""k"": ""+1.0000000"",
        ""m"": ""+1.0000000"",
        ""n"": ""+15.580000"",
        ""o"": ""+0.0000000"",
        ""p"": ""+0.0000000"",
        ""q"": ""+998.20000"",
        ""r"": ""+1.0000000"",
        ""s"": ""+0.0000000"",
        ""t"": ""+0.0000000"",
        ""u"": ""+0.0000000"",
        ""v"": ""+987654290"",
        ""w"": ""+987654290"",
        ""0"": ""+0.0000000"",
        ""1"": ""+0.0000000"",
        ""2"": ""+0.0000000""
    },
    ""k"": {
        ""a"": ""ftp.gsmdata.ch"",
        ""b"": ""datamanager_39@gsmdata.ch"",
        ""c"": ""secretPassword2"",
        ""d"": ""datamanager_39@gsmdata.ch"",
        ""e"": ""21"",
        ""f"": ""21"",
        ""g"": ""2000"",
        ""h"": """"
    },
    ""O"": {
        ""g"": ""52624""
    },
    ""E"": {
        ""e"": """"
    }
}
";

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
    ""n"": ""p.schlegel@keller-pressure.com""
  },
  ""b"": {
    ""a"": ""p.schlegel@keller-pressure.com"",
    ""b"": ""p.schlegel@keller-pressure.com"",
    ""c"": ""p.schlegel@keller-pressure.com"",
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