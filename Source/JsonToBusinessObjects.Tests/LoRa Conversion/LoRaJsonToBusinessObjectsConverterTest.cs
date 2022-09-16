using System;
using System.Linq;
using FluentAssertions;
using JsonToBusinessObjects.Conversion;
using JsonToBusinessObjects.Conversion.LoRa;
using JsonToBusinessObjects.DataContainers;
using Newtonsoft.Json;
using Xunit;

namespace JsonToBusinessObjects.Tests.LoRa_Conversion
{
    public class LoRaJsonToBusinessObjectsConverterTest
    {
        public LoRaJsonToBusinessObjectsConverterTest()
        {
            _testee = new JsonToBusinessObjectsConverterForLoRa();
        }
        private readonly JsonToBusinessObjectsConverterForLoRa _testee;


        [Fact]
        public void WhenGivenAValidActilityJson_ThenNoErrorsAreReported()
        {
            ConversionResult result = _testee.Convert(TestMeasurementMessageActility);
            result.BusinessObjectRoot.LoRaData.DeviceConnectionType.Should().Be(5);
            result.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void WhenGivenAValidTTNJson_ThenNoErrorsAreReported()
        {
            ConversionResult result = _testee.Convert(TestMeasurementMessageTTN_New);
            result.BusinessObjectRoot.LoRaData.DeviceConnectionType.Should().Be(5);
            result.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void WhenGivenAnInValidTTNJson_ThenErrorsAreReported()
        {
            ConversionResult result = _testee.Convert(TestMeasurementMessageTTN_Invalid);
            result.HasErrors.Should().BeTrue();
        }

        [Fact]
        public void WhenGivenAValidTTNJson_ThenConvertedValuesMustBeCorrect()
        {
            ConversionResult result = _testee.Convert(TestMeasurementMessageTTN);
            result.HasErrors.Should().BeFalse();
            result.BusinessObjectRoot.DeviceInformation.ProductLine = ProductLineName.ARC1_LoRa;
            result.BusinessObjectRoot.LoRaData.RSSI = -42; //not the weaker -61 
            result.BusinessObjectRoot.LoRaData.Time.Should().BeCloseTo((new DateTime(2017, 10, 30, 11, 18, 25, DateTimeKind.Utc)).AddMilliseconds(511.3805));
            result.BusinessObjectRoot.LoRaData.Measurements[3] = 0.9713000059127808f;
            result.BusinessObjectRoot.LoRaData.DeviceConnectionType.Should().Be(5);
        }

        [Fact]
        public void WhenGivenAValidTTNJson_ThenConvertedValuesMustBeCorrect2()
        {
            ConversionResult result = _testee.Convert(TestMeasurementMessageTTNX);
            result.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void WhenGivenAValidTTN_V3_Json_TheJsonCanBeDeserialized()
        {
            LoraMessageTheThingsNetworkV3 deserializedObject = JsonConvert.DeserializeObject<LoraMessageTheThingsNetworkV3>(TTN_V3_Message_01);
            string decodedPayload = "{\"P1\":0,\"PBaro\":0.9776700139045715,\"T\":5.104235503814077E+38,\"TBaro\":22.56999969482422,\"TOB1\":20.450000762939453,\"channel\":\"0000000011011010\",\"channelCount\":5,\"ct\":3,\"func\":1,\"payload\":\"010300DA000000007FC0000041A3999A3F7A489541B48F5C\",\"port\":1}";
            deserializedObject.UplinkMessage.DecodedPayload.Root.ToString(Formatting.None).Should()
                .BeEquivalentTo(decodedPayload);
            deserializedObject.UplinkMessage.RxMetadata.First().Location.Altitude.Should().BePositive();
        }

        [Fact]
        public void WhenGivenAValidTTN_V3_Json_ThenConvertedValuesMustBeCorrect()
        {
            ConversionResult result = _testee.Convert(TTN_V3_Message_01);
            result.BusinessObjectRoot.Measurements.DataPointsByChannel.Count.Should().BeGreaterThan(0);
            result.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void WhenGivenAValidTTN_V3_JsonWithStrongGatewayWithoutLocation_ThenConvertedValuesMustBeCorrect2()
        {
            ConversionResult result = _testee.Convert(TTN_V3_Message_02);
            result.BusinessObjectRoot.Measurements.DataPointsByChannel.Count.Should().BeGreaterThan(0);
            result.HasErrors.Should().BeFalse();
        }


        [Fact]
        public void WhenGivenAValidActilityJson_ThenValuesAreConvertedCorrectly()
        {
            ConversionResult result = _testee.Convert(TestMeasurementMessageActility);
            result.BusinessObjectRoot.Measurements.DataPointsByChannel.Count.Should().Be(6);
            result.BusinessObjectRoot.LoRaData.Time.Should().Be((new DateTime(2017, 11, 07, 9 - 1, 51, 0, DateTimeKind.Utc)).AddMilliseconds(1107));
            result.BusinessObjectRoot.LoRaData.DeviceConnectionType.Should().Be(5);

            var timeInDataPoint = result.BusinessObjectRoot.Measurements.DataPointsByChannel.Values.ToArray()[0][0].Time;
            timeInDataPoint.Should().Be((new DateTime(2017, 11, 07, 9 - 1, 51, 0, DateTimeKind.Utc)).AddMilliseconds(1107));
        }

        [Fact]
        public void WhenGivenAValidActilityWithDeviceInformationJson_ThenDeviceInformationAreConvertedCorrectly()
        {
            ConversionResult result = _testee.Convert(TestMeasurementMessageActilityPort4);
            result.BusinessObjectRoot.LoRaData.Should().NotBeNull();
            result.BusinessObjectRoot.Measurements.DataPointsByChannel.Count.Should().Be(0);

            result.BusinessObjectRoot.LoRaData.Port.Should().Be(4);

            result.BusinessObjectRoot.DeviceInformation.BatteryCapacity.Should().Be(99);
            result.BusinessObjectRoot.DeviceInformation.MeasuredBatteryVoltage.Should().BeInRange(4.93593f, 4.93594f);
            result.BusinessObjectRoot.DeviceInformation.DeviceIdAndClass.Should().Be("19.00");
            result.BusinessObjectRoot.DeviceInformation.DeviceLocalDateTime.Should().BeCloseTo(new DateTime(2019, 11, 01, 16, 45, 36));
            result.BusinessObjectRoot.DeviceInformation.MeasuredHumidity.Should().Be(40);
            result.BusinessObjectRoot.DeviceInformation.DeviceSerialNumber.Should().Be(28);
            result.BusinessObjectRoot.DeviceInformation.GsmModuleSoftwareVersion.Should().Be("19.45");
        }

        [Fact]
        public void WhenGivenAPocActilityJson_ThenPayloadIsNotSupported()
        {
            ConversionResult result = _testee.Convert(TestMeasurementMessagePocActility);
            result.BusinessObjectRoot.Measurements.Should().Be(null);
            result.HasErrors.Should().BeTrue();
        }

        [Fact]
        public void WhenGivenAnActilityJsonWithoutLocation_ThenZeroIsUsedAndNoFatalErrorHappened()
        {
            ConversionResult result = _testee.Convert(ActilityMessageWithoutLocation);
            result.BusinessObjectRoot.LoRaData.Longitude = 0.0f;
            result.BusinessObjectRoot.LoRaData.Latitude  = 0.0f;
            result.BusinessObjectRoot.LoRaData.DeviceConnectionType.Should().Be(5);
            result.HasErrors.Should().BeFalse();
        }

        [Fact]
        public void WhenGivenAnValidLoriotJson_ThenNoErrorsAreReported()
        {
            ConversionResult result = _testee.Convert(TestMeasurementMessageLoriot);
            result.HasErrors.Should().BeFalse();
            result.BusinessObjectRoot.LoRaData.RSSI.Should().Be(-101);
            result.BusinessObjectRoot.LoRaData.Time.Should().BeCloseTo(new DateTime(2018, 04, 05, 14, 26, 5, DateTimeKind.Utc).AddMilliseconds(141));
            result.BusinessObjectRoot.LoRaData.Measurements[7].Should().Be(0.96485f);
            result.BusinessObjectRoot.LoRaData.DeviceConnectionType.Should().Be(5);

            result.BusinessObjectRoot.DeviceInformation.BatteryCapacity.Should().Be(98);
        }

        [Fact]
        public void WhenGivenAnInValidLoriotJsonWithoutPayload_ThenWarningsAreReportedAndPayloadFieldIsNull()
        {
            ConversionResult result = _testee.Convert(InvalidMeasurmentMessageLoriot);
            //This message is not a message with measurements. We should ignore it but not treat it as error as errors generate warn mails
            result.HasWarnings.Should().BeTrue();
            result.HasErrors.Should().BeFalse();
            result.BusinessObjectRoot.LoRaData.Payload.Should().BeNullOrEmpty();
        }

        [Fact]
        public void WhenGivenAnGateWayLoriotJsonWithoutPayload_ThenNoWarningsAreReportedAndPayloadFieldIsNull()
        {
            ConversionResult result = _testee.Convert(GatewayInfoFromLoriot);
            //This message is not a message with measurements. We should ignore it but not treat it as error as errors generate warn mails
            result.HasWarnings.Should().BeFalse();
            result.HasErrors.Should().BeFalse();

            result.BusinessObjectRoot.LoRaData.Measurements.Count.Should().Be(0);
            result.BusinessObjectRoot.LoRaData.DeviceConnectionType.Should().Be(-1);
            result.BusinessObjectRoot.LoRaData.DeviceConnectionType.Should().Be(-1);
            result.BusinessObjectRoot.LoRaData.Payload.Should().BeNullOrEmpty();

            result.BusinessObjectRoot.DeviceInformation.BatteryCapacity.Should().Be(79);
            result.BusinessObjectRoot.DeviceInformation.ProductLine.Should().Be(ProductLineName.ARC1_LoRa);
        }


        private const string GatewayInfoFromLoriot =
            @"{""cmd"":""gw"",""EUI"":""0004A30B001F9803"",""ts"":1576767603456,""fcnt"":7,""port"":1,""freq"":867700000,""toa"":823,""dr"":""SF11 BW125 4/5"",""ack"":false,""gws"":[{""rssi"":-119,""snr"":-3,""ts"":1576767603699,""tmms"":399,""time"":""2019-12-19T15:00:03.399018798Z"",""gweui"":""024B08FFFF050211"",""ant"":0},{""rssi"":-120,""snr"":-5.5,""ts"":1576767603451,""time"":""2019-12-19T15:00:02.051687Z"",""gweui"":""024B0BFFFF030534"",""ant"":0},{""rssi"":-120,""snr"":-14.8,""ts"":1576767603456,""tmms"":399,""time"":""2019-12-19T15:00:03.399013798Z"",""gweui"":""024B0BFFFF03042E"",""ant"":0}],""bat"":203,""data"":""""}";

        private const string InvalidMeasurmentMessageLoriot    = @"{""cmd"":""txd"",""EUI"":""0004A30B001FAEC2"",""seqdn"":6,""seqq"":6,""ts"":1536161407899}";
        private const string TestMeasurementMessageTTN         = @"{""app_id"":""ldtapplication"",""dev_id"":""ldtdevice1"",""hardware_serial"":""0004A30B001EC250"",""port"":1,""counter"":0,""payload_raw"":""AQUB03/AAAB/wAAAf8AAAD94px5BxdcKAAAAAA=="",""payload_fields"":{""Channel_1"":5.104235503814077e+38,""Channel_2"":5.104235503814077e+38,""Channel_3"":5.104235503814077e+38,""Channel_4"":0.9713000059127808,""channel"":""0000000111010011"",""ct"":5,""func"":1,""payload"":""AQswAD93JxNBu1wp""},""metadata"":{""time"":""2017-10-30T11:18:25.511380476Z"",""frequency"":867.1,""modulation"":""LORA"",""data_rate"":""SF12BW125"",""coding_rate"":""4/5"",""gateways"":[{""gtw_id"":""eui-c0ee40ffff29356b"",""timestamp"":300640588,""time"":"""",""channel"":3,""rssi"":-42,""snr"":9,""latitude"":47.49873,""longitude"":8.746949},{""gtw_id"":""kellergw2"",""gtw_trusted"":true,""timestamp"":509643924,""time"":""2017-10-30T11:17:44Z"",""channel"":3,""rssi"":-61,""snr"":9.5,""latitude"":47.498688,""longitude"":8.747711}]},""downlink_url"":""https://integrations.thethingsnetwork.org/ttn-eu/api/v2/down/ldtapplication/httpldttest?key=ttn-account-v2.4vThJdZ2IszcdwppUzCaLWsBmF1_GszPXEOglkR3AfA""}";
        private const string TestMeasurementMessageTTNX      = "{\"app_id\":\"adt\",\"dev_id\":\"adt7\",\"hardware_serial\":\"009D6B0000C5D3AD\",\"port\":1,\"counter\":44398,\"payload_raw\":\"AQMA0791hU642eAAQanoAD91fmdBq8KP\",\"payload_fields\":{\"P1\":-0.00010389089584350586,\"PBaro\":0.9589599967002869,\"Pd (P1-PBaro)\":-0.959065318107605,\"TBaro\":21.469999313354492,\"TOB1\":21.23828125,\"channel\":\"0000000011010011\",\"channelCount\":5,\"ct\":3,\"func\":1,\"payload\":\"010300D3BF75854EB8D9E00041A9E8003F757E6741ABC28F\",\"port\":1},\"metadata\":{\"time\":\"2020-09-23T12:53:10.495592788Z\",\"frequency\":867.7,\"modulation\":\"LORA\",\"data_rate\":\"SF7BW125\",\"coding_rate\":\"4/5\",\"gateways\":[{\"gtw_id\":\"eui-0005fcc23d116aa3\",\"timestamp\":983876404,\"time\":\"2020-09-23T12:53:10.471594Z\",\"channel\":6,\"rssi\":-113,\"snr\":-1,\"rf_chain\":0},{\"gtw_id\":\"ttn_gtw_ent_keller\",\"timestamp\":3908144420,\"time\":\"2020-09-23T12:53:10Z\",\"channel\":0,\"rssi\":-34,\"snr\":9.75,\"rf_chain\":0}]},\"downlink_url\":\"https://integrations.thethingsnetwork.org/ttn-eu/api/v2/down/adt/adt1?key=ttn-account-v2.rwvb0pvXqv9Xs8fjhqAEEvCh8ztI5vZBNYVTyK1cWPQ\"}";
        private const string TestMeasurementMessageActility    = @"{""DevEUI_uplink"":{""Time"":""2017-11-07T09:51:01.107+01:00"",""DevEUI"":""0004A30B001E5CD8"",""FPort"":""1"",""FCntUp"":""7"",""ADRbit"":""1"",""MType"":""2"",""FCntDn"":""1"",""payload_hex"":""010501d37fc000007fc000007fc000003f76c8b441bb33333920ac08"",""mic_hex"":""22cf05de"",""Lrcid"":""00000401"",""LrrRSSI"":""-65.000000"",""LrrSNR"":""9.500000"",""SpFact"":""7"",""SubBand"":""G1"",""Channel"":""LC1"",""DevLrrCnt"":""1"",""Lrrid"":""004A09F8"",""Late"":""0"",""Lrrs"":{""Lrr"":[{""Lrrid"":""004A09F8"",""Chain"":""0"",""LrrRSSI"":""-65.000000"",""LrrSNR"":""9.500000"",""LrrESP"":""-65.461838""}]},""CustomerID"":""100000526"",""CustomerData"":{""loc"":{""lat"":""47.49825679999999"",""lon"":""8.747362100000032""},""alr"":{""pro"":""LORA/Generic"",""ver"":""1""}},""ModelCfg"":""1:New"",""AppSKey"":""d5bbf0b5f659f7d72eef31182a1788cc"",""InstantPER"":""0.000000"",""MeanPER"":""0.000000"",""DevAddr"":""094B5DAE""}}";

        private const string TestMeasurementMessageActilityPort4 = @"{""DevEUI_uplink"":{""Time"":""2017-11-07T09:51:01.107+01:00"",""DevEUI"":""0004A30B001E5CD8"",""FPort"":""4"",""FCntUp"":""7"",""ADRbit"":""1"",""MType"":""2"",""FCntDn"":""1"",""payload_hex"":""0C011300132D0000001C254F1DB0409DF32C6328"",""mic_hex"":""22cf05de"",""Lrcid"":""00000401"",""LrrRSSI"":""-65.000000"",""LrrSNR"":""9.500000"",""SpFact"":""7"",""SubBand"":""G1"",""Channel"":""LC1"",""DevLrrCnt"":""1"",""Lrrid"":""004A09F8"",""Late"":""0"",""Lrrs"":{""Lrr"":[{""Lrrid"":""004A09F8"",""Chain"":""0"",""LrrRSSI"":""-65.000000"",""LrrSNR"":""9.500000"",""LrrESP"":""-65.461838""}]},""CustomerID"":""100000526"",""CustomerData"":{""loc"":{""lat"":""47.49825679999999"",""lon"":""8.747362100000032""},""alr"":{""pro"":""LORA/Generic"",""ver"":""1""}},""ModelCfg"":""1:New"",""AppSKey"":""d5bbf0b5f659f7d72eef31182a1788cc"",""InstantPER"":""0.000000"",""MeanPER"":""0.000000"",""DevAddr"":""094B5DAE""}}";

        private const string TestMeasurementMessageTTN_New     = @"{""app_id"":""ldtapplication"",""dev_id"":""ldtdevice1"",""hardware_serial"":""0004A30B001EC250"",""port"":1,""counter"":10,""payload_raw"":""AQUCAAAAAAA="",""metadata"":{""time"":""2017-12-06T14:35:55.628931145Z"",""frequency"":868.5,""modulation"":""LORA"",""data_rate"":""SF12BW125"",""coding_rate"":""4/5"",""gateways"":[{""gtw_id"":""eui-c0ee40ffff29356b"",""timestamp"":1894545724,""time"":"""",""channel"":2,""rssi"":-60,""snr"":6.8,""rf_chain"":1,""latitude"":47.49873,""longitude"":8.746949,""location_source"":""registry""}]},""downlink_url"":""https://integrations.thethingsnetwork.org/ttn-eu/api/v2/down/ldtapplication/httpldttest?key=ttn-account-v2.4vThJdZ2IszcdwppUzCALWsBmF1_GszPXEOglkR3AfA""}";
        private const string TestMeasurementMessageTTN_Invalid = @"{""app_id"":""ldtapplication"",""dev_id"":""ldtdevice11"",""hardware_serial"":""0004A30B001EC21E"",""port"":1,""counter"":0,""payload_raw"":""HwgC"",""metadata"":{""time"":""2017-12-11T14:56:06.732443282Z""},""downlink_url"":""https://integrations.thethingsnetwork.org/ttn-eu/api/v2/down/ldtapplication/httpldttest?key=ttn-account-v2.4vThJdZ2IszcdwppUzCALWsBmF1_GszPXEOglkR3AfA""}";

        private const string ActilityMessageWithoutLocation    = "{\"DevEUI_uplink\":{\"Time\":\"2018-03-06T09:40:06.523+01:00\",\"DevEUI\":\"0004A30B0020DCBB\",\"FPort\":\"1\",\"FCntUp\":\"20\",\"ADRbit\":\"1\",\"MType\":\"2\",\"FCntDn\":\"3\",\"payload_hex\":\"010501d37fc000007fc000007fc000003f709bfa41bdc28f3920ac08\",\"mic_hex\":\"e3747b85\",\"Lrcid\":\"00000401\",\"LrrRSSI\":\"-78.000000\",\"LrrSNR\":\"10.250000\",\"SpFact\":\"7\",\"SubBand\":\"G1\",\"Channel\":\"LC3\",\"DevLrrCnt\":\"4\",\"Lrrid\":\"004A09F8\",\"Late\":\"0\",\"Lrrs\":{\"Lrr\":[{\"Lrrid\":\"004A09F8\",\"Chain\":\"0\",\"LrrRSSI\":\"-78.000000\",\"LrrSNR\":\"10.250000\",\"LrrESP\":\"-78.391785\"},{\"Lrrid\":\"080E0681\",\"Chain\":\"0\",\"LrrRSSI\":\"-97.000000\",\"LrrSNR\":\"8.750000\",\"LrrESP\":\"-97.543648\"},{\"Lrrid\":\"080E04D6\",\"Chain\":\"0\",\"LrrRSSI\":\"-118.000000\",\"LrrSNR\":\"-2.500000\",\"LrrESP\":\"-122.437759\"}]},\"CustomerID\":\"100000526\",\"CustomerData\":{\"alr\":{\"pro\":\"LORA/Generic\",\"ver\":\"1\"}},\"ModelCfg\":\"1:New\",\"InstantPER\":\"0.000000\",\"MeanPER\":\"0.000000\",\"DevAddr\":\"08D31958\"}}";


        /// <summary>
        /// Send from LEO5
        /// </summary>
        private const string TestMeasurementMessagePocActility = @"{""DevEUI_uplink"": {""Time"": ""2017-11-08T10:53:25.85+01:00"",""DevEUI"": ""0004A30B001B0594"",""FPort"": ""1"",""FCntUp"": ""4166"",""ADRbit"": ""1"",""MType"": ""2"",""FCntDn"": ""847"",""payload_hex"": ""e1400112408572e400063f77668041beb820"",""mic_hex"": ""344b6cba"",""Lrcid"": ""00000401"",""LrrRSSI"": ""-54.000000"",""LrrSNR"": ""9.500000"",""SpFact"": ""7"",""SubBand"": ""G0"",""Channel"": ""LC7"",""DevLrrCnt"": ""1"",""Lrrid"": ""004A09F8"",""Late"": ""0"",""Lrrs"": {""Lrr"": [{""Lrrid"": ""004A09F8"",""Chain"": ""0"",""LrrRSSI"": ""-54.000000"",""LrrSNR"": ""9.500000"",""LrrESP"": ""-54.461838""}]},""CustomerID"": ""100000526"",""CustomerData"": {""loc"":{""lat"":""47.49839820938545"",""lon"":""8.747560054052883""},""alr"":{""pro"":""LORA/Generic"",""ver"":""1""}},""ModelCfg"": ""0"",""AppSKey"": ""9441e6e88ec49870d227c89b4022617a"",""InstantPER"": ""0.000000"",""MeanPER"": ""0.001398"",""DevAddr"": ""081984A3""}}";

        private const string TestMeasurementMessageLoriot      = @"{""cmd"":""rx"",""seqno"":1131,""EUI"":""0004A30B001FFA11"",""ts"":1522938365141,""fcnt"":4,""port"":1,""freq"":868300000,""rssi"":-101,""snr"":6.2,""toa"":1974,""dr"":""SF12 BW125 4/5"",""ack"":false,""bat"":251,""data"":""010500d37fc000007fc000007fc000003f77006941cacccd""}";



        private const string TTN_V3_Message_01 = "{\r\n  \"end_device_ids\": {\r\n    \"device_id\": \"adt1-sn20013-entwicklung\",\r\n    \"application_ids\": {\r\n      \"application_id\": \"entw-adt1\"\r\n    },\r\n    \"dev_eui\": \"353831356C389101\",\r\n    \"join_eui\": \"70B3D57ED002179F\",\r\n    \"dev_addr\": \"260BC8A0\"\r\n  },\r\n  \"correlation_ids\": [\r\n    \"as:up:01EZPCTEMHWXY2MJXGB0QDCY34\",\r\n    \"gs:conn:01EZMXPHH6RBFG6TPA0R3N1QAT\",\r\n    \"gs:up:host:01EZMXPHHNDX16DZCG6XS0X1CA\",\r\n    \"gs:uplink:01EZPCTEDY2RPDWDXCDP8JMH0X\",\r\n    \"ns:uplink:01EZPCTEDZX1MR8M36GSE6P4XS\",\r\n    \"rpc:/ttn.lorawan.v3.GsNs/HandleUplink:01EZPCTEDZX45PRQTWJ1XZGKMF\",\r\n    \"rpc:/ttn.lorawan.v3.NsAs/HandleUplink:01EZPCTEMG8HDCH891AXH9MM98\"\r\n  ],\r\n  \"received_at\": \"2021-03-01T08:00:08.849987767Z\",\r\n  \"uplink_message\": {\r\n    \"session_key_id\": \"AXfoS9r54Rywg6u/UqU49g==\",\r\n    \"f_port\": 1,\r\n    \"f_cnt\": 252,\r\n    \"frm_payload\": \"AQMA2gAAAAB/wAAAQaOZmj96SJVBtI9c\",\r\n    \"decoded_payload\": {\r\n      \"P1\": 0,\r\n      \"PBaro\": 0.9776700139045715,\r\n      \"T\": 5.104235503814077e+38,\r\n      \"TBaro\": 22.56999969482422,\r\n      \"TOB1\": 20.450000762939453,\r\n      \"channel\": \"0000000011011010\",\r\n      \"channelCount\": 5,\r\n      \"ct\": 3,\r\n      \"func\": 1,\r\n      \"payload\": \"010300DA000000007FC0000041A3999A3F7A489541B48F5C\",\r\n      \"port\": 1\r\n    },\r\n    \"rx_metadata\": [\r\n      {\r\n        \"gateway_ids\": {\r\n          \"gateway_id\": \"lorix1-gtw-entwicklung\",\r\n          \"eui\": \"FCC23DFFFE0B6011\"\r\n        },\r\n        \"time\": \"2021-03-01T08:00:08.572175979Z\",\r\n        \"timestamp\": 2165657419,\r\n        \"rssi\": -60,\r\n        \"channel_rssi\": -60,\r\n        \"snr\": 8.75,\r\n        \"location\": {\r\n          \"latitude\": 47.498386913659495,\r\n          \"longitude\": 8.747509717941286,\r\n          \"altitude\": 450,\r\n          \"source\": \"SOURCE_REGISTRY\"\r\n        },\r\n        \"uplink_token\": \"CiQKIgoWbG9yaXgxLWd0dy1lbnR3aWNrbHVuZxII/MI9//4LYBEQy57ViAgaDAiIvvKBBhCuh7qwAiD4+fbZg54L\"\r\n      },\r\n      {\r\n        \"gateway_ids\": {\r\n          \"gateway_id\": \"packetbroker\"\r\n        },\r\n        \"packet_broker\": {\r\n          \"message_id\": \"01EZPCTEDX5KAEYB0K9EQE9PCK\",\r\n          \"forwarder_net_id\": \"000013\",\r\n          \"forwarder_tenant_id\": \"ttn\",\r\n          \"forwarder_cluster_id\": \"ttn-v2-eu-4\",\r\n          \"home_network_net_id\": \"000013\",\r\n          \"home_network_tenant_id\": \"ttn\",\r\n          \"home_network_cluster_id\": \"ttn-eu1\",\r\n          \"hops\": [\r\n            {\r\n              \"received_at\": \"2021-03-01T08:00:08.637672291Z\",\r\n              \"sender_address\": \"52.169.150.138\",\r\n              \"receiver_name\": \"router-dataplane-7b8dc4849b-n96r4\",\r\n              \"receiver_agent\": \"pbdataplane/1.4.1 go/1.15.8 linux/amd64\"\r\n            },\r\n            {\r\n              \"received_at\": \"2021-03-01T08:00:08.663550444Z\",\r\n              \"sender_name\": \"router-dataplane-7b8dc4849b-n96r4\",\r\n              \"sender_address\": \"forwarder_uplink\",\r\n              \"receiver_name\": \"router-7fd4d77db-5gcql\",\r\n              \"receiver_agent\": \"pbrouter/1.4.1 go/1.15.8 linux/amd64\"\r\n            },\r\n            {\r\n              \"received_at\": \"2021-03-01T08:00:08.668361733Z\",\r\n              \"sender_name\": \"router-7fd4d77db-5gcql\",\r\n              \"sender_address\": \"deliver.000013_ttn_ttn-eu1.uplink\",\r\n              \"receiver_name\": \"router-dataplane-7b8dc4849b-n96r4\",\r\n              \"receiver_agent\": \"pbdataplane/1.4.1 go/1.15.8 linux/amd64\"\r\n            }\r\n          ]\r\n        },\r\n        \"time\": \"2021-03-01T08:00:08.611388Z\",\r\n        \"rssi\": -60,\r\n        \"channel_rssi\": -60,\r\n        \"snr\": 9.2,\r\n        \"uplink_token\": \"eyJnIjoiWlhsS2FHSkhZMmxQYVVwQ1RWUkpORkl3VGs1VE1XTnBURU5LYkdKdFRXbFBhVXBDVFZSSk5GSXdUazVKYVhkcFlWaFphVTlwU2sxUmF6VkNWbXM1WmxWRlNrMVRNREUyVlZSVmVrbHBkMmxrUjBadVNXcHZhVlZHYjNkUk0xcERVVEJvY0ZKcE1VaFVSbkF5WkZad2EyUldhRWxhZVVvNUxqQmtOVU15VEc1TWQyZFpOVlp4WDE5ak5FTmhNVUV1WXpsb1dFUndUVjltWXpGQ1YxVktWUzVRY2t0c1lYZFFRVFZDY1RWR1YySkRkSFIyTUhONlJrNW1kVTluUlhsc1dXTTRjakY0VnpGSWNGcGtZazQzTWpsQ05EQmpjbFpqUjFoV01reFVTVzVUYUhCdU0xRnJVbWRCUVhGd1NVUnBOVTA0VnpsMFNYTkpSazVGUlVzMmMxWXRkVk00UkdJNWJqUlpabkJqYlc5VVpVMHRhVXhTU0c5QmNrbE9WemhYU2xWS2VGTlFSWE56TmxwelRHMDFORVJhYkc0M1RsbE1RVE5zV25GVmNIRXdTRzlJUkZCVFIwVjZlVVI0TGxGMlJVRnhNMDVCUTJGdVowRTNTelZqTTJjeU5tYz0iLCJhIjp7ImZuaWQiOiIwMDAwMTMiLCJmdGlkIjoidHRuIiwiZmNpZCI6InR0bi12Mi1ldS00In19\"\r\n      },\r\n      {\r\n        \"gateway_ids\": {\r\n          \"gateway_id\": \"packetbroker\"\r\n        },\r\n        \"packet_broker\": {\r\n          \"message_id\": \"01EZPCTEFJETFDTP2DCYGB36CK\",\r\n          \"forwarder_net_id\": \"000013\",\r\n          \"forwarder_tenant_id\": \"ttn\",\r\n          \"forwarder_cluster_id\": \"ttn-v2-eu-2\",\r\n          \"home_network_net_id\": \"000013\",\r\n          \"home_network_tenant_id\": \"ttn\",\r\n          \"home_network_cluster_id\": \"ttn-eu1\",\r\n          \"hops\": [\r\n            {\r\n              \"received_at\": \"2021-03-01T08:00:08.690052748Z\",\r\n              \"sender_address\": \"52.169.73.251\",\r\n              \"receiver_name\": \"router-dataplane-7b8dc4849b-n96r4\",\r\n              \"receiver_agent\": \"pbdataplane/1.4.1 go/1.15.8 linux/amd64\"\r\n            },\r\n            {\r\n              \"received_at\": \"2021-03-01T08:00:08.691628025Z\",\r\n              \"sender_name\": \"router-dataplane-7b8dc4849b-n96r4\",\r\n              \"sender_address\": \"forwarder_uplink\",\r\n              \"receiver_name\": \"router-7fd4d77db-5gcql\",\r\n              \"receiver_agent\": \"pbrouter/1.4.1 go/1.15.8 linux/amd64\"\r\n            },\r\n            {\r\n              \"received_at\": \"2021-03-01T08:00:08.693071167Z\",\r\n              \"sender_name\": \"router-7fd4d77db-5gcql\",\r\n              \"sender_address\": \"deliver.000013_ttn_ttn-eu1.uplink\",\r\n              \"receiver_name\": \"router-dataplane-7b8dc4849b-n96r4\",\r\n              \"receiver_agent\": \"pbdataplane/1.4.1 go/1.15.8 linux/amd64\"\r\n            }\r\n          ]\r\n        },\r\n        \"time\": \"2021-03-01T08:00:09Z\",\r\n        \"rssi\": -66,\r\n        \"channel_rssi\": -66,\r\n        \"snr\": 9.5,\r\n        \"uplink_token\": \"eyJnIjoiWlhsS2FHSkhZMmxQYVVwQ1RWUkpORkl3VGs1VE1XTnBURU5LYkdKdFRXbFBhVXBDVFZSSk5GSXdUazVKYVhkcFlWaFphVTlwU2xGaVJWWlJVMnhXVEZrd1ZtMU9SazV5VlRCVmVrbHBkMmxrUjBadVNXcHZhVnByTldaVlJYUlVWRVpHU0dOVlVrcGlSWEJYWTIxS2RWTkdXbGRWVTBvNUxpMDBUMGQ1Ukd0Q1ZUUlZRMDU1YlhCaFFsSjZaMmN1UkZWSk1HUm9RVTlRVUVvNFRYQnVaeTVGV1VSU1kweEZiRm95VUZCc2JXOVVZbEJSWjBkNVpuVmhTbVZxWDIxVlZUUTFTbVZhTVV0MlkzQjFObXBrTXpKdlptYzJTSFZwVDNGb2NVcFJSbTQzU0RST2IwODVlbkZpTUhFMVZXbHVlRVIxTUZkNk4ydG5ibk5SVmxFMFJIWkxORUZxTUZoSFNWWlNhbTFYWDJGc1lWTlZVMk01TVVwQ1JGTkdXbXQxZFdzMFRWbE1XSGQyVWpaa2JVWjJZMU4wUkZOb0xsQkNjVzUzWnpONFFYbGFja3BmZEhGbmFuSjNiSGM9IiwiYSI6eyJmbmlkIjoiMDAwMDEzIiwiZnRpZCI6InR0biIsImZjaWQiOiJ0dG4tdjItZXUtMiJ9fQ==\"\r\n      }\r\n    ],\r\n    \"settings\": {\r\n      \"data_rate\": {\r\n        \"lora\": {\r\n          \"bandwidth\": 125000,\r\n          \"spreading_factor\": 7\r\n        }\r\n      },\r\n      \"data_rate_index\": 5,\r\n      \"coding_rate\": \"4/5\",\r\n      \"frequency\": \"868300000\",\r\n      \"timestamp\": 2165657419,\r\n      \"time\": \"2021-03-01T08:00:08.572175979Z\"\r\n    },\r\n    \"received_at\": \"2021-03-01T08:00:08.639856218Z\",\r\n    \"consumed_airtime\": \"0.082176s\",\r\n    \"locations\": {\r\n      \"user\": {\r\n        \"latitude\": 47.498390537919015,\r\n        \"longitude\": 8.74755531549454,\r\n        \"altitude\": 450,\r\n        \"source\": \"SOURCE_REGISTRY\"\r\n      }\r\n    }\r\n  }\r\n}";

        private const string TTN_V3_Message_02 = "{\"end_device_ids\":{\"device_id\":\"adt1-sn20013-entwicklung\",\"application_ids\":{\"application_id\":\"entw-adt1\"},\"dev_eui\":\"353831356C389101\",\"join_eui\":\"70B3D57ED002179F\",\"dev_addr\":\"260B62E8\"},\"correlation_ids\":[\"as:up:01EZVQM4W5GNQBWRHHXG7JBNRP\",\"gs:conn:01EZTZYC4121HZY7FAMKKPEB55\",\"gs:up:host:01EZTZYC4FWJ1VMHJFCVNE492S\",\"gs:uplink:01EZVQM4NA4YKESGMHPF86G15B\",\"ns:uplink:01EZVQM4NC7TRRVACHN98PYBNR\",\"rpc:/ttn.lorawan.v3.GsNs/HandleUplink:01EZVQM4NBYM5PE4RDKRJ59NDX\",\"rpc:/ttn.lorawan.v3.NsAs/HandleUplink:01EZVQM4W4Q3M7EZR3GQMVBV23\"],\"received_at\":\"2021-03-03T09:45:08.741610899Z\",\"uplink_message\":{\"session_key_id\":\"AXfymJKY6zO8bMf2m3Qaeg==\",\"f_port\":1,\"f_cnt\":276,\"frm_payload\":\"AQMA2jiAAAB/wAAAQaczMz96ZWtBtAAA\",\"decoded_payload\":{\"P1\":0.00006103515625,\"PBaro\":0.9781100153923035,\"T\":5.104235503814077e+38,\"TBaro\":22.5,\"TOB1\":20.899999618530273,\"channel\":\"0000000011011010\",\"channelCount\":5,\"ct\":3,\"func\":1,\"payload\":\"010300DA388000007FC0000041A733333F7A656B41B40000\",\"port\":1},\"rx_metadata\":[{\"gateway_ids\":{\"gateway_id\":\"lorix1-gtw-entwicklung\",\"eui\":\"FCC23DFFFE0B6011\"},\"time\":\"2021-03-03T09:45:08.471900939Z\",\"timestamp\":3354968300,\"rssi\":-79,\"channel_rssi\":-79,\"snr\":10,\"location\":{\"latitude\":47.498386913659495,\"longitude\":8.747509717941286,\"altitude\":450,\"source\":\"SOURCE_REGISTRY\"},\"uplink_token\":\"CiQKIgoWbG9yaXgxLWd0dy1lbnR3aWNrbHVuZxII/MI9//4LYBEQ7IHjvwwaDAiktf2BBhCJ1Jv5ASDgs8ad0tIF\"},{\"gateway_ids\":{\"gateway_id\":\"packetbroker\"},\"packet_broker\":{\"message_id\":\"01EZVQM4P9KAMWNCNKB5T8K7V1\",\"forwarder_net_id\":\"000013\",\"forwarder_tenant_id\":\"ttn\",\"forwarder_cluster_id\":\"ttn-v2-eu-4\",\"home_network_net_id\":\"000013\",\"home_network_tenant_id\":\"ttn\",\"home_network_cluster_id\":\"ttn-eu1\",\"hops\":[{\"received_at\":\"2021-03-03T09:45:08.553847719Z\",\"sender_address\":\"52.169.150.138\",\"receiver_name\":\"router-dataplane-7b8dc4849b-n96r4\",\"receiver_agent\":\"pbdataplane/1.4.1 go/1.15.8 linux/amd64\"},{\"received_at\":\"2021-03-03T09:45:08.560009424Z\",\"sender_name\":\"router-dataplane-7b8dc4849b-n96r4\",\"sender_address\":\"forwarder_uplink\",\"receiver_name\":\"router-7fd4d77db-pcgpx\",\"receiver_agent\":\"pbrouter/1.4.1 go/1.15.8 linux/amd64\"},{\"received_at\":\"2021-03-03T09:45:08.563011878Z\",\"sender_name\":\"router-7fd4d77db-pcgpx\",\"sender_address\":\"deliver.000013_ttn_ttn-eu1.uplink\",\"receiver_name\":\"router-dataplane-7b8dc4849b-n96r4\",\"receiver_agent\":\"pbdataplane/1.4.1 go/1.15.8 linux/amd64\"}]},\"time\":\"2021-03-03T09:45:08.504928Z\",\"rssi\":-77,\"channel_rssi\":-77,\"snr\":9.2,\"uplink_token\":\"eyJnIjoiWlhsS2FHSkhZMmxQYVVwQ1RWUkpORkl3VGs1VE1XTnBURU5LYkdKdFRXbFBhVXBDVFZSSk5GSXdUazVKYVhkcFlWaFphVTlwU25KVFYzUjNWa2hPV1ZKcVVYZFRSVGx2WWpOb1dFbHBkMmxrUjBadVNXcHZhV1ZXVVhwak1HUlRZVmh3Y0ZaWFpGZGhWMnhXVlRKNFQwNUVRa1JWVTBvNUxpMU9ja3BoZDNKVFlXVkVRemMzUVhSYWNqRnlWRkV1VEVFeVQwZDBVMVpZTVROM1VHdElieTVOY1VKTllUSXlZakJtVm1KaVRVNUlWRGhwUWw5U1NtRm9WRWd5VFhOelIyZFVhRk5EWm01SU5XSnlZVXRQUldrMWFsSnBaMmt5Ym10TmJqRTJZVWxNYUdsVE1tZFJaazFGUTI5d1ZXMHdjSGhGUldJeVdrbHFTWEF3UW1SUVZYSkdMVUYxZWxCU1kwRjBkRWg1YldsVFVWWmlPVGRHUkdkeVFXeGhZMDlQVGxRNVVqTm9SazAyWjBoRFQybDFjRGxzWjB3eGNYcDBOR3RLVlhoclUxOUVPWEoxWmpCaFlUaE5Ua3haTG1VMGJWSkVRbUZITFRCTk5XaHJXR0pDTUUxd01sRT0iLCJhIjp7ImZuaWQiOiIwMDAwMTMiLCJmdGlkIjoidHRuIiwiZmNpZCI6InR0bi12Mi1ldS00In19\"},{\"gateway_ids\":{\"gateway_id\":\"packetbroker\"},\"packet_broker\":{\"message_id\":\"01EZVQM4ST1YK1T8X7ZHCAA8VB\",\"forwarder_net_id\":\"000013\",\"forwarder_tenant_id\":\"ttn\",\"forwarder_cluster_id\":\"ttn-v2-eu-2\",\"home_network_net_id\":\"000013\",\"home_network_tenant_id\":\"ttn\",\"home_network_cluster_id\":\"ttn-eu1\",\"hops\":[{\"received_at\":\"2021-03-03T09:45:08.666834515Z\",\"sender_address\":\"52.169.73.251\",\"receiver_name\":\"router-dataplane-7b8dc4849b-q4pg4\",\"receiver_agent\":\"pbdataplane/1.4.1 go/1.15.8 linux/amd64\"},{\"received_at\":\"2021-03-03T09:45:08.668145979Z\",\"sender_name\":\"router-dataplane-7b8dc4849b-q4pg4\",\"sender_address\":\"forwarder_uplink\",\"receiver_name\":\"router-7fd4d77db-5gcql\",\"receiver_agent\":\"pbrouter/1.4.1 go/1.15.8 linux/amd64\"},{\"received_at\":\"2021-03-03T09:45:08.670580471Z\",\"sender_name\":\"router-7fd4d77db-5gcql\",\"sender_address\":\"deliver.000013_ttn_ttn-eu1.uplink\",\"receiver_name\":\"router-dataplane-7b8dc4849b-n96r4\",\"receiver_agent\":\"pbdataplane/1.4.1 go/1.15.8 linux/amd64\"}]},\"time\":\"2021-03-03T09:45:09Z\",\"rssi\":-75,\"channel_rssi\":-75,\"snr\":9.5,\"uplink_token\":\"eyJnIjoiWlhsS2FHSkhZMmxQYVVwQ1RWUkpORkl3VGs1VE1XTnBURU5LYkdKdFRXbFBhVXBDVFZSSk5GSXdUazVKYVhkcFlWaFphVTlwU2xCUlZFVXpUbTVLU1UxNlVuWlZSM2hZVTBoU2NVbHBkMmxrUjBadVNXcHZhVnBGVWxSaE1EbDFURlJvY1UxV1NrWlVNVnBEWkVkU1IwOUhXbFJrZVVvNUxrTk9XbTVaYjFVMmFsZzJNV0o1VDNsT1JsOTRNbWN1YjNFNU9VRkNORVZrUTA1aVZDMUtUeTVSVFZoUWIxQkpVRmx5U2pGNWFUQklkRkZLYTJkaE5YRlFUbEZRYzNWU2VVOURURGhxUlZOTE1WVlhZVWN3Ym5OeldFeDRiV1ZWYTFBelUxQnhRMmd0U0ZCTlIzSkJPRGRUT1V4Tk5XRnZZak5YZDFKS2FqWktWRzFZUWpkb1UwVmlXa3BpWkZSNlJYbG5ZVWR0ZFhGRFZWZGpVV0pMZGpOQk9IaFhUbk13T0d4RldqRmFRM3ByWjNCMVl6UjNZMmN5TWpOUUxqZDVUVEpXWnkxS1QyOWtYekpVWldWTVJXUkNjM2M9IiwiYSI6eyJmbmlkIjoiMDAwMDEzIiwiZnRpZCI6InR0biIsImZjaWQiOiJ0dG4tdjItZXUtMiJ9fQ==\"}],\"settings\":{\"data_rate\":{\"lora\":{\"bandwidth\":125000,\"spreading_factor\":7}},\"data_rate_index\":5,\"coding_rate\":\"4/5\",\"frequency\":\"867700000\",\"timestamp\":3354968300,\"time\":\"2021-03-03T09:45:08.471900939Z\"},\"received_at\":\"2021-03-03T09:45:08.524139640Z\",\"consumed_airtime\":\"0.082176s\",\"locations\":{\"user\":{\"latitude\":47.498390537919015,\"longitude\":8.74755531549454,\"altitude\":450,\"source\":\"SOURCE_REGISTRY\"}}}}";
    }
}