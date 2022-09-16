using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace KellerAg.Shared.LoRaPayloadConverter
{
    /// <summary>
    /// <para>
    /// ConvertFrom... methods are to be used when converting downlink messages from the devices to a readable model
    ///   input: KELLER LoRa payload string
    ///   output: C# "Business"-Object
    /// </para>
    /// <para>
    /// ConvertTo... methods are to be used when converting uplink messages to the devices from a readable model
    ///   input: C# "Business"-Object
    ///   output: List of KELLER LoRa payload strings
    /// </para>
    /// </summary>
    public static partial class PayloadConverter
    {
        public static PayloadInformation ConvertFromTheThingNetwork(string payloadFromDevice, int port)
        {
            if (string.IsNullOrEmpty(payloadFromDevice))
            {
                return new PayloadInformation();
            }
            byte[] decodedPayload = System.Convert.FromBase64String(payloadFromDevice);
            return ExtractInfo(decodedPayload, port, OriginLoRaNetwork.TheThingsNetwork);
        }

        public static PayloadInformation ConvertFromActility(string payloadFromDevice, int port)
        {
            byte[] payloadInBytes = ConvertHexStringToByteArray(payloadFromDevice);
            return ExtractInfo(payloadInBytes, port, OriginLoRaNetwork.Actility);
        }

        public static PayloadInformation ConvertFromLoriot(string payloadFromDevice, int port)
        {
            byte[] payloadInBytes = ConvertHexStringToByteArray(payloadFromDevice);
            return ExtractInfo(payloadInBytes, port, OriginLoRaNetwork.Loriot);
        }


        public static List<string> ConvertToTheThingNetwork(PayloadInformation downstreamInfo)
        {
            List<string> payloads = GeneratePayloads(downstreamInfo);
            var payloadsAsBase64 = new List<string>(payloads.Count);
            payloadsAsBase64.AddRange(payloads.Select(payload => System.Convert.ToBase64String(ConvertHexStringToByteArray(payload))));
            return payloadsAsBase64;
        }

        public static List<string> ConvertToActility(PayloadInformation downstreamInfo)
        {
            List<string> payloads = GeneratePayloads(downstreamInfo);
            return payloads;
        }
        public static List<string> ConvertToLoriot(PayloadInformation downstreamInfo)
        {
            List<string> payloads = GeneratePayloads(downstreamInfo);
            return payloads;
        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            var hexAsBytes = new byte[hexString.Length / 2];

            for (var index = 0; index < hexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                hexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return hexAsBytes;
        }

        private static PayloadInformation ExtractInfo(byte[] payload, int port, OriginLoRaNetwork originalNetwork)
        {
            FunctionCodeId functionCode = (FunctionCodeId)payload[0];

            var payloadInfo = new PayloadInformation
            {
                DecodedPayload    = new byte[payload.Length],
                Port              = port,
                FunctionCode      = functionCode,
                IsSupportedDevice = true,
                OriginLoRaNetwork = originalNetwork,
                Measurements      = null,
                Info              = null,
                ValuesOf8Bit      = null,
                ValuesOf32Bit     = null,
                ValuesOfFloat     = null,
                ValueOfText       = null,
                SensorInfo        = null
            };
            Array.Copy(payload, payloadInfo.DecodedPayload, payload.Length);

            try
            {
                switch (functionCode)
                {
                    case FunctionCodeId.UnknownMessage:
                        payloadInfo.IsSupportedDevice = false;
                        break;
                    case FunctionCodeId.MeasurementMessage:
                        payloadInfo = ExtractMeasurementMessage(payloadInfo);
                        break;
                    case FunctionCodeId.InfoMessage:
                        payloadInfo = ExtractInfoMessage(payloadInfo);
                        break;
                    case FunctionCodeId.ValuesOf8BitMessage:
                        payloadInfo = ExtractValuesOf8BitMessage(payloadInfo);
                        break;
                    case FunctionCodeId.ValuesOf8BitStreamMessage:
                        payloadInfo = ExtractValuesOf8BitStreamMessage(payloadInfo);
                        break;
                    case FunctionCodeId.ValuesOf32BitMessage:
                        payloadInfo = ExtractValuesOf32BitMessage(payloadInfo);
                        break;
                    case FunctionCodeId.ValuesOf32BitStreamMessage:
                        payloadInfo = ExtractValuesOf32BitStreamMessage(payloadInfo);
                        break;
                    case FunctionCodeId.ValuesOfFloatMessage:
                        payloadInfo = ExtractValuesOfFloatMessage(payloadInfo);
                        break;
                    case FunctionCodeId.ValuesOfFloatStreamMessage:
                        payloadInfo = ExtractValuesOfFloatStreamMessage(payloadInfo);
                        break;
                    case FunctionCodeId.ValuesOfTextMessage:
                        payloadInfo = ExtractValuesOfTextMessage(payloadInfo);
                        break;
                    case FunctionCodeId.ValuesOfTextStreamMessage:
                        payloadInfo = ExtractValuesOfTextStreamMessage(payloadInfo);
                        break;
                    case FunctionCodeId.SensorInformationMessage:
                        payloadInfo = ExtractSensorInformationMessage(payloadInfo);
                        break;
                    case FunctionCodeId.CommandMessage:
                        payloadInfo = ExtractCommandMessage(payloadInfo);
                        break;
                    default:
                        payloadInfo.IsSupportedDevice = false;
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch
            {
                payloadInfo.IsSupportedDevice = false;
            }
            return payloadInfo;
        }

        private static List<string> GeneratePayloads(PayloadInformation downstreamInfo)
        {
            var payloads = new List<string>();
            switch (downstreamInfo.FunctionCode)
            {
                case FunctionCodeId.UnknownMessage:
                    break;
                case FunctionCodeId.MeasurementMessage:
                    payloads = GeneratePayloadForMeasurementMessage(downstreamInfo, payloads); //no really needed
                    break;
                case FunctionCodeId.InfoMessage:
                    payloads = GeneratePayloadForInfoMessage(downstreamInfo, payloads); //no really needed
                    break;
                case FunctionCodeId.ValuesOf8BitMessage:
                    payloads = GeneratePayloadForValuesOf8BitMessage(downstreamInfo, payloads);
                    break;
                case FunctionCodeId.ValuesOf8BitStreamMessage:
                    payloads = GeneratePayloadForValuesOf8BitStreamMessage(downstreamInfo, payloads);
                    break;
                case FunctionCodeId.ValuesOf32BitMessage:
                    payloads = GeneratePayloadForValuesOf32BitMessage(downstreamInfo, payloads);
                    break;
                case FunctionCodeId.ValuesOf32BitStreamMessage:
                    payloads = GeneratePayloadForValuesOf32BitStreamMessage(downstreamInfo, payloads);
                    break;
                case FunctionCodeId.ValuesOfFloatMessage:
                    payloads = GeneratePayloadForValuesOfFloatMessage(downstreamInfo, payloads);
                    break;
                case FunctionCodeId.ValuesOfFloatStreamMessage:
                    payloads = GeneratePayloadForValuesOfFloatStreamMessage(downstreamInfo, payloads);
                    break;
                case FunctionCodeId.ValuesOfTextMessage:
                    payloads = GeneratePayloadForValuesOfTextMessage(downstreamInfo, payloads);
                    break;
                case FunctionCodeId.ValuesOfTextStreamMessage:
                    payloads = GeneratePayloadForValuesOfTextStreamMessage(downstreamInfo, payloads);
                    break;
                case FunctionCodeId.SensorInformationMessage:
                    payloads = GeneratePayloadForSensorInformationMessage(downstreamInfo, payloads);
                    break;
                case FunctionCodeId.CommandMessage:
                    payloads = GeneratePayloadForCommandMessage(downstreamInfo, payloads);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return payloads;
        }
    }
}