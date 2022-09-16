namespace JsonToBusinessObjects.Conversion.GsmArc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MeasurementDataDecoder
    {
        private readonly StringBuilder _diagnosticInfo = new StringBuilder();

        public string DiagnosticOutput => _diagnosticInfo.ToString();

        public ChannelDataStorage Decode(byte[] data)
        {
            _diagnosticInfo.Clear();

            ChannelDataStorage cds = new ChannelDataStorage();

            List<IList<byte>> splitInPackets = SplitInPages(data).ToList();

            foreach (IList<byte> page in splitInPackets)
            {
                DecodePageIntoChannelDataStorage(page, cds);
            }

            return cds;
        }

        private void DecodePageIntoChannelDataStorage(IList<byte> data, ChannelDataStorage cds)
        {
            List<byte> header = data.Take(8).ToList();
            List<byte> dataPackets = data.Skip(8).ToList();

            DateTime startTime = RetrieveStartTimeFromHeader(header);

            DateTime currentTime = startTime;
            List<IList<byte>> splitInPackets = SplitInPackets(dataPackets).ToList();

            string binaryHeader = string.Join(" ", header.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

            _diagnosticInfo.AppendLine($"{binaryHeader} HEADER: StartTime: {startTime}");

            foreach (IList<byte> packet in splitInPackets)
            {
                int channel = packet[0] >> 4;
                int timeIntervalInSeconds = packet[0] & 0x0F;

                string firstByte  =  Convert.ToString(packet[0], 2).PadLeft(8, '0').Insert(4, " ");
                string otherBytes = (Convert.ToString(packet[1], 2).PadLeft(8, '0') 
                                     + Convert.ToString(packet[2], 2).PadLeft(8, '0')
                                     + Convert.ToString(packet[3], 2).PadLeft(8, '0')
                ).Insert(1, " ").Insert(9, " ");


                bool isTimeDelayPacket = channel == 15 && timeIntervalInSeconds == 0;
                bool isTextContentPacket = channel == 15 && timeIntervalInSeconds == 4;
                bool isEmptyPacket = channel == 15 && timeIntervalInSeconds == 15;

                float value = ConvertBytesToFloat(new byte[] {packet[1], packet[2], packet[3], 0});
                
                //test if OFL was send and wrongly decoded
                if (value > 9E10)
                {
                    value = float.NaN;
                }

                if (isEmptyPacket || isTextContentPacket)
                {
                    string kind = (isTextContentPacket ? "TEXT" : "") + (isEmptyPacket ? "EMPTY" : "");
                    _diagnosticInfo.AppendLine($@"{firstByte} {otherBytes} - {kind}");

                    // Stop processing after empty packet
                    break;
                }

                if (isTimeDelayPacket)
                {
                    int offset = ConvertBytesToInt(new byte[] {0, packet[1], packet[2], packet[3]});
                    currentTime = currentTime + TimeSpan.FromSeconds(offset);
                    _diagnosticInfo.AppendLine($@"{firstByte} {otherBytes} - DELAY: {offset}s");
                    continue;
                }

                if (timeIntervalInSeconds != 0)
                {
                    currentTime += TimeSpan.FromSeconds(timeIntervalInSeconds);
                }

                _diagnosticInfo.AppendLine($@"{firstByte} {otherBytes} - CH{channel} INT{timeIntervalInSeconds} F{value}");
                cds.StoreInChannel(channel, new DataPoint(currentTime, value));
            }

            _diagnosticInfo.AppendLine("");
        }

        private IEnumerable<IList<byte>> SplitInPackets(List<byte> dataPackets)
        {
            IEnumerable<byte> p = dataPackets;

            List<byte> ret = dataPackets.Take(4).ToList();

            var packets = new List<IList<byte>>();

            while (ret.Count > 0)
            {
                packets.Add(ret);

                // ReSharper disable once PossibleMultipleEnumeration - Readability
                p = p.Skip(4);

                // ReSharper disable once PossibleMultipleEnumeration - Readability
                ret = p.Take(4).ToList();
            }

            return packets;
        }

        private IEnumerable<IList<byte>> SplitInPages(byte[] dataPackets)
        {
            IEnumerable<byte> p = dataPackets;

            List<byte> ret = dataPackets.Take(64).ToList();

            var packets = new List<IList<byte>>();

            while (ret.Count > 0)
            {
                packets.Add(ret);

                // ReSharper disable once PossibleMultipleEnumeration - Readability
                p = p.Skip(64);

                // ReSharper disable once PossibleMultipleEnumeration - Readability
                ret = p.Take(64).ToList();
            }

            return packets;
        }

        private DateTime RetrieveStartTimeFromHeader(IList<byte> header)
        {
            if (header.Count != 8)
            {
                throw new ArgumentException();
            }

            byte[] bytes = header.Skip(2).Take(4).ToArray();

            int secondsSince2000 = ConvertBytesToInt(bytes);

            return new DateTime(2000, 1, 1, 0,0,0, DateTimeKind.Utc) + TimeSpan.FromSeconds(secondsSince2000);
        }

        private static int ConvertBytesToInt(byte[] bytes)
        {
            // If the system architecture is little-endian (that is, little end first), reverse the byte array.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }

        private static float ConvertBytesToFloat(byte[] bytes)
        {
            // If the system architecture is little-endian (that is, little end first), reverse the byte array.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }
    }
}