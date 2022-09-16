using KellerAg.Shared.Entities.Channel;
using System.Linq;

namespace KellerAg.Shared.Entities.Device
{
    public class DeviceInfo
    {
        public static DeviceInfo[] GetDevices()
        {
            return new[]
            {
                new DeviceInfo("Unknown device", "This device is not supported", 0, 0, DeviceType.Unknown),

                // Converter K-114
                new DeviceInfo("Converter K-114", "", 20, 1, DeviceType.ConverterK114),
                new DeviceInfo("Converter K-114 M", "", 20, 2, DeviceType.ConverterK114_M),
                new DeviceInfo("Converter K-114 BT", "", 20, 5, DeviceType.ConverterK114_BT),

                // DCX
                new DeviceInfo("DCX", "Various DCX models: DCX 16, DCX 22, ...", 5, 5, DeviceType.DCX22),
                new DeviceInfo("DCX 22", "DCX for Pacitec", 5, 6, DeviceType.DCX22),
                new DeviceInfo("DCX 18 ECO", "DCX-18-ECO   (mit Akku) mit Leiterplatte 08022", 5, 7, DeviceType.DCX18ECO),
                new DeviceInfo("DCX 22", "DCX-22 Hochtemperatur mit option Startknopf/LED und Timetable ab Index 10", 5, 8, DeviceType.DCX22),
                new DeviceInfo("DCX 18", "DCX-18 mit neuer Elektronik (Lade-IC) Leiterplatte 12011", 5, 9, DeviceType.DCX18ECO),
                new DeviceInfo("DCX CTD", "DCX with conductivity ", 5, 10, DeviceType.DCX_CTD),

                // LEO
                new DeviceInfo("LEO 1 / LEO 2", "", 7, 0, DeviceType.LEO1_2),
                new DeviceInfo("LEO 3", "", 7, 1, DeviceType.LEO3),
                new DeviceInfo("ECO", "on LEO basis", 7, 2, DeviceType.ECO1),
                new DeviceInfo("LEO", "LEO for Volvo", 7, 3, DeviceType.LeoVolvo),
                new DeviceInfo("LEO", "LEO for Isler", 7, 4, DeviceType.LeoIsler),
                new DeviceInfo("LEO", "", 7, 5, DeviceType.LEO1_2),
                new DeviceInfo("LEO", "LEO for Gühring", 7, 6, DeviceType.LeoGuehring),

                // LEO 5 / LEO1_EM
                new DeviceInfo("LEO 5", "", 30, 1, DeviceType.Leo5),
                new DeviceInfo("LEO 5 SF6", "", 30, 2, DeviceType.Leo5),
                new DeviceInfo("LEO 5 RS485", "", 30, 3, DeviceType.Leo5),
                new DeviceInfo("LEO 5 LoRa", "", 30, 4, DeviceType.Leo5),
                new DeviceInfo("LEO 1", "with energy micro- Leopard Gecko", 30, 5, DeviceType.LEO1_2),
                new DeviceInfo("LEO 5", "", 30, 6, DeviceType.Leo5),

                // LEX / LEO1X / LEO Record
                new DeviceInfo("LEO 1X", "", 10, 0, DeviceType.LEO1x),
                new DeviceInfo("LEO 1X", "", 10, 1, DeviceType.LEO1x),
                new DeviceInfo("LEO Record", "", 10, 2, DeviceType.LeoRecord),
                new DeviceInfo("LEO Record", "", 10, 3, DeviceType.LeoRecord),
                new DeviceInfo("LEX", "", 10, 5, DeviceType.Lex1),
                new DeviceInfo("LEX", "with leak function", 10, 6, DeviceType.Lex1),
                new DeviceInfo("LEX", "", 10, 7, DeviceType.Lex1),
                new DeviceInfo("LEX", "", 10, 8, DeviceType.Lex1),
                new DeviceInfo("LEX", "", 10, 10, DeviceType.Lex1),
                new DeviceInfo("LEO 1 / LEO 2", "", 10, 50, DeviceType.LEO1_2),
                
                // GSM / ARC / ADT
                new DeviceInfo("GSM-1", "", 9, 0, DeviceType.GSM1),
                new DeviceInfo("GSM-1", "GSM for PALL", 9, 1, DeviceType.GSM1),
                new DeviceInfo("GSM-2", "", 9, 5, DeviceType.GSM2),
                new DeviceInfo("GSM-3I", "", 9, 7, DeviceType.GSM3),
                new DeviceInfo("GSM-3", "", 9, 8, DeviceType.GSM3),
                new DeviceInfo("GSM-3", "with FTP and e-mail", 9, 9, DeviceType.GSM3),
                new DeviceInfo("ARC1", "", 9, 20, DeviceType.ARC1),
                new DeviceInfo("ARC1-LR", "", 9, 50, DeviceType.ARC1_lora),
                new DeviceInfo("ADT1-LR", "", 19, 0, DeviceType.ADT1),
                new DeviceInfo("ADT1-NB", "", 19, 20, DeviceType.ADT1_cellular),

                // Internal KELLER devices
                new DeviceInfo("Internal KELLER device", "", 8, 0, DeviceType.Unknown),
                new DeviceInfo("Internal KELLER device", "", 8, 1, DeviceType.Unknown),
                new DeviceInfo("Internal KELLER device", "", 8, 2, DeviceType.Unknown),
                new DeviceInfo("Internal KELLER device", "", 8, 3, DeviceType.Unknown),
                new DeviceInfo("Internal KELLER device", "", 8, 4, DeviceType.Unknown),
                new DeviceInfo("Internal KELLER device", "", 8, 5, DeviceType.Unknown),
                new DeviceInfo("Internal KELLER device", "", 8, 6, DeviceType.Unknown),

                // Series 30
                new DeviceInfo("Series 30X", "", 5, 20, DeviceType.S30X),
                new DeviceInfo("Series 30X2", "", 5, 21, DeviceType.S30X2),
                new DeviceInfo("Series 30X2a", "", 5, 22, DeviceType.S30X2),
                new DeviceInfo("Series 30X2P", "", 5, 23, DeviceType.S30X2),
                new DeviceInfo("Series 30 Hummingbird", "", 5, 40, DeviceType.S30X2),
                new DeviceInfo("Series 30 DACS3", "", 5, 50, DeviceType.S30X2),
                new DeviceInfo("Series 30 DACS2", "", 5, 52, DeviceType.S30X2),
                new DeviceInfo("Series 30 S2A-Box", "", 5, 60, DeviceType.S30X2),
                new DeviceInfo("Series 30 X2P", "", 5, 70, DeviceType.S30X2),
                new DeviceInfo("Series 30 X2P", "", 5, 71, DeviceType.S30X2),
                new DeviceInfo("Series 30 X2P", "", 5, 72, DeviceType.S30X2),
                new DeviceInfo("Series 30 X2P", "", 5, 80, DeviceType.S30X2),

                // S30-IO-BOX
                new DeviceInfo("DPI30", "", 16, 1, DeviceType.Unknown),
                new DeviceInfo("DPI30", "", 16, 5, DeviceType.Unknown),
                new DeviceInfo("DPI30", "", 16, 6, DeviceType.Unknown),

                // DPI30
                new DeviceInfo("DPI30", "", 15, 1, DeviceType.Unknown),

                // PA-22 PS / DV-22 PP
                new DeviceInfo("DV-22 PP", "Programming device for PA-22 PS", 12, 1, DeviceType.dV_2PP),
                new DeviceInfo("PA-22 PS", "", 12, 10, DeviceType.Unknown),
                new DeviceInfo("PA-22 PS", "", 12, 11, DeviceType.Unknown),

                // DV2
                new DeviceInfo("DV2", "", 11, 0, DeviceType.dV_2),
                new DeviceInfo("DV2", "with on/off button and zero", 11, 3, DeviceType.dV_2),
                new DeviceInfo("DV2 - Mano Cool", "", 11, 5, DeviceType.dV_2Cool),
                new DeviceInfo("DV2", "Radtke dV2-CPM", 11, 6, DeviceType.dV_2_Radtke),
                new DeviceInfo("DV2", "", 11, 7, DeviceType.dV_2),
                new DeviceInfo("DV2", "Flowmeter", 11, 8, DeviceType.dV_2),
                new DeviceInfo("DV2", "", 11, 9, DeviceType.dV_2),
                new DeviceInfo("DV2-PS", "", 11, 10, DeviceType.dV_2PS),

                // Bt-Logger/Transmitter
                new DeviceInfo("Bluetooth Logger", "", 50, 0, DeviceType.Bt_Transmitter),

            };
        }

