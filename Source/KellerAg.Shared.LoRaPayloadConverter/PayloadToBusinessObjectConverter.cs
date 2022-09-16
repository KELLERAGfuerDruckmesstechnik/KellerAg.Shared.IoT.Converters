using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KellerAg.Shared.LoRaPayloadConverter
{
    public static partial class PayloadConverter
    {
        private static PayloadInformation ExtractMeasurementMessage(PayloadInformation payloadInfo)
        {
            int deviceConnectionType = payloadInfo.DecodedPayload[1];
            byte channel_H = payloadInfo.DecodedPayload[2];
            byte channel_L = payloadInfo.DecodedPayload[3];

            int measurementsCount = (payloadInfo.DecodedPayload.Length - 4) / 4; //Limited through casting: A misaligned size is cut to the minimum working size.

            var extractedMeasurement = new float[measurementsCount];

            try
            {
                for (var i = 0; i < measurementsCount; i++)
                {
                    extractedMeasurement[i] = ExtractFloat(payloadInfo.DecodedPayload, 4 + (i * 4));
                }

                int[] usedChannels = ExtractChannels(deviceConnectionType, channel_H, channel_L);
                List<ConnectionTypeChannelLookup> connectionTypeChannelLookup = ConnectionTypeChannelLookup.GetChannelInfo();

                payloadInfo.Measurements = new List<Measurement>(measurementsCount);
                for (var i = 0; i < extractedMeasurement.Length; i++)
                {
                    float value = extractedMeasurement[i];
                    payloadInfo.Measurements.Add(new Measurement
                    {
                        ConnectionDeviceType = deviceConnectionType,
                        ChannelNumber = usedChannels[i],
                        MeasurementDefinitionId = connectionTypeChannelLookup
                            .Single(_ =>
                                _.ChannelNumber == usedChannels[i] && _.ConnectionDeviceTypeId == deviceConnectionType)
                            .MeasurementDefinitionId,
                        Value = value
                    });
                }
            }
            catch
            {
                payloadInfo.IsSupportedDevice = false;
            }


            return payloadInfo;
        }

        private static PayloadInformation ExtractInfoMessage(PayloadInformation payloadInfo)
        {
            if (payloadInfo.DecodedPayload.Length != 20)
            {
                payloadInfo.IsSupportedDevice = false;
                return payloadInfo;
            }

            payloadInfo.Info = new InfoMessage
            {
                Type                 = 1, //unused
                BatteryVoltage       = ExtractBatteryVoltage(payloadInfo.DecodedPayload),
                BatteryCapacity      = ExtractBatteryCapacity(payloadInfo.DecodedPayload),
                HumidityPercentage   = ExtractHumidityPercentage(payloadInfo.DecodedPayload),
                DeviceClassGroupText = ExtractDeviceClassGroupText(payloadInfo.DecodedPayload),
                SwVersionText        = ExtractSwVersionText(payloadInfo.DecodedPayload),
                SerialNumber         = ExtractSerialNumber(payloadInfo.DecodedPayload),
                DeviceLocalDateTime  = ExtractDeviceLocalDateTime(payloadInfo.DecodedPayload)
            };
            return payloadInfo;
        }

        private static PayloadInformation ExtractValuesOf8BitMessage(PayloadInformation payloadInfo)
        {
            payloadInfo.ValuesOf8Bit = new Dictionary<IndexOfValuesOf8Bit, byte>();

            int valuesCount = (payloadInfo.DecodedPayload.Length - 1) / 2;
            for (int i = 0; i < valuesCount; i++)
            {
                byte index = payloadInfo.DecodedPayload[(2 * i) + 1];
                byte value = payloadInfo.DecodedPayload[(2 * i) + 2];
                 
                payloadInfo.ValuesOf8Bit.Add((IndexOfValuesOf8Bit) index, value);
            }

            return payloadInfo;
        }

        private static PayloadInformation ExtractValuesOf8BitStreamMessage(PayloadInformation payloadInfo)
        {
            payloadInfo.ValuesOf8Bit = new Dictionary<IndexOfValuesOf8Bit, byte>();

            int valuesCount = payloadInfo.DecodedPayload.Length - 2;
            byte startingIndex = payloadInfo.DecodedPayload[1];

            for (int i = 0; i < valuesCount; i++)
            {
                byte index = (byte) (startingIndex + i);
                byte value = payloadInfo.DecodedPayload[i + 2];

                payloadInfo.ValuesOf8Bit.Add((IndexOfValuesOf8Bit) index, value);
            }

            return payloadInfo;
        }

        private static PayloadInformation ExtractValuesOf32BitMessage(PayloadInformation payloadInfo)
        {
            payloadInfo.ValuesOf32Bit = new Dictionary<IndexOfValuesOf32Bit, uint>();

            int valuesCount = (payloadInfo.DecodedPayload.Length - 1) / 5;
            for (int i = 0; i < valuesCount; i++)
            {
                byte index = payloadInfo.DecodedPayload[(5*i) + 1];
                var value = ExtractUInt32(payloadInfo.DecodedPayload, (5*i) + 2);

                payloadInfo.ValuesOf32Bit.Add((IndexOfValuesOf32Bit) index, value);
            }

            return payloadInfo;
        }

        private static PayloadInformation ExtractValuesOf32BitStreamMessage(PayloadInformation payloadInfo)
        {
            payloadInfo.ValuesOf32Bit = new Dictionary<IndexOfValuesOf32Bit, uint>();

            int valuesCount = (payloadInfo.DecodedPayload.Length - 2) / 4;
            byte startingIndex = payloadInfo.DecodedPayload[1];

            for (int i = 0; i < valuesCount; i++)
            {
                byte index = (byte) (startingIndex + i);
                var value = ExtractUInt32(payloadInfo.DecodedPayload, (4*i) + 2);

                payloadInfo.ValuesOf32Bit.Add((IndexOfValuesOf32Bit) index, value);
            }

            return payloadInfo;
        }

        private static PayloadInformation ExtractValuesOfFloatMessage(PayloadInformation payloadInfo)
        {
            payloadInfo.ValuesOfFloat = new Dictionary<IndexOfValuesOfFloat, float>();

            int valuesCount = (payloadInfo.DecodedPayload.Length - 1) / 5;
            for (int i = 0; i < valuesCount; i++)
            {
                byte index = payloadInfo.DecodedPayload[(5*i) + 1];
                var value = ExtractFloat(payloadInfo.DecodedPayload, (5*i) + 2);

                payloadInfo.ValuesOfFloat.Add((IndexOfValuesOfFloat) index, value);
            }

            return payloadInfo;
        }

        private static PayloadInformation ExtractValuesOfFloatStreamMessage(PayloadInformation payloadInfo)
        {
            payloadInfo.ValuesOfFloat = new Dictionary<IndexOfValuesOfFloat, float>();

            int valuesCount = (payloadInfo.DecodedPayload.Length - 2) / 4;
            byte startingIndex = payloadInfo.DecodedPayload[1];

            for (int i = 0; i < valuesCount; i++)
            {
                byte index = (byte) (startingIndex + i);
                var value = ExtractFloat(payloadInfo.DecodedPayload, (4 * i) + 2);

                payloadInfo.ValuesOfFloat.Add((IndexOfValuesOfFloat) index, value);
            }

            return payloadInfo;
        }

        private static PayloadInformation ExtractValuesOfTextMessage(PayloadInformation payloadInfo)
        {
            payloadInfo.ValueOfText = new Dictionary<IndexOfTextValues, ValuesOfText>();

            for (var i = 1; i < payloadInfo.DecodedPayload.Length; )
            {
                byte characterStartPosition = payloadInfo.DecodedPayload[i];
                i++;
                byte index = payloadInfo.DecodedPayload[i];
                i++;
                bool hasReachedEndOfWord = false;
                var textAsBytes = new List<byte>();
                while (!hasReachedEndOfWord && i < payloadInfo.DecodedPayload.Length)
                {
                    byte value = payloadInfo.DecodedPayload[i];
                    if (value.Equals(0xFF))
                    {
                        hasReachedEndOfWord = true;
                    }
                    else
                    {
                        textAsBytes.Add(value);
                    }

                    i++;
                }

                payloadInfo.ValueOfText.Add(
                    (IndexOfTextValues) index,
                    new ValuesOfText
                    {
                        HasReachedEndOfText = hasReachedEndOfWord,
                        CharacterStartingPosition = characterStartPosition,
                        Text = ByteArrayToString(textAsBytes.ToArray())
                    });
            }

            return payloadInfo;
        }

        private static PayloadInformation ExtractValuesOfTextStreamMessage(PayloadInformation payloadInfo)
        {
            payloadInfo.ValueOfText = new Dictionary<IndexOfTextValues, ValuesOfText>();

            if (payloadInfo.DecodedPayload.Length < 5) return payloadInfo;

            byte characterStartPosition = payloadInfo.DecodedPayload[1];
            byte index = payloadInfo.DecodedPayload[2];

            for (var i = 3; i < payloadInfo.DecodedPayload.Length; i++)
            {
                bool hasReachedEndOfWord = false;
                var textAsBytes = new List<byte>();
                while (!hasReachedEndOfWord && i < payloadInfo.DecodedPayload.Length)
                {
                    byte value = payloadInfo.DecodedPayload[i];
                    if (value.Equals(0xFF))
                    {
                        hasReachedEndOfWord = true;
                    }
                    else
                    {
                        textAsBytes.Add(value);
                        i++;
                    }
                }

                payloadInfo.ValueOfText.Add(
                    (IndexOfTextValues) index,
                    new ValuesOfText
                    {
                        HasReachedEndOfText = hasReachedEndOfWord,
                        CharacterStartingPosition = characterStartPosition,
                        Text = ByteArrayToString(textAsBytes.ToArray())
                    });
                index++;
            }

            return payloadInfo;
        }

        private static PayloadInformation ExtractSensorInformationMessage(PayloadInformation payloadInfo)
        {
            payloadInfo.SensorInfo = new KellerSensorInfo
            {
                SensorCount = payloadInfo.DecodedPayload[1],
                SensorType = SensorType.Unknown,
                SensorType1Data = null,
                SensorType2Data = null
            };

            switch (payloadInfo.DecodedPayload[2])
            {
                case 0:
                    payloadInfo.SensorInfo.SensorType = SensorType.Unknown;
                    break;
                case 1:
                    payloadInfo.SensorInfo.SensorType = SensorType.RS485;
                    break;
                case 2:
                    payloadInfo.SensorInfo.SensorType = SensorType.InterIntegratedCircuit;
                    break;
            }

            if (payloadInfo.SensorInfo.SensorType == SensorType.RS485)
            {
                if (payloadInfo.DecodedPayload.Length < 8) return payloadInfo;

                payloadInfo.SensorInfo.SensorType1Data = new SensorType1
                {
                    SensorClass = payloadInfo.DecodedPayload[3],
                    SensorGroup = payloadInfo.DecodedPayload[4],
                    SwVersionYear = payloadInfo.DecodedPayload[5],
                    SwVersionWeek = payloadInfo.DecodedPayload[6]
                };

                if (payloadInfo.DecodedPayload.Length < 11) return payloadInfo;
                payloadInfo.SensorInfo.SensorType1Data.SerialNumber = ExtractUInt32(payloadInfo.DecodedPayload, 7);
            }

            if (payloadInfo.SensorInfo.SensorType == SensorType.InterIntegratedCircuit)
            {
                if (payloadInfo.DecodedPayload.Length < 7) return payloadInfo;

                payloadInfo.SensorInfo.SensorType2Data = new SensorType2
                {
                    UniqueId = ExtractUInt32(payloadInfo.DecodedPayload, 3),
                    Scaling0 = ExtractUInt16(payloadInfo.DecodedPayload, 7),
                    PMinVal  = ExtractFloat(payloadInfo.DecodedPayload, 9),
                    PMaxVal  = ExtractFloat(payloadInfo.DecodedPayload, 13),
                };
            }

            return payloadInfo;
        }

        /// <summary>
        /// <para>
        /// This is part of a transmission that goes down to the device and triggers an answer transmission.
        /// An extraction doesn't really make much sense. Nevertheless, it is added for compability reasons.
        /// </para>
        /// <para>
        /// It is possible to request multiple indexes at once using the start index (para1) and the end index (para2)
        /// The end index shall not be smaller than the start index.
        /// </para>
        /// </summary>
        /// <param name="payloadInfo"></param>
        /// <returns></returns>
        private static PayloadInformation ExtractCommandMessage(PayloadInformation payloadInfo)
        {
            byte index = payloadInfo.DecodedPayload[1];
            byte para1 = payloadInfo.DecodedPayload[2]; // Start index of the requested command
            byte para2 = payloadInfo.DecodedPayload[3]; // End index of the requested command

            payloadInfo.Command = new CommandMessage
            {
                CommandFunction = (CommandFunction) index,
                StartIndex = para1,
                EndIndex = para2
            };

            return payloadInfo;
        }

        private static float? ExtractBatteryVoltage(byte[] payload)
        {
            try
            {
                float batteryVoltage = ExtractFloat(payload, 14);
                return batteryVoltage;
            }
            catch
            {
                return null;
            }
        }

        private static byte? ExtractBatteryCapacity(byte[] payload)
        {
            try
            {
                var batteryCapacity = (byte) Convert.ToSByte(payload[18]);
                return batteryCapacity;
            }
            catch
            {
                return null;
            }
        }

        private static byte? ExtractHumidityPercentage(byte[] payload)
        {
            try
            {
                var humidityPercentage = (byte) Convert.ToSByte(payload[19]);
                return humidityPercentage;
            }
            catch
            {
                return null;
            }
        }

        private static string ExtractDeviceClassGroupText(byte[] payload)
        {
            int deviceClass = Convert.ToInt32(payload[2]);
            int deviceGroup = Convert.ToInt32(payload[3]);
            string classGroupText = $"{deviceClass:00}.{deviceGroup:00}";
            return classGroupText;
        }

        private static string ExtractSwVersionText(byte[] payload)
        {
            int swYear = Convert.ToInt32(payload[4]);
            int swWeak = Convert.ToInt32(payload[5]);
            string swVersionText = $"{swYear:00}.{swWeak:00}";
            return swVersionText;
        }

        private static int ExtractSerialNumber(byte[] payload)
        {
            byte[] payloadPart = new byte[4];
            Array.Copy(payload, 6, payloadPart, 0, payloadPart.Length);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(payloadPart); //need the bytes in the reverse order
            }

            int serialNumber = BitConverter.ToInt32(payloadPart, 0);
            return serialNumber;
        }

        private static DateTime ExtractDeviceLocalDateTime(byte[] payload)
        {
            byte[] payloadPart = new byte[4];
            Array.Copy(payload, 10, payloadPart, 0, payloadPart.Length);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(payloadPart); //need the bytes in the reverse order
            }

            int secondsFromYear2000 = BitConverter.ToInt32(payloadPart, 0);

            var deviceLocalDateTime = new DateTime(2000, 1, 1, 0, 0, 0);
            deviceLocalDateTime += new TimeSpan(0, 0, secondsFromYear2000);
            return deviceLocalDateTime;
        }

        /// <summary>
        /// Extracts the list of used channels in a certain order.
        /// Be aware that these are not GetMeasurementDefinitionIdLookupByChannelNumberForDeviceTypeIdAsync
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="channelH"></param>
        /// <param name="channelL"></param>
        /// <returns>int[] Array of Channel numbers</returns>
        private static int[] ExtractChannels(int connectionType, byte channelH, byte channelL)
        {
            var channelsBitData = new BitArray(new[] {channelL, channelH});
            var channels = new List<int>();

            for (var ix = 0; ix < channelsBitData.Length; ix++)
            {
                if (channelsBitData[ix])
                {
                    channels.Add(ix + 1); //"Gsm2 & ARC Daten Kommunikations_v1.6_en.pdf": Do the channels start with 0 or with 1??
                }
            }

            return channels.ToArray();
        }

        /// <summary>
        /// Use this when you want to have a mapping of channel-number to measurementDefinitionId
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="channelH"></param>
        /// <param name="channelL"></param>
        /// <returns>int[] Array of MeasurementDefinitionIds</returns>
        private static int[] ExtractConnectionTypeAndLookUp(int connectionType, byte channelH, byte channelL)
        {
            var mappedChannels = ConnectionTypeChannelLookup.GetChannelInfo();

            var channelsBitData = new BitArray(new[] {channelL, channelH});

            var measurementDefinitions = new List<int>();

            for (var ix = 0; ix < channelsBitData.Length; ix++)
            {
                if (channelsBitData[ix])
                {
                    int md = mappedChannels
                        .Where(chn => chn.ConnectionDeviceTypeId == connectionType && chn.ChannelNumber == ix + 1)
                        .Select(chn => chn.MeasurementDefinitionId).Single();
                    measurementDefinitions.Add(md);
                }
            }

            return measurementDefinitions.ToArray();
        }

        private static float ExtractFloat(byte[] payLoadBytes, int pos)
        {
            var bytes = new byte[4];
            Array.Copy(payLoadBytes, pos, bytes, 0, bytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            return BitConverter.ToSingle(bytes, 0);
        }

        private static ushort ExtractUInt16(byte[] payLoadBytes, int pos)
        {
            var bytes = new byte[2];
            Array.Copy(payLoadBytes, pos, bytes, 0, bytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            return BitConverter.ToUInt16(bytes, 0);
        }

        private static UInt32 ExtractUInt32(byte[] payLoadBytes, int pos)
        {
            var bytes = new byte[4];
            Array.Copy(payLoadBytes, pos, bytes, 0, bytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            return BitConverter.ToUInt32(bytes, 0);
        }
        
        private static string ByteArrayToString(byte[] payLoadBytes, int start, int length)
        {
            var bytes = new byte[length];
            Array.Copy(payLoadBytes, start, bytes, 0, bytes.Length);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(bytes);
        }

        private static string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }

        private static string ByteArrayToHexString(byte[] arr)
        {
            string hex = BitConverter.ToString(arr).Replace("-", string.Empty);
            return hex;
        }

        private static string ByteToString(byte value)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(new byte[value]);
        }

        public static uint DateTimeToSecondsAfterYear2000(DateTime dateTime)
        {
            DateTime year2000 = new DateTime(2000, 01, 01, 0, 0, 0, new DateTimeKind());
            var seconds = (uint)(dateTime - year2000).TotalSeconds;
            return seconds;
        }

        public static DateTime SecondsAfterYear2000ToDateTime(uint secondsAfterYear2000)
        {
            DateTime year2000 = new DateTime(2000, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateTime;
            if (secondsAfterYear2000 < int.MaxValue)
            {
                dateTime = year2000 + new TimeSpan(0, 0, (int)secondsAfterYear2000);
            }
            else  //Year 2068 ?
            {
                uint halfOfIntMaxValue =  (int.MaxValue / 2);
                dateTime = year2000 + new TimeSpan(0, 0, (int)halfOfIntMaxValue) + new TimeSpan(0, 0, (int)(secondsAfterYear2000 - halfOfIntMaxValue));
            }

            return dateTime;
        }
    }
}