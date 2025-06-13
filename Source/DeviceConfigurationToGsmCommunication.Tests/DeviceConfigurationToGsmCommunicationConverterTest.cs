using KellerAg.Shared.Entities.Database;
using Newtonsoft.Json;

namespace DeviceConfigurationToGsmCommunication.Tests
{
    using System;
    using Xunit;
    using FluentAssertions;
    using DeviceConfigurationToGsmCommunication.Exceptions;

    public class DeviceConfigurationToGsmCommunicationConverterTest
    {
        private readonly DeviceConfigurationToGsmCommunicationConverter testee = new DeviceConfigurationToGsmCommunicationConverter();

        [Fact]
        public void WhenTryingToConvertWithEmptyDeviceConfig_ThenExceptionIsThrown()
        {
            testee.Invoking(_ => _.Convert(new DatabaseDeviceSettings())).Should().Throw<EmptyGsmMessageException>();
        }

        [Fact]
        public void WhenTryingToConvertWithCompleteDeviceConfig_ThenFullTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                GeneralNetworkName = "Test Network Name",
                GeneralOwnTelNumber = "+41793893499",
                GeneralLocationName = "Hinter dem Haus",
                GeneralLocationAddressText = "Berninastrasse 9",
                GeneralGsmSerialNumber = "5483",
                GeneralGsmSoftwareVersion = "1.0.1",
                GeneralLongitudeText = "7.58831",
                GeneralLatitudeText = "5.38752",
                GeneralAltitudeText = "500",
                GeneralGpsLongitude = 7.58831m,
                GeneralGpsLatitude = 5.38752m,
                GeneralGpsAltitude = 500,
                GprsAPN = "New APN",
                GprsID = "546813",
                GprsPassword = "Password",
                GprsDNS = "dns.swisscom.ch",
                GprsSimPin = "6526",
                GprsSmsServiceCenterNr = "+41783456000",
                GprsPasswordForQuerySms = "QueryPW",
                GprsModemProtocol = 1,
                FtpServerName = "ftp.keller-druck.com",
                FtpServerPath = "folder",
                FtpUsername = "Username",
                FtpAccount = "AccountName",
                FtpPassword = "PasswordForFTP",
                FtpPort = 21,
                FtpUseTLS = false,
                FtpSourceControlPort = 65,
                FtpSourceDataPort = 66,
                FtpUseActiveMode = true,
                MailSmtpShowedName = "SMTP Name",
                MailPop3Username = "POP Username",
                MailPop3Password = "POP Password",
                MailSmtpUseSSL = true,
                MailPop3UseSSL = false,
                MailUseAlternativeSMTPLogin = true,
                MailReturnAddress = "doris.leuthard@gmail.com",
                MailOptSmtpUsername = "SMTP Username",
                MailOptSmtpPassword = "SMTP Password",
                MailSmtpServer = "smtp.keller-druck.ch",
                MailSmtpPort = 55,
                MailPop3Server = "pop.keller-druck.ch",
                MailPop3Port = 56,
                LockTimerMeasurement = true,
                LockTimerAlarm = false,
                LockTimerInfo = true,
                LockTimerCheck = false,
                LockTimerDial = true,
                MeasurementInterval = 68501,
                MeasurementSendSms = true,
                MeasurementSendMail = false,
                MeasurementSendFTP = true,
                MeasurementMailAddress = "measuerement@keller-druck.com",
                MeasurementSmsNumber = "+41793854745",
                MeasurementSmsText = "MEASURE",
                MeasurementSendMailAfterX = 20,
                MeasurementSendSmsAfterX = 21,
                MeasurementSendFtpAfterX = 22,
                InfoInterval = 68503,
                InfoSendSms = false,
                InfoSendMail = true,
                InfoSendFTP = false,
                InfoMailAddress = "info@keller-druck.com",
                InfoSmsNumber = "+49582164834",
                CheckInterval = 68506,
                CheckSendSms = true,
                CheckSendMail = false,
                CheckSendFTP = true,
                CheckAnswerSmsText = "ANSWER",
                AlarmInterval = 68511,
                AlarmSendSms = false,
                AlarmSendMail = true,
                AlarmSendFTP = false,
                AlarmMailAddress = "alarm@keller-druck.com",
                AlarmSmsNumber = "+652423842347",
                AlarmSmsText = "ALARM",
                AlarmSendXTimes = 50,
                AlarmType = 2,
                AlarmChannelNumber = 8,
                AlarmOnThreshold = 5.685m,
                AlarmOffThreshold = 7.354m,
                AlarmDeltaThreshold = 2.471m,
                EventType = 3,
                EventCheckInterval = 68521,
                EventMeasureInterval = 68522,
                EventChannel = 11,
                EventOnValueThreshold = 1.25m,
                EventOffValueThreshold = 94.21m,
                EventDeltaValueThreshold = 122.0m,
                EventSendMailXTimes = 15,
                HardwareConnectionType = 2,
                HardwarePowerExternalDevice = 4,
                HardwareMeasureSaveChannel0 = true,
                HardwareMeasureSaveChannel1 = true,
                HardwareMeasureSaveChannel2 = true,
                HardwareMeasureSaveChannel3 = false,
                HardwareMeasureSaveChannel4 = false,
                HardwareMeasureSaveChannel5 = true,
                HardwareMeasureSaveChannel6 = true,
                HardwareMeasureSaveChannel7 = false,
                HardwareMeasureSaveChannel8 = true,
                HardwareMeasureSaveChannel9 = false,
                HardwareMeasureSaveChannel10 = true,
                HardwareMeasureSaveChannel11 = false,
                HardwareMeasureSaveChannel12 = true,
                HardwareMeasureSaveChannel13 = false,
                HardwareMeasureSaveChannel14 = true,
                HardwareMeasureSaveChannel15 = false,
                HardwareDataConnectionInterval = 68531,
                HardwareSupportedSensorTypes = 5,
                HardwarePreOnTime = 250,
                HardwareMultiplierPressureChannels = 5.21m,
                HardwareMultiplierTemperatureChannels = 7.14m,
                HardwareResolutionPressureChannels = 6,
                HardwareResolutionTemperatureChannels = 4,
                HardwareDataConnectionCallNumber = "+5821347865",
                WaterLevelCalculationEnable = 1,
                WaterLevelCalculationLength = -50.001m,
                WaterLevelCalculationHeight = 10.47m,
                WaterLevelCalculationOffset = 2.3m,
                WaterLevelCalculationDensity = 1.05m,
                WaterLevelCalculationWidth = 5.1m,
                WaterLevelCalculationAngle = 45.2m,
                WaterLevelCalculationFormFactor = 6.51m,
                WaterLevelCalculationMinCalcHeight = 78.4m,
                WaterLevelCalculationFormType = 7,
                WaterLevelCalculationIsAbsoluteSensor = false,
                WaterLevelCalculationCalculateFrom = 0,
                WaterLevelCalculationConversionTo = 3
            };

