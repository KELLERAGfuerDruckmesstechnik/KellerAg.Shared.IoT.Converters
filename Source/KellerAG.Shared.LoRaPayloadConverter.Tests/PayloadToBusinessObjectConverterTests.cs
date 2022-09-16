using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KellerAg.Shared.LoRaPayloadConverter.Tests
{
    [TestClass()]
    public class PayloadToBusinessObjectConverterTests
    {
        [TestMethod()]
        public void WhenGivenANullPayload_ThenNoFatalErrorShouldArise()
        {
            var extractedInformation = PayloadConverter.ConvertFromTheThingNetwork(null, Int32.MaxValue);
            extractedInformation.IsSupportedDevice.Should().BeFalse();
        }


        [TestMethod()]
        public void WhenGivenAValidPayloadWithNaNFloats_ThenValuesShouldBeCorrect()
        {
            var extractedInformation = PayloadConverter.ConvertFromActility("010501d37fc000007fc000007fc000003f76c8b441bb33333920ac08", 1);

            extractedInformation.FunctionCode.Should().Be(1);
            extractedInformation.DecodedPayload.Length.Should().NotBe(0);
            extractedInformation.Measurements.Count.Should().Be(6);
            extractedInformation.Measurements.ToArray()[3].Value.Should().Be(0.964f);
            extractedInformation.Measurements.ToArray()[4].Value.Should().Be(23.4f);
            extractedInformation.Measurements.ToArray()[5].Value.Should().Be(0.00015322875f);

            extractedInformation.Measurements.ToArray()[0].ChannelNumber.Should().Be(1);
            extractedInformation.Measurements.ToArray()[1].ChannelNumber.Should().Be(2);
            extractedInformation.Measurements.ToArray()[2].ChannelNumber.Should().Be(5);
            extractedInformation.Measurements.ToArray()[3].ChannelNumber.Should().Be(7);
            extractedInformation.Measurements.ToArray()[4].ChannelNumber.Should().Be(8);
            extractedInformation.Measurements.ToArray()[5].ChannelNumber.Should().Be(9);
            
            extractedInformation.Measurements.ToArray()[0].MeasurementDefinitionId.Should().Be(11);
            extractedInformation.Measurements.ToArray()[1].MeasurementDefinitionId.Should().Be(2);
            extractedInformation.Measurements.ToArray()[2].MeasurementDefinitionId.Should().Be(5);
            extractedInformation.Measurements.ToArray()[3].MeasurementDefinitionId.Should().Be(7);
            extractedInformation.Measurements.ToArray()[4].MeasurementDefinitionId.Should().Be(8);
            extractedInformation.Measurements.ToArray()[5].MeasurementDefinitionId.Should().Be(9);

            extractedInformation.Measurements.ToArray()[0].Value.Should().Be(float.NaN);
        }

        [TestMethod()]
        public void WhenGivenAValidTTNPayload_ThenValuesShouldBeCorrect()
        {
            var extractedInformation = PayloadConverter.ConvertFromTheThingNetwork("AQswAD93JxNBu1wp",(int)FunctionCodeId.MeasurementMessage);

            extractedInformation.FunctionCode.Should().Be(1);
            extractedInformation.DecodedPayload.Length.Should().NotBe(0);
            extractedInformation.Measurements.Count.Should().Be(2);

            extractedInformation.Measurements.ToArray()[0].Value.Should().Be(0.96544f);
            extractedInformation.Measurements.ToArray()[1].Value.Should().Be(23.42f);

            extractedInformation.Measurements.ToArray()[0].MeasurementDefinitionId.Should().Be(7);
            extractedInformation.Measurements.ToArray()[1].MeasurementDefinitionId.Should().Be(8);
        }

        [TestMethod()]
        public void WhenGivenAValidTTNPayloadWithPort4_ThenDeviceInformationShouldBeCorrect()
        {
            var payload = new byte[]
            {
                0x0C,
                0x01,
                0x13,
                0x00,
                0x13,
                0x2D,
                0x00,
                0x00,
                0x00,
                0x1C,
                0x25,
                0x4F,
                0x1D,
                0xB0,
                0x40,
                0x9D,
                0xF3,
                0x2C,
                0x63,
                0x28
            };

            var payloadText = System.Convert.ToBase64String(payload);

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromTheThingNetwork(payloadText, 4);

            extractedInformation.DecodedPayload.Length.Should().Be(20);
            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.InfoMessage);
            extractedInformation.Measurements.Should().BeNull();

            extractedInformation.Info.BatteryCapacity.Should().Be(99);
            extractedInformation.Info.BatteryVoltage.Should().BeInRange(4.93593f, 4.93594f);
            extractedInformation.Info.DeviceClassGroupText.Should().Be("19.00");
            extractedInformation.Info.DeviceLocalDateTime.Should().BeCloseTo(new DateTime(2019, 11, 01, 16, 45, 36));
            extractedInformation.Info.HumidityPercentage.Should().Be(40);
            extractedInformation.Info.SerialNumber.Should().Be(28);
            extractedInformation.Info.SwVersionText.Should().Be("19.45");
        }



        /// <summary>
        /// Protocol page 5 - Example with 0x01
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWithMeasurements_ThenValuesShouldBeCorrect()
        {
            var extractedInformation = PayloadConverter.ConvertFromActility("010300D3BF7595F03C5C91304249C7C03F79081C42477AE1", 1);

            extractedInformation.FunctionCode.Should().Be(1);

            extractedInformation.Measurements.Count.Should().Be(5);
            extractedInformation.Measurements.ToArray()[0].Value.Should().Be(-0.9593191f);
            extractedInformation.Measurements.ToArray()[1].Value.Should().Be(0.01346235f);
            extractedInformation.Measurements.ToArray()[2].Value.Should().Be(50.44507f);
            extractedInformation.Measurements.ToArray()[3].Value.Should().Be(0.97278f);
            extractedInformation.Measurements.ToArray()[4].Value.Should().Be(49.87f);

            extractedInformation.Measurements.ToArray()[0].ChannelNumber.Should().Be(1);
            extractedInformation.Measurements.ToArray()[1].ChannelNumber.Should().Be(2);
            extractedInformation.Measurements.ToArray()[2].ChannelNumber.Should().Be(5);
            extractedInformation.Measurements.ToArray()[3].ChannelNumber.Should().Be(7);
            extractedInformation.Measurements.ToArray()[4].ChannelNumber.Should().Be(8);

            extractedInformation.Measurements.ToArray()[0].MeasurementDefinitionId.Should().Be(11);
            extractedInformation.Measurements.ToArray()[1].MeasurementDefinitionId.Should().Be(2);
            extractedInformation.Measurements.ToArray()[2].MeasurementDefinitionId.Should().Be(5);
            extractedInformation.Measurements.ToArray()[3].MeasurementDefinitionId.Should().Be(7);
            extractedInformation.Measurements.ToArray()[4].MeasurementDefinitionId.Should().Be(8);
        }

        /// <summary>
        /// Protocol - Page 7 - Information Message example
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWithInformationMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "0C011300132F000000642576AFAA4098B368631D";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            extractedInformation.DecodedPayload.Length.Should().Be(20);
            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.InfoMessage);
            extractedInformation.Measurements.Should().BeNull();

            extractedInformation.Info.BatteryCapacity.Should().Be(99);
            extractedInformation.Info.BatteryVoltage.Should().BeInRange(4.7719f, 4.77191f);
            extractedInformation.Info.DeviceClassGroupText.Should().Be("19.00");
            extractedInformation.Info.DeviceLocalDateTime.Should().BeCloseTo(new DateTime(2019, 12, 01, 17, 06, 50));
            extractedInformation.Info.HumidityPercentage.Should().Be(29);
            extractedInformation.Info.SerialNumber.Should().Be(100);
            extractedInformation.Info.SwVersionText.Should().Be("19.47");
        }

        /// <summary>
        /// Protocol - Page 8 - 1 Byte Message example
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWith8BitMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "1F011302000313042F0DFF0E000F00100115051603";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOf8BitMessage);
            extractedInformation.Measurements.Should().BeNull();

            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.DeviceClass].Should().Be(19);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.DeviceGroup].Should().Be(0);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.SWVersionYear].Should().Be(19);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.SWVersionWeek].Should().Be(47);

            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.EventChannel].Should().Be(255);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.EventType].Should().Be(0);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.AlarmChannel].Should().Be(0);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.AlarmType].Should().Be(1);

            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.RadioBandRegion].Should().Be(5);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.LoRaModuleType].Should().Be(3);
        }


        /// <summary>
        /// Protocol - Page 8 - 1 Byte Stream Message example
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWith8BitStreamMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "20011300132F0D010301000500D3FF000001630105010503";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOf8BitStreamMessage);
            extractedInformation.Measurements.Should().BeNull();

            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.DeviceClass].Should()                     .Be(19);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.DeviceGroup].Should()                     .Be(0);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.SWVersionYear].Should()                   .Be(19);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.SWVersionWeek].Should()                   .Be(47);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.SupportedMaxConnectionDeviceType].Should().Be(13);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.UplinkMode].Should()                      .Be(1);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.DeviceType].Should()                      .Be(3);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.IsPowerForExternalDeviceEnabled].Should() .Be(1);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.PowerPreOnTimeInSec].Should()             .Be(0);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.LockTimer].Should()                       .Be(5);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.MeasureSaveChannelsHigh].Should()         .Be(0);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.MeasureSaveChannelsLow].Should()          .Be(211);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.EventChannel].Should()                    .Be(255);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.EventType].Should()                       .Be(0);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.AlarmChannel].Should()                    .Be(0);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.AlarmType].Should()                       .Be(1);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.BatteryCapacityPercentage].Should()       .Be(99);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.IsAdaptiveDataRateOn].Should()            .Be(1);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.DataRate].Should()                        .Be(5);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.PowerIndex].Should()                      .Be(1);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.RadioBandRegion].Should()                 .Be(5);
            extractedInformation.ValuesOf8Bit[IndexOfValuesOf8Bit.LoRaModuleType].Should()                  .Be(3);
        }

        /// <summary>
        /// Protocol - Page 10 - 4 Byte Message example
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWith32BitMessage_ThenValuesShouldBeCorrect()
        {
            //var payloadText = "34 01 00000064 02 25766038 03 00000025 04 25766B51 05 254AD830 06 2577845F 07 00000000 08 00000E10 09 00015180 0A 00015180";
            var payloadText = "330100000064022576603803000000250425766B5105254AD830062577845F07000000000800000E1009000151800A00015180";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOf32BitMessage);
            extractedInformation.Measurements.Should().BeNull();

            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.SerialNumber].Should().Be(100u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.MainTime].Should().Be(628514872u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.MainTimeCorrectionInSec].Should().Be(37u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.NextMeasureDateTime].Should().Be(628517713u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.NextAlarmDateTime].Should().Be(625662000u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.NextInfoDateTime].Should().Be(628589663u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.NextEventMeasuringDateTime].Should().Be(0);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.MeasureIntervalInSec].Should().Be(3600u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.AlarmIntervalInSec].Should().Be(86400u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.InfoIntervalInSec].Should().Be(86400u);
        //    extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.EventCheckIntervalInSec].Should().Be(0);
        //    extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.EventMeasureIntervalInSec].Should().Be(0);

            var dt = PayloadConverter.SecondsAfterYear2000ToDateTime(extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.MainTime]);
            dt.ToString(CultureInfo.InvariantCulture).Should().Be("12/01/2019 11:27:52");
        }


        /// <summary>
        /// Protocol - Page 8 - 4 Byte Stream Message example
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWith32BitStreamMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "340100000064257660380000002525766B51254AD8302577845F0000000000000E1000015180000151800000000000000000";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOf32BitStreamMessage);
            extractedInformation.Measurements.Should().BeNull();

            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.SerialNumber].Should()              .Be(100u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.MainTime].Should()                  .Be(628514872u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.MainTimeCorrectionInSec].Should()   .Be(37u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.NextMeasureDateTime].Should()       .Be(628517713u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.NextAlarmDateTime].Should()         .Be(625662000u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.NextInfoDateTime].Should()          .Be(628589663u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.NextEventMeasuringDateTime].Should().Be(0);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.MeasureIntervalInSec].Should()      .Be(3600u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.AlarmIntervalInSec].Should()        .Be(86400u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.InfoIntervalInSec].Should()         .Be(86400u);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.EventCheckIntervalInSec].Should()   .Be(0);
            extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.EventMeasureIntervalInSec].Should() .Be(0);

            var dt = PayloadConverter.SecondsAfterYear2000ToDateTime(extractedInformation.ValuesOf32Bit[IndexOfValuesOf32Bit.MainTime]);
            dt.AsUtc().ToString(CultureInfo.InvariantCulture).Should().Be("12/01/2019 11:27:52");
        }

        /// <summary>
        /// Protocol - Page 12 - Float Message example without stream
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWithFloatMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "3D1941DCF695183F9E0610";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOfFloatMessage);
            extractedInformation.Measurements.Should().BeNull();

            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.RelativeHumidityPercentage].Should().Be(27.6204014f);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.BatteryVoltage].Should().Be(1.23456f);
        }


        /// <summary>
        /// Protocol - Page 12 - Float Stream Message example - Payload #109
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayload109WithFloatStreamMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "3E013F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF3F8000003F8000003F8000000000000044798CCD3F800000";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOfFloatStreamMessage);
            extractedInformation.Measurements.Should().BeNull();

            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.AlarmDeltaThreshold].Should().Be(1);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.AlarmOffThreshold].Should().Be(1);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.AlarmOnThreshold].Should().Be(1);

            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.EventDeltaThreshold].Should().Be(float.NaN);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.EventOnThreshold].Should().Be(float.NaN);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.EventOffThreshold].Should().Be(float.NaN);

            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.WaterLevelConfigurationEnable].Should().Be(1);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.WaterLevelConfigurationLength_B].Should().Be(1);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.WaterLevelConfigurationHeight_A].Should().Be(1);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.WaterLevelConfigurationOffset_F].Should().Be(0);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.WaterLevelConfigurationDensity].Should().Be(998.2f);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.WaterLevelConfigurationWidth_b].Should().Be(1);
        }

        /// <summary>
        /// Protocol - Page 12 - Float Stream Message example - Payload 111
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayload111WithFloatStreamMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "3E1941DCF69500000000";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOfFloatStreamMessage);
            extractedInformation.Measurements.Should().BeNull();

            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.RelativeHumidityPercentage].Should().Be(27.6204014f);
            extractedInformation.ValuesOfFloat[IndexOfValuesOfFloat.OffsetBarometer].Should().Be(0f);
        }

        /// <summary>
        /// Protocol Page 14 - Payload example 121
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWithASCIIChars_ThenValuesShouldBeCorrect()
        {
            var extractedInformation = PayloadConverter.ConvertFromActility("480001312E302E3032FF30303944364230303030433544333832FF37304233443537454430303234364134FF", 5);

            extractedInformation.DecodedPayload.Length.Should().NotBe(0);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOfTextStreamMessage);
            extractedInformation.Port.Should().Be(5);
            extractedInformation.OriginLoRaNetwork.Should().Be(OriginLoRaNetwork.Actility);

            extractedInformation.ValueOfText.Count.Should().Be(3);
            extractedInformation.ValueOfText[IndexOfTextValues.FirmwareVersionShort].Text.Should().Be("1.0.02");
            extractedInformation.ValueOfText[IndexOfTextValues.DeviceEUI].Text.Should().Be("009D6B0000C5D382");
            extractedInformation.ValueOfText[IndexOfTextValues.ApplicationEUI].Text.Should().Be("70B3D57ED00246A4");

            extractedInformation.ValueOfText.Values.Count(_ => _.HasReachedEndOfText).Should().Be(extractedInformation.ValueOfText.Count);
            extractedInformation.ValueOfText.Values.Count(_ => _.CharacterStartingPosition == 0).Should().Be(extractedInformation.ValueOfText.Count);
        }

        /// <summary>
        /// Protocol Page 14 - Payload example 122
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayload122WithASCIIChars_ThenValuesShouldBeCorrect()
        {
            var extractedInformation = PayloadConverter.ConvertFromActility("4800043331433439344337444644383746303333353546444530413731453842353732FF3236303132314336FF", 5);

            extractedInformation.DecodedPayload.Length.Should().NotBe(0);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOfTextStreamMessage);
            extractedInformation.Port.Should().Be(5);
            extractedInformation.OriginLoRaNetwork.Should().Be(OriginLoRaNetwork.Actility);

            extractedInformation.ValueOfText.Count.Should().Be(2);
            extractedInformation.ValueOfText[IndexOfTextValues.ApplicationKey].Text.Should().Be("31C494C7DFD87F03355FDE0A71E8B572");
            extractedInformation.ValueOfText[IndexOfTextValues.DeviceAddress].Text.Should().Be("260121C6");

            extractedInformation.ValueOfText.Values.Count(_ => _.HasReachedEndOfText).Should().Be(extractedInformation.ValueOfText.Count);
            extractedInformation.ValueOfText.Values.Count(_ => _.CharacterStartingPosition == 0).Should().Be(extractedInformation.ValueOfText.Count);
        }

        /// <summary>
        /// Protocol Page 14 - Payload example 123
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayload123WithASCIIChars_ThenValuesShouldBeCorrect()
        {
            var extractedInformation = PayloadConverter.ConvertFromActility("4800063246413830324332433944354646414146353544453535383430344536363230FF", 5);

            extractedInformation.DecodedPayload.Length.Should().NotBe(0);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOfTextStreamMessage);
            extractedInformation.Port.Should().Be(5);
            extractedInformation.OriginLoRaNetwork.Should().Be(OriginLoRaNetwork.Actility);

            extractedInformation.ValueOfText.Count.Should().Be(1);
            extractedInformation.ValueOfText[IndexOfTextValues.NetworkSessionKey].Text.Should().Be("2FA802C2C9D5FFAAF55DE558404E6620");

            extractedInformation.ValueOfText.Values.Count(_ => _.HasReachedEndOfText).Should().Be(extractedInformation.ValueOfText.Count);
            extractedInformation.ValueOfText.Values.Count(_ => _.CharacterStartingPosition == 0).Should().Be(extractedInformation.ValueOfText.Count);
        }
        

        /// <summary>
        /// Protocol Page 14 - Payload example 124
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayload124WithASCIIChars_ThenValuesShouldBeCorrect()
        {
            var extractedInformation = PayloadConverter.ConvertFromActility("4800073046363932464531434544463737424632383837333944393645444535344137FF", 5);

            extractedInformation.DecodedPayload.Length.Should().NotBe(0);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOfTextStreamMessage);
            extractedInformation.Port.Should().Be(5);
            extractedInformation.OriginLoRaNetwork.Should().Be(OriginLoRaNetwork.Actility);

            extractedInformation.ValueOfText.Count.Should().Be(1);
            extractedInformation.ValueOfText[IndexOfTextValues.AppSessionKey].Text.Should().Be("0F692FE1CEDF77BF288739D96EDE54A7");

            extractedInformation.ValueOfText.Values.Count(_ => _.HasReachedEndOfText).Should().Be(extractedInformation.ValueOfText.Count);
            extractedInformation.ValueOfText.Values.Count(_ => _.CharacterStartingPosition == 0).Should().Be(extractedInformation.ValueOfText.Count);
        }

        /// <summary>
        /// Protocol Page 14 - Like Payload example 121 but split in two messages
        /// </summary>
        [TestMethod()]
        public void WhenGivenTwoSplitPayloadWithASCIIChars_ThenValuesShouldBeCorrect()
        {
            //"480001312E302E3032FF30303944364230303030433544333832FF37304233443537454430303234364134FF"
            //"480001312E302E3032FF30303944364230303030433544333832FF373042"  cut on position 2 (3 chars) without FF
            //"48 + 03 + 33443537454430303234364134FF" 

            var payloadPart1 = "480001312E302E3032FF30303944364230303030433544333832FF373042";
            var payloadPart2 = "48030333443537454430303234364134FF";

            var extractedInformation = PayloadConverter.ConvertFromActility(payloadPart1, 5);

            extractedInformation.DecodedPayload.Length.Should().NotBe(0);
            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOfTextStreamMessage);
            extractedInformation.Port.Should().Be(5);
            extractedInformation.OriginLoRaNetwork.Should().Be(OriginLoRaNetwork.Actility);

            extractedInformation.ValueOfText.Count.Should().Be(3);
            extractedInformation.ValueOfText[IndexOfTextValues.FirmwareVersionShort].Text.Should().Be("1.0.02");
            extractedInformation.ValueOfText[IndexOfTextValues.DeviceEUI].Text.Should().Be("009D6B0000C5D382");
            extractedInformation.ValueOfText[IndexOfTextValues.ApplicationEUI].Text.Should().Be("70B");

            extractedInformation.ValueOfText.Values.Count(_ => _.HasReachedEndOfText).Should().NotBe(extractedInformation.ValueOfText.Count);

            extractedInformation.ValueOfText[IndexOfTextValues.ApplicationEUI].HasReachedEndOfText.Should().BeFalse();
            extractedInformation.ValueOfText[IndexOfTextValues.ApplicationEUI].CharacterStartingPosition.Should().Be(0);

            //payload part 2
            extractedInformation = PayloadConverter.ConvertFromActility(payloadPart2, 5);

            extractedInformation.DecodedPayload.Length.Should().NotBe(0);
            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.ValuesOfTextStreamMessage);
            extractedInformation.Port.Should().Be(5);
            extractedInformation.OriginLoRaNetwork.Should().Be(OriginLoRaNetwork.Actility);

            extractedInformation.ValueOfText.Count.Should().Be(1);

            extractedInformation.ValueOfText[IndexOfTextValues.ApplicationEUI].Text.Should().Be("3D57ED00246A4");

            extractedInformation.ValueOfText.Values.Count(_ => _.CharacterStartingPosition == 0).Should().NotBe(extractedInformation.ValueOfText.Count);

            extractedInformation.ValueOfText[IndexOfTextValues.ApplicationEUI].HasReachedEndOfText.Should().BeTrue();
            extractedInformation.ValueOfText[IndexOfTextValues.ApplicationEUI].CharacterStartingPosition.Should().Be(3);
        }

        /// <summary>
        /// Protocol Page 16 - Payload example
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWithKellerSensorInformation_ThenValuesShouldBeCorrect()
        {
            var extractedInformation = PayloadConverter.ConvertFromActility("51010105140C1C000CEBA1", 5);

            extractedInformation.DecodedPayload.Length.Should().NotBe(0);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.SensorInformationMessage);
            extractedInformation.Port.Should().Be(5);
            extractedInformation.OriginLoRaNetwork.Should().Be(OriginLoRaNetwork.Actility);

            extractedInformation.SensorInfo.SensorType.Should().Be(SensorType.RS485);
            extractedInformation.SensorInfo.SensorCount.Should().Be(1);
            extractedInformation.SensorInfo.SensorType2Data.Should().BeNull();
            extractedInformation.SensorInfo.SensorType1Data.SerialNumber.Should().Be(846753);
            extractedInformation.SensorInfo.SensorType1Data.SensorClass.Should().Be(5);
            extractedInformation.SensorInfo.SensorType1Data.SensorGroup.Should().Be(20);
            extractedInformation.SensorInfo.SensorType1Data.SwVersionWeek.Should().Be(28);
            extractedInformation.SensorInfo.SensorType1Data.SwVersionYear.Should().Be(12);
        }

        /// <summary>
        /// Protocol Page 16 - Payload example with I2C
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWithKellerSensorInformationAndI2C_ThenValuesShouldBeCorrect()
        {
            var extractedInformation = PayloadConverter.ConvertFromActility("5101020407014415740000000041f00000", 5);

            extractedInformation.DecodedPayload.Length.Should().NotBe(0);

            extractedInformation.FunctionCode.Should().Be(FunctionCodeId.SensorInformationMessage);
            extractedInformation.Port.Should().Be(5);
            extractedInformation.OriginLoRaNetwork.Should().Be(OriginLoRaNetwork.Actility);

            extractedInformation.SensorInfo.SensorType.Should().Be(SensorType.InterIntegratedCircuit);
            extractedInformation.SensorInfo.SensorCount.Should().Be(1);
            extractedInformation.SensorInfo.SensorType1Data.Should().BeNull();

            extractedInformation.SensorInfo.SensorType2Data.UniqueId.Should().Be(67567940);
            extractedInformation.SensorInfo.SensorType2Data.Scaling0.Should().Be(5492);
            extractedInformation.SensorInfo.SensorType2Data.PMinVal.Should().Be(0f);
            extractedInformation.SensorInfo.SensorType2Data.PMaxVal.Should().Be(30f);
        }


        [TestMethod()]
        public void WhenRequestedASmallConfiguration_ThenValuesShouldBeCorrect()
        {

            //After Requesting:
            //5A020000 on port 3
            // 5 messages where replied on port 3
            /*
                20011300132F0D010301000500D3FF000001550005010503
                3401000000072691F271000000262691F4BC2538552026931E64000000000000025800000708000151800000000000000000
                3E013F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF000000003F8000003F800000000000003F8000003F800000
                3E0D3F8000003F8000003F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000040A59CB9
                3E194221800000000000 
             */

            var extractedInformation1 = PayloadConverter.ConvertFromActility("20011300132F0D010301000500D3FF000001550005010503", 3);
           
            var extractedInformation2 = PayloadConverter.ConvertFromActility("3401000000072691F271000000262691F4BC2538552026931E64000000000000025800000708000151800000000000000000", 3);
          
            var extractedInformation3 = PayloadConverter.ConvertFromActility("3E013F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF000000003F8000003F800000000000003F8000003F800000", 3);
            var extractedInformation4 = PayloadConverter.ConvertFromActility("3E0D3F8000003F8000003F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000040A59CB9", 3);
            var extractedInformation5 = PayloadConverter.ConvertFromActility("3E194221800000000000", 3);

            extractedInformation1.ValuesOf8Bit.Should().NotBeNull();
            extractedInformation2.ValuesOf32Bit.Should().NotBeNull();
            extractedInformation3.ValuesOfFloat.Should().NotBeNull();
            extractedInformation4.ValuesOfFloat.Should().NotBeNull();
            extractedInformation5.ValuesOfFloat.Should().NotBeNull();
        }


        [TestMethod()]
        public void WhenRequestedABigConfiguration_ThenValuesShouldBeCorrect()
        {

            //After Requesting:
            //5A080000 on port 5
            // 10 messages where replied on port 5
            /*
                20011300132F0D010301000500D3FF000001550005010503
                
                340100000007269590390000002626959284253855202695C164000000000000025800000708000151800000000000000000
                
                3E013F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF000000003F8000003F800000000000003F8000003F800000
                3E0D3F8000003F8000003F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000040A5D1F3
                3E19422016BC00000000
            
                480001312E302E3032FF30303944364230303030433544334144FF37304233443537454430303231373946FF
                4800043531323538374244424437433232373236463243343334383245394444383332FF3236303132434135FF
                4800063630463639464632444543333333333146383930384637443131363039433044FF
                4800073133443846454238313443453839383444363646363337353335394531303736FF

                51010105140C1C000CEB99
             */


            var xx = PayloadConverter.ConvertFromTheThingNetwork("AQZA0kBGFTZBSAMAP4I1QEHpcKRCKAAA", 1);

            var extractedInformation1 = PayloadConverter.ConvertFromActility("20011300132F0D010301000500D3FF000001550005010503", 5);
            var extractedInformation2 = PayloadConverter.ConvertFromActility("340100000007269590390000002626959284253855202695C164000000000000025800000708000151800000000000000000", 5);
            var extractedInformation3 = PayloadConverter.ConvertFromActility("3E013F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF000000003F8000003F800000000000003F8000003F800000", 5);
            var extractedInformation4 = PayloadConverter.ConvertFromActility("3E0D3F8000003F8000003F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000040A5D1F3", 5);
            var extractedInformation5 = PayloadConverter.ConvertFromActility("3E19422016BC00000000", 5);
            var extractedInformation6 = PayloadConverter.ConvertFromActility("480001312E302E3032FF30303944364230303030433544334144FF37304233443537454430303231373946FF", 5);
            var extractedInformation7 = PayloadConverter.ConvertFromActility("4800043531323538374244424437433232373236463243343334383245394444383332FF3236303132434135FF", 5);
            var extractedInformation8 = PayloadConverter.ConvertFromActility("4800063630463639464632444543333333333146383930384637443131363039433044FF", 5);
            var extractedInformation9 = PayloadConverter.ConvertFromActility("4800073133443846454238313443453839383444363646363337353335394531303736FF", 5);
            var extractedInformation10= PayloadConverter.ConvertFromActility("51010105140C1C000CEB99", 5);

            extractedInformation1.ValuesOf8Bit.Should().NotBeNull();
            extractedInformation2.ValuesOf32Bit.Should().NotBeNull();
            extractedInformation3.ValuesOfFloat.Should().NotBeNull();
            extractedInformation4.ValuesOfFloat.Should().NotBeNull();
            extractedInformation5.ValuesOfFloat.Should().NotBeNull();
        }
    }
}