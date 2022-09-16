using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using KellerAg.Shared.LoRaPayloadConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable All

namespace KellerAg.Shared.LoRaPayloadConverter.Tests
{
    [TestClass()]
    public class BusinessObjectToPayloadConverterTests
    {
        [TestMethod()]
        public void WhenGivenACommand_ThenPayloadShouldBeCorrect()
        {
            var payloadInfo  = PayloadConverter.ConvertFromActility("5A0A0000", 3);
            payloadInfo.Command.CommandFunction.Should().Be(CommandFunction.RequestSensorInformation_FunctionCode81);
            List<string> payloads = PayloadConverter.ConvertToActility(payloadInfo);
            payloads.First().Should().Be("5A0A0000");
        }

        //It is not necessary to convert measurement values to payload as this is no real use case
        ///// <summary>
        ///// Protocol page 5 - Example with 0x01
        ///// </summary>
        //[TestMethod()]
        //public void WhenGivenAPayloadWithMeasurements_ThenValuesShouldBeCorrect()
        //{
        //    PayloadInformation payloadInfo = PayloadConverter.ConvertFromActility("010300D3BF7595F03C5C91304249C7C03F79081C42477AE1", 1);

        //    List<string> payloads = PayloadConverter.ConvertToActility(payloadInfo);
        //    payloads.First().Should().Be("010300D3BF7595F03C5C91304249C7C03F79081C42477AE1");
        //}

        // It is not necessary to convert measurement values to payload as this is no real use case
        ///// <summary>
        ///// Protocol - Page 7 - Information Message example
        ///// </summary>
        //[TestMethod()]
        //public void WhenGivenAPayloadWithInformationMessage_ThenValuesShouldBeCorrect()
        //{
        //    var payloadText = "0C011300132F000000642576AFAA4098B368631D";

        //    PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

        //    List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
        //    payloads.First().Should().Be(payloadText);
        //}

        /// <summary>
        /// Protocol - Page 8 - 1 Byte Message example
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWith8BitMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "1F011302000313042F0DFF0E000F00100115051603";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }


        /// <summary>
        /// Protocol - Page 8 - 1 Byte Stream Message example
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWith8BitStreamMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "20011300132F0D010301000500D3FF000001630105010503";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
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

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }


        /// <summary>
        /// Protocol - Page 8 - 4 Byte Stream Message example
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWith32BitStreamMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "340100000064257660380000002525766B51254AD8302577845F0000000000000E1000015180000151800000000000000000";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }

        /// <summary>
        /// Protocol - Page 12 - Float Message example without stream
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWithFloatMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "3D1941DCF695183F9E0610";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }


        /// <summary>
        /// Protocol - Page 12 - Float Stream Message example - Payload #109
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayload109WithFloatStreamMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "3E013F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF3F8000003F8000003F8000000000000044798CCD3F800000";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }

        [TestMethod()]
        public void WhenGivenAValidTTNPayload_ThenValuesShouldBeCorrect()
        {
            var payloadText = "3E013F8000003F8000003F800000FFFFFFFFFFFFFFFFFFFFFFFF3F8000003F8000003F8000000000000044798CCD3F800000";
            var payloadTextBase64 = System.Convert.ToBase64String(PayloadConverter.ConvertHexStringToByteArray(payloadText));
            //payloadTextBase64 == PgE/gAAAP4AAAD+AAAD///////////////8/gAAAP4AAAD+AAAAAAAAARHmMzT+AAAA=
            var extractedInformation = PayloadConverter.ConvertFromTheThingNetwork(payloadTextBase64, (int)FunctionCodeId.MeasurementMessage);
            List<string> payloads = PayloadConverter.ConvertToTheThingNetwork(extractedInformation);
            payloads.First().Should().Be(payloadTextBase64);
        }

        /// <summary>
        /// Protocol - Page 12 - Float Stream Message example - Payload 111
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayload111WithFloatStreamMessage_ThenValuesShouldBeCorrect()
        {
            var payloadText = "3E1941DCF69500000000";

            PayloadInformation extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 4);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }

        /// <summary>
        /// Protocol Page 14 - Payload example 121
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWithASCIIChars_ThenValuesShouldBeCorrect()
        {
            var payloadText = "480001312E302E3032FF30303944364230303030433544333832FF37304233443537454430303234364134FF";
            var extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 5);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }

        /// <summary>
        /// Protocol Page 14 - Payload example 122
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayload122WithASCIIChars_ThenValuesShouldBeCorrect()
        {
            var payloadText = "4800043331433439344337444644383746303333353546444530413731453842353732FF3236303132314336FF";
            var extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 5);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }

        /// <summary>
        /// Protocol Page 14 - Payload example 123
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayload123WithASCIIChars_ThenValuesShouldBeCorrect()
        {
            var payloadText = "4800063246413830324332433944354646414146353544453535383430344536363230FF";
            var extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 5);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }

        /// <summary>
        /// Protocol Page 14 - Payload example 124
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayload124WithASCIIChars_ThenValuesShouldBeCorrect()
        {
            var payloadText = "4800073046363932464531434544463737424632383837333944393645444535344137FF";
            var extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 5);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
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

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadPart1);
            //payload part 2
            extractedInformation = PayloadConverter.ConvertFromActility(payloadPart2, 5);

            payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadPart2);
        }

        /// <summary>
        /// TextValues that are not a stream
        /// </summary>
        [TestMethod()]
        public void WhenGivenANonStreamPayloadWithASCIIChars_ThenValuesShouldBeCorrect()
        {
            var payloadPart = "47000337304233443537454430303234364134FF00043331433439344337444644383746303333353546444530413731453842353732FF";
            var extractedInformation = PayloadConverter.ConvertFromActility(payloadPart, 5);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadPart);
        }

        /// <summary>
        /// Protocol Page 16 - Payload example
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWithKellerSensorInformation_ThenValuesShouldBeCorrect()
        {
            var payloadText = "51010105140C1C000CEBA1";
            var extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 5);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }

        /// <summary>
        /// Protocol Page 16 - Payload example with I2C
        /// </summary>
        [TestMethod()]
        public void WhenGivenAPayloadWithKellerSensorInformationAndI2C_ThenValuesShouldBeCorrect()
        {
            var payloadText = "5101020407014415740000000041F00000";
            var extractedInformation = PayloadConverter.ConvertFromActility(payloadText, 5);

            List<string> payloads = PayloadConverter.ConvertToActility(extractedInformation);
            payloads.First().Should().Be(payloadText);
        }
    }
}