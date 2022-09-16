using System;
using System.Collections.Generic;
using System.Linq;

namespace KellerAg.Shared.LoRaPayloadConverter
{
    public static partial class PayloadConverter
    {
        /// <summary>
        /// Not really needed as we do not send measurements to the device
        /// </summary>
        /// <param name="payloadInfo"></param>
        /// <param name="payloads"></param>
        /// <returns></returns>
        private static List<string> GeneratePayloadForMeasurementMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            if (payloadInfo.Measurements == null || payloadInfo.Measurements.Count == 0)
                return payloads;
            
            var connectionDeviceType = payloadInfo.Measurements.First().ConnectionDeviceType;
            int byteCount = payloadInfo.Measurements.Count * 4 + 4;
            byte[] payload = new byte[byteCount];
            payload[0] = 0x01;
            payload[1] = (byte) connectionDeviceType;

            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText);
            return payloads;
        }

        /// <summary>
        /// Not really needed as we do not send information to the device
        /// </summary>
        /// <param name="payloadInfo"></param>
        /// <param name="payloads"></param>
        /// <returns></returns>
        private static List<string> GeneratePayloadForInfoMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            return payloads;
        }

        private static List<string> GeneratePayloadForValuesOf8BitMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            if (payloadInfo.ValuesOf8Bit == null || payloadInfo.ValuesOf8Bit.Count == 0)
                return payloads;

            int byteCount = (payloadInfo.ValuesOf8Bit.Count * 2) + 1;
            byte[] payload = new byte[byteCount];

            payload[0] = 0x1F;
            for (int i = 0; i < payloadInfo.ValuesOf8Bit.Count; i++)
            {
                var item = payloadInfo.ValuesOf8Bit.ElementAt(i);
                payload[(2 * i) + 1] = (byte)item.Key;
                payload[(2 * i) + 2] = item.Value;
            }

            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText.ToUpper());
            return payloads;
        }

        private static List<string> GeneratePayloadForValuesOf8BitStreamMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            if (payloadInfo.ValuesOf8Bit == null || payloadInfo.ValuesOf8Bit.Count == 0)
                return payloads;

            int byteCount = payloadInfo.ValuesOf8Bit.Count + 2;
            byte[] payload = new byte[byteCount];

            byte lowestIndexMustBeStartIndex = (byte)payloadInfo.ValuesOf8Bit.Keys.Min();
            payload[0] = 0x20;
            payload[1] = lowestIndexMustBeStartIndex;
            for (var i = 0; i < payloadInfo.ValuesOf8Bit.Count; i++)
            {
                var value = payloadInfo.ValuesOf8Bit[(IndexOfValuesOf8Bit) (i + lowestIndexMustBeStartIndex)];
                payload[i + 2] = value;
            }

            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText.ToUpper());
            return payloads;
        }

        private static List<string> GeneratePayloadForValuesOf32BitMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            if (payloadInfo.ValuesOf32Bit == null || payloadInfo.ValuesOf32Bit.Count == 0)
                return payloads;

            int byteCount = (payloadInfo.ValuesOf32Bit.Count * 5) + 1;
            byte[] payload = new byte[byteCount];

            payload[0] = 0x33;
            for (int i = 0; i < payloadInfo.ValuesOf32Bit.Count; i++)
            {
                var item = payloadInfo.ValuesOf32Bit.ElementAt(i);
                payload[(5 * i) + 1] = (byte)item.Key;
                byte[] splitValue = GetBytes(item.Value);
                payload[(5 * i) + 2] = splitValue[0];
                payload[(5 * i) + 3] = splitValue[1];
                payload[(5 * i) + 4] = splitValue[2];
                payload[(5 * i) + 5] = splitValue[3];
            }

            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText.ToUpper());
            return payloads;
        }

        private static List<string> GeneratePayloadForValuesOf32BitStreamMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            if (payloadInfo.ValuesOf32Bit == null || payloadInfo.ValuesOf32Bit.Count == 0)
                return payloads;

            int byteCount = (payloadInfo.ValuesOf32Bit.Count * 4) + 2;
            byte[] payload = new byte[byteCount];
            byte lowestIndexMustBeStartIndex = (byte)payloadInfo.ValuesOf32Bit.Keys.Min();
            payload[0] = 0x34;
            payload[1] = lowestIndexMustBeStartIndex;

            for (var i = 0; i < payloadInfo.ValuesOf32Bit.Count; i++)
            {
                var value = payloadInfo.ValuesOf32Bit[(IndexOfValuesOf32Bit)(i + lowestIndexMustBeStartIndex)];
                byte[] splitValue = GetBytes(value);
                payload[(4 * i) + 2] = splitValue[0];
                payload[(4 * i) + 3] = splitValue[1];
                payload[(4 * i) + 4] = splitValue[2];
                payload[(4 * i) + 5] = splitValue[3];
            }

            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText.ToUpper());
            return payloads;
        }

        private static List<string> GeneratePayloadForValuesOfFloatMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            if (payloadInfo.ValuesOfFloat == null || payloadInfo.ValuesOfFloat.Count == 0)
                return payloads;

            int byteCount = (payloadInfo.ValuesOfFloat.Count * 5) + 1;
            byte[] payload = new byte[byteCount];

            payload[0] = 0x3D;
            for (int i = 0; i < payloadInfo.ValuesOfFloat.Count; i++)
            {
                var item = payloadInfo.ValuesOfFloat.ElementAt(i);
                payload[(5 * i) + 1] = (byte)item.Key;
                byte[] splitValue = GetBytes(item.Value);
                payload[(5 * i) + 2] = splitValue[0];
                payload[(5 * i) + 3] = splitValue[1];
                payload[(5 * i) + 4] = splitValue[2];
                payload[(5 * i) + 5] = splitValue[3];
            }

            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText.ToUpper());
            return payloads;
        }

        private static List<string> GeneratePayloadForValuesOfFloatStreamMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            if (payloadInfo.ValuesOfFloat == null || payloadInfo.ValuesOfFloat.Count == 0)
                return payloads;

            int byteCount = (payloadInfo.ValuesOfFloat.Count * 4) + 2;
            byte[] payload = new byte[byteCount];
            byte lowestIndexMustBeStartIndex = (byte)payloadInfo.ValuesOfFloat.Keys.Min();
            payload[0] = 0x3E;
            payload[1] = lowestIndexMustBeStartIndex;

            for (var i = 0; i < payloadInfo.ValuesOfFloat.Count; i++)
            {
                var value = payloadInfo.ValuesOfFloat[(IndexOfValuesOfFloat)(i + lowestIndexMustBeStartIndex)];
                byte[] splitValue = GetBytes(value);
                payload[(4 * i) + 2] = splitValue[0];
                payload[(4 * i) + 3] = splitValue[1];
                payload[(4 * i) + 4] = splitValue[2];
                payload[(4 * i) + 5] = splitValue[3];
            }

            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText.ToUpper());
            return payloads;
        }

        private static List<string> GeneratePayloadForValuesOfTextMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            if (payloadInfo.ValueOfText == null || payloadInfo.ValueOfText.Count == 0)
                return payloads;

            int byteCount = 1;
            foreach (var valuesOfText in payloadInfo.ValueOfText)
            {
                if (valuesOfText.Value.HasReachedEndOfText)
                {
                    byteCount += 3;
                }
                else
                {
                    byteCount += 2;
                }
                byteCount += valuesOfText.Value.Text.Length;
            }

            byte[] payload = new byte[byteCount];
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            var index = 0;
            payload[index] = 0x47;
            foreach (var valuesOfText in payloadInfo.ValueOfText)
            {
                payload[++index] = (byte)valuesOfText.Value.CharacterStartingPosition;
                payload[++index] = (byte)valuesOfText.Key;

                var chars = enc.GetBytes(valuesOfText.Value.Text);
                foreach (byte character in chars)
                {
                    payload[++index] = character;
                }

                if (valuesOfText.Value.HasReachedEndOfText)
                {
                    payload[++index] = 0xFF;
                }
            }

            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText.ToUpper());
            return payloads;
        }

        private static List<string> GeneratePayloadForValuesOfTextStreamMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            if (payloadInfo.ValueOfText == null || payloadInfo.ValueOfText.Count == 0)
                return payloads;

            int byteCount = 3; //first three bytes
            foreach (var valuesOfText in payloadInfo.ValueOfText)
            {
                if (valuesOfText.Value.HasReachedEndOfText)
                {
                    byteCount++; //for 0xFF
                }

                byteCount += valuesOfText.Value.Text.Length;
            }

            byte[] payload = new byte[byteCount];
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            payload[0] = 0x48;
            byte lowestIndexMustBeStartIndex = (byte)payloadInfo.ValueOfText.Keys.Min();
            payload[1] = (byte)payloadInfo.ValueOfText[(IndexOfTextValues)lowestIndexMustBeStartIndex].CharacterStartingPosition; //CharacterStartingPosition
            payload[2] = lowestIndexMustBeStartIndex; //Starting Index

            var index = 2;
            foreach (var valuesOfText in payloadInfo.ValueOfText)
            {
                var chars = enc.GetBytes(valuesOfText.Value.Text);
                foreach (byte character in chars)
                {
                    payload[++index] = character;
                }

                if (valuesOfText.Value.HasReachedEndOfText)
                {
                    payload[++index] = 0xFF;
                }
            }

            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText.ToUpper());
            return payloads;
        }

        /// <summary>
        /// Not really needed as we do not send information to the device
        /// </summary>
        /// <param name="payloadInfo"></param>
        /// <param name="payloads"></param>
        /// <returns></returns>
        private static List<string> GeneratePayloadForSensorInformationMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            if (payloadInfo.SensorInfo == null)
                return payloads;

            int byteCount = 0;
            byte[] payload;
            switch (payloadInfo.SensorInfo.SensorType)
            {
                case SensorType.RS485:
                    byteCount = 3 + 8;
                    payload = new byte[byteCount];
                    payload[0] = 0x51;
                    payload[1] = (byte)payloadInfo.SensorInfo.SensorCount;
                    payload[2] = (byte)SensorType.RS485;
                    payload[3] = payloadInfo.SensorInfo.SensorType1Data.SensorClass;
                    payload[4] = payloadInfo.SensorInfo.SensorType1Data.SensorGroup;
                    payload[5] = payloadInfo.SensorInfo.SensorType1Data.SwVersionYear;
                    payload[6] = payloadInfo.SensorInfo.SensorType1Data.SwVersionWeek;
                    var serialNumber = GetBytes(payloadInfo.SensorInfo.SensorType1Data.SerialNumber);
                    payload[7]  = serialNumber[0];
                    payload[8]  = serialNumber[1];
                    payload[9]  = serialNumber[2];
                    payload[10] = serialNumber[3];
                    break;
                case SensorType.InterIntegratedCircuit:
                    byteCount = 3 + 14;
                    payload = new byte[byteCount];
                    payload[0] = 0x51;
                    payload[1] = (byte)payloadInfo.SensorInfo.SensorCount;
                    payload[2] = (byte)SensorType.InterIntegratedCircuit;

                    var uniqueId = GetBytes(payloadInfo.SensorInfo.SensorType2Data.UniqueId);
                    var scaling0 = GetBytes(payloadInfo.SensorInfo.SensorType2Data.Scaling0);
                    var pMinVal  = GetBytes(payloadInfo.SensorInfo.SensorType2Data.PMinVal);
                    var pMaxVal  = GetBytes(payloadInfo.SensorInfo.SensorType2Data.PMaxVal);

                    payload[3]  = uniqueId[0];
                    payload[4]  = uniqueId[1];
                    payload[5]  = uniqueId[2];
                    payload[6]  = uniqueId[3];
                    payload[7]  = scaling0[0];
                    payload[8]  = scaling0[1];
                    payload[9]  = pMinVal[0];
                    payload[10] = pMinVal[1];
                    payload[11] = pMinVal[2];
                    payload[12] = pMinVal[3];
                    payload[13] = pMaxVal[0];
                    payload[14] = pMaxVal[1];
                    payload[15] = pMaxVal[2];
                    payload[16] = pMaxVal[3];
                    break;
                default:
                    return payloads;
            }

            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText.ToUpper());
            return payloads;
        }

        private static List<string> GeneratePayloadForCommandMessage(PayloadInformation payloadInfo, List<string> payloads)
        {
            byte[] payload =
            {
                0x5A,
                (byte) (payloadInfo.Command.CommandFunction),
                payloadInfo.Command.StartIndex,
                payloadInfo.Command.EndIndex
            };
            string payloadText = ByteArrayToHexString(payload);
            payloads.Add(payloadText.ToUpper());
            return payloads;
        }


        private static byte[] GetBytes(bool value)
        {
            return ReverseAsNeeded(BitConverter.GetBytes(value));
        }

        private static byte[] GetBytes(char value)
        {
            return ReverseAsNeeded(BitConverter.GetBytes(value));
        }
        private static byte[] GetBytes(double value)
        {
            return ReverseAsNeeded(BitConverter.GetBytes(value));
        }
        private static byte[] GetBytes(float value)
        {
            return ReverseAsNeeded(BitConverter.GetBytes(value));
        }
        private static byte[] GetBytes(int value)
        {
            return ReverseAsNeeded(BitConverter.GetBytes(value));
        }
        private static byte[] GetBytes(long value)
        {
            return ReverseAsNeeded(BitConverter.GetBytes(value));
        }
        private static byte[] GetBytes(short value)
        {
            return ReverseAsNeeded(BitConverter.GetBytes(value));
        }
        private static byte[] GetBytes(uint value)
        {
            var bytes = ReverseAsNeeded(BitConverter.GetBytes(value));
            return bytes;
        }
        private static byte[] GetBytes(ulong value)
        {
            return ReverseAsNeeded(BitConverter.GetBytes(value));
        }
        private static byte[] GetBytes(ushort value)
        {
            return ReverseAsNeeded(BitConverter.GetBytes(value));
        }

        private static byte[] ReverseAsNeeded(byte[] bytes)
        {
            return BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes;
        }
    }
}