            var serialized = JsonConvert.SerializeObject(config);

            string convertedObject = testee.Convert(config);
            string expected = "#a/a=New APN/b=546813/c=Password/d=dns.swisscom.ch/e=SMTP Name/f=POP Username/g=POP Password" +
                    "/h=SMTP Username/i=SMTP Password/j=pop.keller-druck.ch/k=56/l=smtp.keller-druck.ch/m=55/n=beni.ricchiuto@gmail.com" +
                "#b/a=measuerement@keller-druck.com/b=alarm@keller-druck.com/c=info@keller-druck.com/g=QueryPW/j=6526" +
                    "/k=+5821347865/m=+41793854745/n=+652423842347/o=+49582164834/q=+41783456124/r=Test Network Name/s=+41793893499" +
                    "/t=Hinter dem Haus/u=MEASURE/v=ALARM/w=ANSWER/0=7.58831/1=5.38752/2=500" +
                "#c/g=68501/h=68511/i=68503/j=68506/k=68531" +
                    "/m=103/o=21/p=85/q=20/r=8/s=2/t=50/v=6/w=4/x=21/y=21/z=105/0=1/1=1/2=1/3=7/4=4/5=5/6=2/7=0/8=0/9=3/A=0/B=1" +
                "#d/a=+5.6850000/b=+7.3540000/c=+2.4710000/f=+7.1400000/g=+5.2100000/i=+1.2500000/j=+94.210000" +
                    "/k=+122.00000/m=+1.0000000/n=-50.001000/o=+10.470000/p=+2.3000000/q=+1.0500000/r=+5.1000000" +
                        "/s=+45.200000/t=+6.5100000/u=+78.400000/0=+7.5883100/1=+5.3875200/2=+500.00000" +
                "#f/g=68521/h=68522/m=11/n=3/o=15/q=22/z=9/3=250" +
                "#k/a=ftp.keller-druck.com/b=Username/c=PasswordForFTP/d=AccountName/e=65/f=21/g=66/h=folder";

