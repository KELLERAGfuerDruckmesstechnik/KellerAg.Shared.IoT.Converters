using System.Collections.Generic;

namespace KellerAg.Shared.LoRaPayloadConverter
{
    public class ConnectionTypeChannelLookup
    {
        public int ChannelNumber { get; }
        public int ConnectionDeviceTypeId { get; }
        public int MeasurementDefinitionId { get; }

        private ConnectionTypeChannelLookup(int channelNumber, int connectionDeviceTypeId, int measurementDefinitionId)
        {
            ChannelNumber = channelNumber;
            ConnectionDeviceTypeId = connectionDeviceTypeId;
            MeasurementDefinitionId = measurementDefinitionId;
        }

        /// <summary>
        /// The set of used Channels are different on each ConnectionDeviceType.
        /// 
        /// This class maps the channel number of a certain DeviceType to a universal MeasurementDefinitionId
        /// 
        /// copied from Azure SQL Server: Table [TypeMappings]
        /// </summary>
        /// <returns></returns>
        public static List<ConnectionTypeChannelLookup> GetChannelInfo()
        {
            return new List<ConnectionTypeChannelLookup>
            {
                new ConnectionTypeChannelLookup(1, 0, 1),
                new ConnectionTypeChannelLookup(2, 0, 2),
                new ConnectionTypeChannelLookup(3, 0, 3),
                new ConnectionTypeChannelLookup(4, 0, 4),
                new ConnectionTypeChannelLookup(5, 0, 5),
                new ConnectionTypeChannelLookup(6, 0, 6),

                new ConnectionTypeChannelLookup(1, 1, 1),
                new ConnectionTypeChannelLookup(2, 1, 2),
                new ConnectionTypeChannelLookup(3, 1, 3),
                new ConnectionTypeChannelLookup(4, 1, 4),
                new ConnectionTypeChannelLookup(5, 1, 5),
                new ConnectionTypeChannelLookup(6, 1, 6),

                new ConnectionTypeChannelLookup(1, 2, 11),
                new ConnectionTypeChannelLookup(2, 2, 2),
                new ConnectionTypeChannelLookup(3, 2, 3),
                new ConnectionTypeChannelLookup(4, 2, 4),
                new ConnectionTypeChannelLookup(5, 2, 5),
                new ConnectionTypeChannelLookup(6, 2, 6),
                new ConnectionTypeChannelLookup(7, 2, 7),
                new ConnectionTypeChannelLookup(8, 2, 8),

                new ConnectionTypeChannelLookup(1, 5, 11),
                new ConnectionTypeChannelLookup(2, 5, 2),
                new ConnectionTypeChannelLookup(3, 5, 3),
                new ConnectionTypeChannelLookup(4, 5, 4),
                new ConnectionTypeChannelLookup(5, 5, 5),
                new ConnectionTypeChannelLookup(6, 5, 6),
                new ConnectionTypeChannelLookup(7, 5, 7),
                new ConnectionTypeChannelLookup(8, 5, 8),
                new ConnectionTypeChannelLookup(9, 5, 9),
                new ConnectionTypeChannelLookup(10, 5, 10),

                new ConnectionTypeChannelLookup(1, 10, 11),
                new ConnectionTypeChannelLookup(2, 10, 2),
                new ConnectionTypeChannelLookup(3, 10, 3),
                new ConnectionTypeChannelLookup(4, 10, 14),
                new ConnectionTypeChannelLookup(5, 10, 5),
                new ConnectionTypeChannelLookup(6, 10, 6),
                new ConnectionTypeChannelLookup(7, 10, 7),
                new ConnectionTypeChannelLookup(8, 10, 8),
                new ConnectionTypeChannelLookup(9, 10, 9),
                new ConnectionTypeChannelLookup(10, 10, 10),
                new ConnectionTypeChannelLookup(11, 10, 12),
                new ConnectionTypeChannelLookup(12, 10, 13),

                new ConnectionTypeChannelLookup(1, 3, 11),
                new ConnectionTypeChannelLookup(2, 3, 2),
                new ConnectionTypeChannelLookup(3, 3, 3),
                new ConnectionTypeChannelLookup(4, 3, 4),
                new ConnectionTypeChannelLookup(5, 3, 5),
                new ConnectionTypeChannelLookup(6, 3, 6),
                new ConnectionTypeChannelLookup(7, 3, 7),
                new ConnectionTypeChannelLookup(8, 3, 8),

                new ConnectionTypeChannelLookup(1, 4, 1),
                new ConnectionTypeChannelLookup(2, 4, 2),
                new ConnectionTypeChannelLookup(3, 4, 3),
                new ConnectionTypeChannelLookup(4, 4, 4),
                new ConnectionTypeChannelLookup(5, 4, 5),
                new ConnectionTypeChannelLookup(6, 4, 6),
                new ConnectionTypeChannelLookup(7, 4, 7),
                new ConnectionTypeChannelLookup(8, 4, 8),
                new ConnectionTypeChannelLookup(9, 4, 9),
                new ConnectionTypeChannelLookup(10, 4, 10),

                new ConnectionTypeChannelLookup(1, 6, 1),
                new ConnectionTypeChannelLookup(2, 6, 2),
                new ConnectionTypeChannelLookup(3, 6, 3),
                new ConnectionTypeChannelLookup(4, 6, 4),
                new ConnectionTypeChannelLookup(5, 6, 5),
                new ConnectionTypeChannelLookup(6, 6, 6),
                new ConnectionTypeChannelLookup(7, 6, 7),
                new ConnectionTypeChannelLookup(8, 6, 8),
                new ConnectionTypeChannelLookup(9, 6, 9),
                new ConnectionTypeChannelLookup(10, 6, 10),
                new ConnectionTypeChannelLookup(11, 6, 15),
                new ConnectionTypeChannelLookup(12, 6, 16),
                new ConnectionTypeChannelLookup(13, 6, 17),
                new ConnectionTypeChannelLookup(14, 6, 18),
                new ConnectionTypeChannelLookup(15, 6, 19),

                //new ConnectionTypeChannelLookup(1, 7, NOT USED),
                new ConnectionTypeChannelLookup(2, 7, 7),
                new ConnectionTypeChannelLookup(3, 7, 8),
                new ConnectionTypeChannelLookup(4, 7, 9),
                new ConnectionTypeChannelLookup(5, 7, 10),
                new ConnectionTypeChannelLookup(6, 7, 20),
                new ConnectionTypeChannelLookup(7, 7, 21),
                new ConnectionTypeChannelLookup(8, 7, 22),
                new ConnectionTypeChannelLookup(9, 7, 23),
                new ConnectionTypeChannelLookup(10, 7, 24),
                new ConnectionTypeChannelLookup(11, 7, 25),
                new ConnectionTypeChannelLookup(12, 7, 26),
                new ConnectionTypeChannelLookup(13, 7, 27),
                new ConnectionTypeChannelLookup(14, 7, 28),
                new ConnectionTypeChannelLookup(15, 7, 29),

                new ConnectionTypeChannelLookup(1, 8, 2),
                new ConnectionTypeChannelLookup(2, 8, 5),
                new ConnectionTypeChannelLookup(3, 8, 15),
                new ConnectionTypeChannelLookup(4, 8, 30),
                new ConnectionTypeChannelLookup(5, 8, 16),
                new ConnectionTypeChannelLookup(6, 8, 31),
                new ConnectionTypeChannelLookup(7, 8, 17),
                new ConnectionTypeChannelLookup(8, 8, 32),
                new ConnectionTypeChannelLookup(9, 8, 18),
                new ConnectionTypeChannelLookup(10, 8, 33),
                new ConnectionTypeChannelLookup(11, 8, 9),
                new ConnectionTypeChannelLookup(12, 8, 10),
                new ConnectionTypeChannelLookup(13, 8, 7),
                new ConnectionTypeChannelLookup(14, 8, 8),
                new ConnectionTypeChannelLookup(15, 8, 19),

                new ConnectionTypeChannelLookup(1, 9, 1),
                new ConnectionTypeChannelLookup(2, 9, 2),
                new ConnectionTypeChannelLookup(3, 9, 3),
                new ConnectionTypeChannelLookup(4, 9, 14),
                new ConnectionTypeChannelLookup(5, 9, 5),
                new ConnectionTypeChannelLookup(6, 9, 6),
                new ConnectionTypeChannelLookup(7, 9, 7),
                new ConnectionTypeChannelLookup(8, 9, 8),
                new ConnectionTypeChannelLookup(9, 9, 9),
                new ConnectionTypeChannelLookup(10, 9, 10),
                new ConnectionTypeChannelLookup(11, 9, 12),
                new ConnectionTypeChannelLookup(12, 9, 13),

                new ConnectionTypeChannelLookup(1 ,11,2 ),
                new ConnectionTypeChannelLookup(2 ,11,5 ),
                new ConnectionTypeChannelLookup(3 ,11,12),
                new ConnectionTypeChannelLookup(4 ,11,14),
                new ConnectionTypeChannelLookup(5 ,11,15),
                new ConnectionTypeChannelLookup(6 ,11,30),
                new ConnectionTypeChannelLookup(7 ,11,42),
                new ConnectionTypeChannelLookup(8 ,11,44),
                new ConnectionTypeChannelLookup(9 ,11,16),
                new ConnectionTypeChannelLookup(10,11,31),
                new ConnectionTypeChannelLookup(11,11,43),
                new ConnectionTypeChannelLookup(12,11,45),
                new ConnectionTypeChannelLookup(13,11,7 ),
                new ConnectionTypeChannelLookup(14,11,8 ),
                new ConnectionTypeChannelLookup(15,11,19),

                new ConnectionTypeChannelLookup(1 ,12,11),
                new ConnectionTypeChannelLookup(2 ,12,2),
                new ConnectionTypeChannelLookup(3 ,12,3),
                new ConnectionTypeChannelLookup(4 ,12,4),
                new ConnectionTypeChannelLookup(5 ,12,5),
                new ConnectionTypeChannelLookup(6 ,12,6),
                new ConnectionTypeChannelLookup(7 ,12,7),
                new ConnectionTypeChannelLookup(8 ,12,8),
                new ConnectionTypeChannelLookup(9 ,12,9),
                new ConnectionTypeChannelLookup(10,12,10),
                new ConnectionTypeChannelLookup(11,12,48),
                new ConnectionTypeChannelLookup(12,12,49),
                new ConnectionTypeChannelLookup(13,12,50),
                new ConnectionTypeChannelLookup(14,12,51),
                new ConnectionTypeChannelLookup(15,12,19),

                new ConnectionTypeChannelLookup(1 ,13,2 ),
                new ConnectionTypeChannelLookup(2 ,13,3 ),
                new ConnectionTypeChannelLookup(3 ,13,5 ),
                new ConnectionTypeChannelLookup(4 ,13,6 ),
                new ConnectionTypeChannelLookup(5 ,13,15),
                new ConnectionTypeChannelLookup(6 ,13,46),
                new ConnectionTypeChannelLookup(7 ,13,30),
                new ConnectionTypeChannelLookup(8 ,13,47),
                new ConnectionTypeChannelLookup(9 ,13,7 ),
                new ConnectionTypeChannelLookup(10,13,8 ),
                new ConnectionTypeChannelLookup(11,13,9 ),
                new ConnectionTypeChannelLookup(12,13,10),
                new ConnectionTypeChannelLookup(13,13,19)
            };
        }
    }
}