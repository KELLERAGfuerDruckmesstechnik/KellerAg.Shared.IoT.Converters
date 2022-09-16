using KellerAg.Shared.Entities.Database;

namespace KellerAg.Shared.IoT.Converters.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using FluentAssertions;
    using Newtonsoft.Json;
    using JsonToBusinessObjects.DataContainers;
    using JsonToBusinessObjects.DataContainers.Configuration;

    [TestClass]
    public class ConvertTests
    {
        private const string ExampleGsmCommunicationWithConfigurationText = "#F/d=0#T/s=443025550/p=14.01.14,15:38:49+04#M/a=+0.0004080+0.9559831+0.0000000+24.482742+0.9555750+25.100002/c=+1+1/d=+0.0004080+0.9559831+0.0000000+0.0000000+24.482742+0.0000000+0.9555750+25.100002#I/n=1416/s=17/b=99/f=12.40/v=+3.823#a/a=gprs.swisscom.ch/b=gprrs/c=gprs/d=000.000.000.000/e=Gdemo/f=gsm_xx@gsmdata.ch/g=yourpassword/h=gsm_xx@gsmdata.ch/i= yourpassword/j=pop.gsmdata.ch/k=110/l=smtp.gsmdata.ch/m=587/n=gsm_xx@gsmdata.ch#b/a=datamanager_xx@gsmdata.ch/b=demo@keller-druck.ch /c=demo@keller-druck.ch/g=/j=/k=/m=+41781234567/n=+41781234567/o=/q=+41794999000/r=KELLERDEMO/s=+41798101088/t=DemoStation/u=station keller/v=alarm message body/w=/0=8.747837/1=47.497869/2=450.000#c/a=443027701/b=443025541/c=631144800/d=442926000/e=631144800/g=3000/h=60/i=86400/j=86400/k=86400/m=215/n=0/o=3/p=0/q=1/r=4/s=1/t=1/v=4/w=4/x=0/y=3/z=0/0=0/1=0/2=0/3=0/4=4/5=7/6=3/7=0/8=1/9=0#f/a=431005483/g=3600/h=3600/m=1/n=1/o=1/q=1/z=15#d/a=+10.000000/b=+9.0000000/c=+0.0010000/f=+1.0000000/g=+1.0000000/i=+0.9000000/j=+1.2000000/k=+5.0000000/m=+1.0000000/n=+9.1180000/o=+400.00000/p=+0.0000000/q=+1000.0000/r=+1.0000000/s=+0.0000000/t=+1.0000000/u=+0.0000000/v=+10.000000/w=+11.000000/0=+8.7478370/1=+47.497869/2=+450.00000#k/a=ftp.gsmdata.ch/b=datamanager_xx@gsmdata.ch/c=yourpassword/d=datamanager_xx/e=21/f=21/g=2000/h=#O/g=43607#E/e";
        private const string ExampleGsmCommunicationWithMeasurementsText = "#F/e=0#C/a=427/b=30#T/s=650410250/p=20.08.11,01:31:01+08#M/a=-0.0006130+0.9656312+26.115936+26.057617+0.9662400+25.559999+0.0000000+0.0000000/c=+1+1#I/n=926/s=15/b=99/e=9.20/f=19.50/h=31/v=+3.890#B/a=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ALoc7BA/dzowQdFDQEHQv2A/d2JwQczMoAAAALAAAADwAAOEALogtBA/dzMwQdDtQEHQdmA/d1sBqybEeOsAAHBBzHqgAAAAsAAAAP//////////////////////////////////////////////////////////#E/e#X/a=40205";

        private const string ExampleGsmCommunicationWithNewConfigurationForDeviceText = "#a/e=ARC-1#b/r=Sales Demo/s=+901405100488249/t=ARC-1  SN 5 - Demo/u=MEASURE message/v=ALARM message/w=CHECK message/0=8.7397621/1=47.4997252/2=455.2#c/g=1800/h=3600/i=86400/j=43200/k=86400/m=211/o=1/p=1/q=10/r=1/s=1/t=1/v=6/w=4/x=9/y=9/z=34/0=0/2=0/3=0/4=3/5=13/6=5/7=0/8=0/9=0#d/a=+2.0000000/b=+1.5000000/c=+0.5000000/f=+1.0000000/g=+1.0000000/i=+0.0000000/j=+0.0000000/k=+0.1000000/m=+0.0000000/n=+50.000000/o=+400.00000/p=+0.0000000/q=+998.20000/r=+1.0000000/s=+0.0000000/t=+0.0000000/u=+0.0000000/0=+8.7500000/1=+47.500000/2=+450.00000#f/g=86400/h=86400/m=1/n=1/o=1/q=10/z=13/3=0#O/f=15671#E/e";

        [TestMethod]
        public void GsmCommunicationToJson_GivenAValidFtpText_TheContentCanBeConvertedToAProperJsonText()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string gsmCommunicationText = ExampleGsmCommunicationWithConfigurationText;
            var result = "";
            // Act
            try
            {
                result = convert.GsmCommunicationToJson(gsmCommunicationText);
            }
            catch
            {
                Assert.Fail("Can not convert to Json string");
            }
            
            // Assert
            Assert.IsTrue(result.Length>50);
        }

        [TestMethod]
        public void GsmCommunicationJsonToBusinessObject_GivenAValidCommunicationTextWithConfiguration_TheConvertedBusinessObjectContainsConfiguration()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string gsmCommunicationJson = convert.GsmCommunicationToJson(ExampleGsmCommunicationWithConfigurationText);
            
            // Act
            var result = convert.GsmCommunicationJsonToBusinessObject(gsmCommunicationJson);

            // Assert
            Assert.IsNotNull(result.BusinessObjectRoot.Configuration);
        }

        [TestMethod]
        public void GsmCommunicationJsonToBusinessObject_GivenAValidCommunicationTextWithMeasurements_TheConvertedBusinessObjectContainsMeasurements()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string gsmCommunicationJson = convert.GsmCommunicationToJson(ExampleGsmCommunicationWithMeasurementsText);

            // Act
            var result = convert.GsmCommunicationJsonToBusinessObject(gsmCommunicationJson);

            // Assert
            Assert.IsNotNull(result.BusinessObjectRoot.Measurements);
        }


        [TestMethod]
        public void BusinessObjectToDeviceConfiguration_GivenABusinessObjectWithSomeNewSettings_TheConversionToDeviceConfigurationShouldBeValid()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();

            BusinessObjectRoot businessObject = new BusinessObjectRoot
            {
                Configuration = new ConfigurationContainer
                {
                    GprsSettings = null,
                    TextEmailSmsLocSettings = new TextEmailSmsLocSettings
                    {
                        NetworkName = "Sales Demo",         // New Network Name
                        LocationName = "ARC-1 SN 5 - Demo", // New Device Name
                        LongitudeText = "8.7397621",        // New Coordinates
                        LatitudeText = "47.4997252",        // ..
                        AltitudeText = "455.2"              // ..
                    },
                    MeasurementSettings = new MeasurementSettings
                    {
                        MeasureAndSaveCH0_7 = 211, //changed which channels are being measured
                        MeasureAndSaveCH8_15 = 1,  // ..
                        Interval0Measure = 1800,   // Measurement every 30min (30x60s)
                        Interval3Check = 43200     // The device shall check the FTP for new configuration every 12h (12hx60x60)
                    },
                    FloatingPointMeasurementSettings = null,
                    FtpSettings = null,
                    MeasurementSettings2 = new MeasurementSettings2
                    {
                        SendToFtpAfterXCollectedMeasurements = 10, //Send after 10 measurements
                    }
                }
            };

            var s = Newtonsoft.Json.JsonConvert.SerializeObject(businessObject);

            // Act
            DeviceSettings result = convert.BusinessObjectToDeviceConfiguration(businessObject);

            // Assert

            Assert.AreEqual("Sales Demo", result.GeneralNetworkName);
            Assert.AreEqual("ARC-1 SN 5 - Demo", result.GeneralLocationName);
            Assert.AreEqual("8.7397621", result.GeneralLongitudeText); 
            Assert.AreEqual("47.4997252", result.GeneralLatitudeText);
            Assert.AreEqual("455.2", result.GeneralAltitudeText);

            Assert.IsTrue((bool)result.HardwareMeasureSaveChannel0);  //211 d = 1101'0011 b
            Assert.IsTrue((bool)result.HardwareMeasureSaveChannel1);
            Assert.IsTrue((bool)!result.HardwareMeasureSaveChannel2);
            Assert.IsTrue((bool)!result.HardwareMeasureSaveChannel3);
            Assert.IsTrue((bool)result.HardwareMeasureSaveChannel4);
            Assert.IsTrue((bool)!result.HardwareMeasureSaveChannel5);
            Assert.IsTrue((bool)result.HardwareMeasureSaveChannel6);
            Assert.IsTrue((bool)result.HardwareMeasureSaveChannel7);
                         
            Assert.IsTrue((bool)result.HardwareMeasureSaveChannel8);  //01 d = 0000'0001 b
            Assert.IsTrue((bool)!result.HardwareMeasureSaveChannel9);
            Assert.IsTrue((bool)!result.HardwareMeasureSaveChannel10);
            Assert.IsTrue((bool)!result.HardwareMeasureSaveChannel11);
            Assert.IsTrue((bool)!result.HardwareMeasureSaveChannel12);
            Assert.IsTrue((bool)!result.HardwareMeasureSaveChannel13);
            Assert.IsTrue((bool)!result.HardwareMeasureSaveChannel14);
            Assert.IsTrue((bool)!result.HardwareMeasureSaveChannel15);

            Assert.AreEqual(1800, result.MeasurementInterval);
            Assert.AreEqual(43200, result.CheckInterval);

            Assert.AreEqual((byte)10, result.MeasurementSendFtpAfterX);
        }


        [TestMethod]
        public void DeviceConfigurationToGsmCommunication_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string deviceConfigurationDifferenceJson = "{\"generalLongitudeText\":\"lond1234\",\"hardwareConnectionType\":3,\"hardwarePowerExternalDevice\":12,\"hardwareMeasureSaveChannel0\":false,\"hardwareMeasureSaveChannel1\":true,\"hardwareDataConnectionCallNumber\":\"string\",\"measurementTimer\":12345,\"measurementInterval\":987,\"waterLevelCalculationLength\":42}";

            // Act
            var result = convert.DeviceConfigurationToGsmCommunication(deviceConfigurationDifferenceJson);

            // Assert
            result.Should().Contain("#b/k=string/0=lond1234#c/a=12345/g=987/m=2/4=12/6=3#d/n=+42");
        }

        [TestMethod]
        public void DeviceConfigurationToGsmCommunication_StateUnderTest_ExpectedBehavior2()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string deviceConfigurationDifferenceJson = "{\"generalLongitudeText\":\"lond1234\",\"hardwareConnectionType\":3,\"hardwarePowerExternalDevice\":12,\"hardwareMeasureSaveChannel0\":false,\"hardwareMeasureSaveChannel1\":true,\"hardwareDataConnectionCallNumber\":\"string\",\"measurementTimer\":12345,\"measurementInterval\":987,\"waterLevelCalculationLength\":42}";
            DeviceSettings deviceSettings = JsonConvert.DeserializeObject<DeviceSettings>(deviceConfigurationDifferenceJson);

            // Act
            var result = convert.DeviceConfigurationToGsmCommunication(deviceSettings);

            // Assert
            result.Should().Contain("#b/k=string/0=lond1234#c/a=12345/g=987/m=2/4=12/6=3#d/n=+42");
        }

        [TestMethod]
        public void DeviceConfigurationToGsmCommunication_GivenSomeRandomDeviceConfiguration_TheConfigurationHasToBeConvertedToFtpText()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            DeviceSettings deviceConfigurations = new DeviceSettings
            {
                AlarmChannelNumber = 55,
                CheckTimer = 12345,
                FtpAccount = "ftp@gsmdata.ch",
                GeneralLocationName = "MY NAME",
                GeneralNetworkName = "MyNETWORK",
                HardwareMultiplierPressureChannels = 44,
                HardwarePowerExternalDevice = 111,
                HardwarePreOnTime = 150,
                InfoSendMail = true,
                InfoTimer = 23,
                IsSentFromAPI = true,
                MeasurementInterval = 12,
                MeasurementMailAddress = "mail4measuremnts@customer.com",
                State = SendState.PendingForProgramming,
                WaterLevelCalculationAngle = (decimal)1.2345,
                WaterLevelCalculationCalculateFrom = 123,
            };

            // Act
            string result = convert.DeviceConfigurationToGsmCommunication(deviceConfigurations);

            // Assert
            Assert.AreEqual("#b/a=mail4measuremnts@customer.com/r=MyNETWORK/t=MY NAME#c/g=12/r=55/z=64/4=111/8=123#d/g=+44.000000/s=+1.2345000#f/3=150#O/f=0#E/e", result);
        }



        private const string ExampleLoRaPayloadMeasurementText = "010501d37fc000007fc000007fc000003f76c8b441bb33333920ac08";  //Actility Port 1
        private const string ExampleLoRaPayloadInformationText = "DAETABMtAAAAHCVPHbBAnfMsYyg=";  //TTN Port 4
        private const string ExampleLoRaPayloadConfigurationText = "20011300132F0D010301000500D3FF000001550005010503"; //Actility Port 5

        [TestMethod]
        public void LoRaPayloadToLoRaMessage_WhenPayloadFromActilityAndPort1_ShouldBeSupported()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string loRaPayload = ExampleLoRaPayloadMeasurementText;
            int port = 1;

            // Act
            var result = convert.LoRaPayloadToLoRaMessage(loRaPayload, port);

            // Assert
            Assert.IsTrue(result.IsSupportedDevice);
        }

        [TestMethod]
        public void LoRaPayloadToLoRaMessage_WhenPayloadFromTTNAndPort4_ShouldBeSupported()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string loRaPayload = ExampleLoRaPayloadInformationText;
            int port = 4;

            // Act
            var result = convert.LoRaPayloadToLoRaMessage(loRaPayload, port);

            // Assert
            Assert.IsTrue(result.IsSupportedDevice);
        }


        [TestMethod]
        public void LoRaPayloadToLoRaMessage_WhenPayloadFromActilityAndPort5_ShouldBeSupported()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string loRaPayload = ExampleLoRaPayloadConfigurationText;
            int port = 5;

            // Act
            var result = convert.LoRaPayloadToLoRaMessage(loRaPayload, port);

            // Assert
            Assert.IsTrue(result.IsSupportedDevice);
        }


        private const string ExampleLoRaTransmissionTTNText = "{\"app_id\":\"adt\",\"dev_id\":\"adt7\",\"hardware_serial\":\"009D6B0000C5D3AD\",\"port\":1,\"counter\":44398,\"payload_raw\":\"AQMA0791hU642eAAQanoAD91fmdBq8KP\",\"payload_fields\":{\"P1\":-0.00010389089584350586,\"PBaro\":0.9589599967002869,\"Pd (P1-PBaro)\":-0.959065318107605,\"TBaro\":21.469999313354492,\"TOB1\":21.23828125,\"channel\":\"0000000011010011\",\"channelCount\":5,\"ct\":3,\"func\":1,\"payload\":\"010300D3BF75854EB8D9E00041A9E8003F757E6741ABC28F\",\"port\":1},\"metadata\":{\"time\":\"2020-09-23T12:53:10.495592788Z\",\"frequency\":867.7,\"modulation\":\"LORA\",\"data_rate\":\"SF7BW125\",\"coding_rate\":\"4/5\",\"gateways\":[{\"gtw_id\":\"eui-0005fcc23d116aa3\",\"timestamp\":983876404,\"time\":\"2020-09-23T12:53:10.471594Z\",\"channel\":6,\"rssi\":-113,\"snr\":-1,\"rf_chain\":0},{\"gtw_id\":\"ttn_gtw_ent_keller\",\"timestamp\":3908144420,\"time\":\"2020-09-23T12:53:10Z\",\"channel\":0,\"rssi\":-34,\"snr\":9.75,\"rf_chain\":0}]},\"downlink_url\":\"https://integrations.thethingsnetwork.org/ttn-eu/api/v2/down/adt/adt1?key=ttn-account-v2.rwvb0pvXqv9Xs8fjhqAEEvCh8ztI5vZBNYVTyK1cWPQ\"}";
        private const string ExampleLoRaTransmissionActilityText = @"{""DevEUI_uplink"":{""Time"":""2017-11-07T09:51:01.107+01:00"",""DevEUI"":""0004A30B001E5CD8"",""FPort"":""1"",""FCntUp"":""7"",""ADRbit"":""1"",""MType"":""2"",""FCntDn"":""1"",""payload_hex"":""010501d37fc000007fc000007fc000003f76c8b441bb33333920ac08"",""mic_hex"":""22cf05de"",""Lrcid"":""00000401"",""LrrRSSI"":""-65.000000"",""LrrSNR"":""9.500000"",""SpFact"":""7"",""SubBand"":""G1"",""Channel"":""LC1"",""DevLrrCnt"":""1"",""Lrrid"":""004A09F8"",""Late"":""0"",""Lrrs"":{""Lrr"":[{""Lrrid"":""004A09F8"",""Chain"":""0"",""LrrRSSI"":""-65.000000"",""LrrSNR"":""9.500000"",""LrrESP"":""-65.461838""}]},""CustomerID"":""100000526"",""CustomerData"":{""loc"":{""lat"":""47.49825679999999"",""lon"":""8.747362100000032""},""alr"":{""pro"":""LORA/Generic"",""ver"":""1""}},""ModelCfg"":""1:New"",""AppSKey"":""d5bbf0b5f659f7d72eef31182b1788cc"",""InstantPER"":""0.000000"",""MeanPER"":""0.000000"",""DevAddr"":""094B5DAE""}}";
        private const string ExampleLoRaTransmissionLoriotText = @"{""cmd"":""rx"",""seqno"":1131,""EUI"":""0004A30B001FFA11"",""ts"":1522938365141,""fcnt"":4,""port"":1,""freq"":868300000,""rssi"":-101,""snr"":6.2,""toa"":1974,""dr"":""SF12 BW125 4/5"",""ack"":false,""bat"":251,""data"":""010500d37fc000007fc000007fc000003f77006941cacccd""}";

        [TestMethod]
        public void LoRaJsonMessageToBusinessObject_WithAMessageFromTTN_ShouldConvertToReadableMeasurements()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string jsonMessage = ExampleLoRaTransmissionTTNText;

            // Act
            var result = convert.LoRaJsonMessageToBusinessObject(jsonMessage);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoRaJsonMessageToBusinessObject_WithAMessageWithDeviceInformation_ShouldConvertToReadableDeviceInformation()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string jsonMessage = ExampleLoRaTransmissionActilityText;

            // Act
            var result = convert.LoRaJsonMessageToBusinessObject(jsonMessage);

            // Assert
            Assert.IsNotNull(result.DeviceInformation);
        }

        [TestMethod]
        public void LoRaJsonMessageToBusinessObject_WithAMessageWithConfigurations_ShouldConvertToReadableConfiguration()
        {
            // Arrange
            var convert = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string jsonMessage = ExampleLoRaTransmissionLoriotText;

            // Act
            var result = convert.LoRaJsonMessageToBusinessObject(jsonMessage);

            // Assert
            Assert.IsNotNull(result.Measurements);
        }
    }
}