            string newExpected =
                "#a/e=SMTP Name" +
                "#b/a=measuerement@keller-druck.com/b=alarm@keller-druck.com/c=info@keller-druck.com/k=+5821347865/m=+41793854745/n=+652423842347/o=+49582164834/r=Test Network Name/s=+41793893499/t=Hinter dem Haus/u=MEASURE/v=ALARM/w=ANSWER/0=7.58831/1=5.38752/2=500" +
                "#c/g=68501/h=68511/i=68503/j=68506/k=68531/m=103/o=21/p=85/q=20/r=8/s=2/t=50/v=6/w=4/x=21/y=21/z=105/0=1/2=1/3=7/4=4/5=5/6=2/7=0/8=0/9=3" +
                "#d/a=+5.6850000/b=+7.3540000/c=+2.4710000/f=+7.1400000/g=+5.2100000/i=+1.2500000/j=+94.210000/k=+122.00000/m=+1.0000000/n=-50.001000/o=+10.470000/p=+2.3000000/q=+1.0500000/r=+5.1000000/s=+45.200000/t=+6.5100000/u=+78.400000/0=+7.5883100/1=+5.3875200/2=+500.00000" +
                "#f/g=68521/h=68522/m=11/n=3/o=15/q=22/z=9/3=250";
            convertedObject.Should().StartWith(newExpected + "#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact(Skip = "FTP changes will be ignored")]
        public void WhenTryingToConvertWithASimpleDeviceConfig_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                FtpUsername = "datamanager_666@gsmdata.ch",
                FtpSourceControlPort = 21
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#k/b=datamanager_666@gsmdata.ch/e=21#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertByteField_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                MeasurementSendSms = true,
                AlarmSendSms = false,
                InfoSendSms = true,
                CheckSendSms = true,
                MeasurementSendMail = false,
                AlarmSendMail = false,
                InfoSendMail = false,
                CheckSendMail = true
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#c/z=141#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertPositiveDecimalFields_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = 1,
                AlarmOffThreshold = 1,
                AlarmDeltaThreshold = 1,
                HardwareMultiplierTemperatureChannels = 1,
                HardwareMultiplierPressureChannels = 1,
                EventOnValueThreshold = 1,
                EventOffValueThreshold = 1,
                EventDeltaValueThreshold = 1,
                WaterLevelCalculationEnable = 1,
                WaterLevelCalculationLength = 1,
                WaterLevelCalculationHeight = 1,
                WaterLevelCalculationOffset = 1,
                WaterLevelCalculationDensity = 1,
                WaterLevelCalculationWidth = 1,
                WaterLevelCalculationAngle = 1,
                WaterLevelCalculationFormFactor = 1,
                WaterLevelCalculationMinCalcHeight = 1
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#d/a=+1.0000000/b=+1.0000000/c=+1.0000000/f=+1.0000000/g=+1.0000000" +
                "/i=+1.0000000/j=+1.0000000/k=+1.0000000/m=+1.0000000/n=+1.0000000/o=+1.0000000/p=+1.0000000/q=+1.0000000" +
                "/r=+1.0000000/s=+1.0000000/t=+1.0000000/u=+1.0000000#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertNegativeDecimalFields_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = -1,
                AlarmOffThreshold = -1,
                AlarmDeltaThreshold = -1,
                HardwareMultiplierTemperatureChannels = -1,
                HardwareMultiplierPressureChannels = -1,
                EventOnValueThreshold = -1,
                EventOffValueThreshold = -1,
                EventDeltaValueThreshold = -1,
                WaterLevelCalculationEnable = -1,
                WaterLevelCalculationLength = -1,
                WaterLevelCalculationHeight = -1,
                WaterLevelCalculationOffset = -1,
                WaterLevelCalculationDensity = -1,
                WaterLevelCalculationWidth = -1,
                WaterLevelCalculationAngle = -1,
                WaterLevelCalculationFormFactor = -1,
                WaterLevelCalculationMinCalcHeight = -1
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#d/a=-1.0000000/b=-1.0000000/c=-1.0000000/f=-1.0000000/g=-1.0000000" +
                "/i=-1.0000000/j=-1.0000000/k=-1.0000000/m=-1.0000000/n=-1.0000000/o=-1.0000000/p=-1.0000000/q=-1.0000000" +
                "/r=-1.0000000/s=-1.0000000/t=-1.0000000/u=-1.0000000#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertDecimalFieldWithNineDigits_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = 999999999.4m
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#d/a=+999999999#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertDecimalFieldWithMoreThanNineDigits_ThenExceptionIsThrown()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = 999999999.5m
            };