        public static DeviceInfo GetInfo(int deviceClass, int group)
        {
            return GetDevices().SingleOrDefault(device => device.Class == deviceClass && device.Group == group) ?? GetDevices()[0];
        }

        /// <summary>
        /// string in Format: "XX.XX (for example: 10.05)"
        /// </summary>
        /// <param name="deviceClassAndGroup"></param>
        /// <returns></returns>
        public static DeviceInfo GetInfo(string deviceClassAndGroup)
        {
            if (!string.IsNullOrEmpty(deviceClassAndGroup) && deviceClassAndGroup.Contains('.'))
            {
                var splits = deviceClassAndGroup.Split('.');
                if (int.TryParse(splits[0], out var devClass) && int.TryParse(splits[1], out var devGroup))
                {

                    return GetInfo(devClass, devGroup);
                }
            }

            return GetInfo(0, 0);
        }

        internal DeviceInfo(string name, string description, int deviceClass, int group, DeviceType type)
        {
            Name = name;
            Description = description;
            DeviceType = type;
            Class = deviceClass;
            Group = group;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Class { get; set; }

        public int Group { get; set; }

        public DeviceType DeviceType { get; set; }

        public ChannelType[] ChannelTypes { get; set; }

        public ChannelType[] PhysicalChannelTypes => ChannelTypes.Select(x => ChannelTypesHelper.NotPhysicalChannels.Contains(x) ? ChannelType.Undefined : x).ToArray();

        public double[] ChannelMaxValues { get; set; }

        public double[] ChannelMinValues { get; set; }

        public bool IsCTD => Class == 5 && Group == 10;
    }
}