            testee.Invoking(_ => _.Convert(config)).Should().Throw<DecimalTooManyDigitsException>();
        }

        [Fact]
        public void WhenTryingToConvertDecimalFieldWithMoreThanNineDecimals_ThenRoundedDownDecimalIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = 0.00000001m
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#d/a=+0.0000000#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertDecimalFieldWithMoreThanNineDecimals_ThenRoundedUpDecimalIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = 0.00000005m
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#d/a=+0.0000001#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertNegativeDecimalFieldWithNineDigits_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = -999999999.4m
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#d/a=-999999999#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertNegativeDecimalFieldWithMoreThanNineDigits_ThenExceptionIsThrown()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = -999999999.5m
            };

            testee.Invoking(_ => _.Convert(config)).Should().Throw<DecimalTooManyDigitsException>();
        }

        [Fact]
        public void WhenTryingToConvertNegativeDecimalFieldWithMoreThanNineDecimals_ThenRoundedDownDecimalIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = -0.00000011m
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#d/a=-0.0000001#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertNegativeDecimalFieldRoundedToZero_ThenPositiveRoundedDecimalIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = -0.00000001m
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#d/a=+0.0000000#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertNegativeDecimalFieldWithMoreThanNineDecimals_ThenRoundedUpDecimalIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmOnThreshold = -0.00000005m
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#d/a=-0.0000001#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertTrueBooleanValue_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
             //   FtpUseTLS = true,
             //   FtpUseActiveMode = true,
                MailUseAlternativeSMTPLogin = true,
                WaterLevelCalculationIsAbsoluteSensor = true
            };

            string convertedObject = testee.Convert(config);
            //convertedObject.Should().StartWith("#c/2=1/7=1/A=1/B=1#O/f=");
            convertedObject.Should().StartWith("#c/2=1/7=1#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertFalseBooleanValue_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
           //     FtpUseTLS = false,
            //    FtpUseActiveMode = false,
                MailUseAlternativeSMTPLogin = false,
                WaterLevelCalculationIsAbsoluteSensor = false
            };

            string convertedObject = testee.Convert(config);
            //convertedObject.Should().StartWith("#c/2=0/7=0/A=0/B=0#O/f=");
            convertedObject.Should().StartWith("#c/2=0/7=0#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertByteValue_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                GprsModemProtocol = 5,
                MeasurementSendMailAfterX = 5,
                MeasurementSendSmsAfterX = 5,
                MeasurementSendFtpAfterX = 5,
                AlarmSendXTimes = 5,
                AlarmType = 5,
                AlarmChannelNumber = 5,
                HardwareConnectionType = 5,
                HardwarePowerExternalDevice = 5,
                HardwareSupportedSensorTypes = 5,
                WaterLevelCalculationFormType = 5,
                WaterLevelCalculationCalculateFrom = 5,
                WaterLevelCalculationConversionTo = 5,
                EventChannel = 5,
                EventType = 5
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#c/o=5/q=5/r=5/s=5/t=5/0=5/3=5/4=5/5=5/6=5/8=5/9=5#f/m=5/n=5/q=5#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertZeroByteValue_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                GprsModemProtocol = 0,
                MeasurementSendMailAfterX = 0,
                MeasurementSendSmsAfterX = 0,
                MeasurementSendFtpAfterX = 0,
                AlarmSendXTimes = 0,
                AlarmType = 0,
                AlarmChannelNumber = 0,
                HardwareConnectionType = 0,
                HardwarePowerExternalDevice = 0,
                HardwareSupportedSensorTypes = 0,
                WaterLevelCalculationFormType = 0,
                WaterLevelCalculationCalculateFrom = 0,
                WaterLevelCalculationConversionTo = 0,
                EventChannel = 0,
                EventType = 0
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#c/o=0/q=0/r=0/s=0/t=0/0=0/3=0/4=0/5=0/6=0/8=0/9=0#f/m=0/n=0/q=0#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact]
        public void WhenTryingToConvertIntegerWithinMinAndMax_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
             //   FtpPort = 1024,
             //   FtpSourceControlPort = 1025,
             //   FtpSourceDataPort = 1026,
                MailSmtpPort = 1027,
                MailPop3Port = 1028,
                MeasurementInterval = 6,
                InfoInterval = 9,
                CheckInterval = 11,
                AlarmInterval = 13,
                EventCheckInterval = 16,
                EventMeasureInterval = 17,
                EventSendMailXTimes = 19,
                HardwareDataConnectionInterval = 21,
                HardwarePreOnTime = 22
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith(
            //    "#a/k=1028/m=1027" +
                "#c/g=6/h=13/i=9/j=11/k=21" +
                "#f/g=16/h=17/o=19/3=22" +
               // "#k/e=1025/f=1024/g=1026" +
                "#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact(Skip = "FTP changes will be ignored")]
        public void WhenTryingToConvertIntegerOutOfUpperBound_ThenExceptionIsThrown()
        {
            var config = new DatabaseDeviceSettings
            {
                FtpPort = 65536
            };

            testee.Invoking(_ => _.Convert(config)).Should().Throw<OverflowException>();
        }

        [Fact]
        public void WhenTryingToConvertIntegerOutOfLowerBound_ThenExceptionIsThrown()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmInterval = -1
            };

            testee.Invoking(_ => _.Convert(config)).Should().Throw<OverflowException>();
        }

        [Fact(Skip = "FTP changes will be ignored")]
        public void WhenTryingToConvertString_ThenSimpleTextIsProduced()
        {
            var config = new DatabaseDeviceSettings
            {
                GprsAPN = "Test APN"
            };

            string convertedObject = testee.Convert(config);
            convertedObject.Should().StartWith("#a/a=Test APN#O/f=");
            convertedObject.Should().EndWith("#E/e");
        }

        [Fact(Skip = "GprsAPN is not supported anymore")]
        public void WhenTryingToConvertTooLongString_ThenExceptionIsThrown()
        {
            var config = new DatabaseDeviceSettings
            {
                GprsAPN = "Test APN wich should be way too long for the converter to process it"
            };

            testee.Invoking(_ => _.Convert(config)).Should().Throw<CharValueTooLongException>();
        }

        [Fact(Skip = "GprsAPN is not supported anymore")]
        public void WhenTryingToConvertStringWithUnsupportedCharacters_ThenExceptionIsThrown()
        {
            int unsupportedStartCharacter = 0;
            int unsupportedEndCharacter = 31;

            for (int i = unsupportedStartCharacter; i <= unsupportedEndCharacter; i++)
            {
                var config = new DatabaseDeviceSettings
                {
                    GprsAPN = char.ConvertFromUtf32(i)
                };

                testee.Invoking(_ => _.Convert(config)).Should().Throw<UnsupportedCharException>();
            }

            unsupportedStartCharacter = 123;
            unsupportedEndCharacter = 127;

            for (int i = unsupportedStartCharacter; i <= unsupportedEndCharacter; i++)
            {
                var config = new DatabaseDeviceSettings
                {
                    GprsAPN = char.ConvertFromUtf32(i)
                };

                testee.Invoking(_ => _.Convert(config)).Should().Throw<UnsupportedCharException>();
            }
        }

        [Fact]
        public void WhenTryingToConvertTimerFields_ThenEmptyConfigurationIsGenerated()
        {
            var config = new DatabaseDeviceSettings
            {
                AlarmTimer = 1,
                CheckTimer = 2,
                EventMeasureTimer = 3,
                HardwareDataConnectionTimer = 4,
                InfoTimer = 5,
                //MeasurementTimer = 6
            };

            testee.Invoking(_ => _.Convert(config)).Should().Throw<EmptyGsmMessageException>();
        }
    }
}
