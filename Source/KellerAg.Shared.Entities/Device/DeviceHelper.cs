using KellerAg.Shared.Entities.FileFormat;
using System.Linq;

namespace KellerAg.Shared.Entities.Device
{

    public static class DeviceHelper
    {
        private static string _prefixLogger = "REC";
        private static string _prefixARC = "ARC";
        private static string _prefixGSM = "GSM";
        private static string _prefixEUI = "EUI";
        private static string _prefixADT = "ADT";
        private static string _prefixConverter = "Converter";
        private static string _prefixUnknown = "unknown";

        /// <summary>
        /// Returns uniqueSerialNumber of the GSM device
        /// </summary>
        /// <param name="serialNumber">Serial number of the device</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumberGsm(string serialNumber)
        {
            return GenerateUniqueSerialNumber(GetClassAndGroup(DeviceType.GSM2), serialNumber);
        }

        /// <summary>
        /// Returns uniqueSerialNumber of the cellular device (ARC1 and ADT1-NB)
        /// </summary>
        /// <param name="deviceType">Device type in form CLASS.Group (XX.XX)</param>
        /// <param name="serialNumber">Serial number of the device</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumberCellular(string deviceType, string serialNumber)
        {
            return GenerateUniqueSerialNumber(deviceType, serialNumber);
        }

        /// <summary>
        /// Returns uniqueSerialNumber of the device
        /// </summary>
        /// <param name="deviceType">Type of the device</param>
        /// <param name="serialNumber">Serial number of the device</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumberCellular(DeviceType deviceType, string serialNumber)
        {
            return GenerateUniqueSerialNumber(GetClassAndGroup(deviceType), serialNumber);
        }

        /// <summary>
        /// Returns uniqueSerialNumber of the LoRa device (ADT1-LR and ARC-LR)
        /// </summary>
        /// <param name="deviceType">Type of the device</param>
        /// <param name="eui">EUI of the device</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumberLoRa(DeviceType deviceType, string eui)
        {
            return GenerateUniqueSerialNumber(GetClassAndGroup(deviceType), string.Empty, eui);
        }        
        
        /// <summary>
        /// Returns uniqueSerialNumber of the LoRa device
        /// </summary>
        /// <param name="deviceType">Device type in form CLASS.Group (XX.XX)</param>
        /// <param name="eui">EUI of the device</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumberLoRa(string deviceType, string eui)
        {
            return GenerateUniqueSerialNumber(deviceType, string.Empty, eui);
        }

        /// <summary>
        /// Returns uniqueSerialNumber of the device
        /// </summary>
        /// <param name="serialNumber">Serial number of the device</param>
        /// <param name="eui">EUI of the device</param>
        /// <param name="deviceType">Device type in form CLASS.Group (XX.XX)</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumber(string deviceType, string serialNumber, string eui = "")
        {
            switch (DeviceInfo.GetInfo(deviceType).DeviceType)
            {
                case DeviceType.ADT1:
                case DeviceType.ARC1_lora:
                    return $"{_prefixEUI}-{eui}";
                case DeviceType.ADT1_cellular:
                    return $"{_prefixADT}-{deviceType}-{serialNumber}";
                case DeviceType.GSM1:
                case DeviceType.GSM2:
                case DeviceType.GSM3:
                    return $"{_prefixGSM}-{serialNumber}";
                case DeviceType.ARC1:
                    return $"{_prefixARC}-{deviceType}-{serialNumber}";
                case DeviceType.DCX22:
                case DeviceType.DCX18ECO:
                case DeviceType.DCX22AA:
                case DeviceType.DCX_CTD:
                case DeviceType.Leo5:
                case DeviceType.LeoRecord:
                case DeviceType.Bt_Transmitter:
                    return $"{_prefixLogger}-{deviceType}-{serialNumber}";
                case DeviceType.ConverterK114:
                case DeviceType.ConverterK114_BT:
                case DeviceType.ConverterK114_M:
                    return $"{_prefixConverter}-{deviceType}-{serialNumber}";
                case DeviceType.LEO1_2:
                case DeviceType.LeoIsler:
                case DeviceType.LeoVolvo:
                case DeviceType.LeoGuehring:
                case DeviceType.Castello:
                case DeviceType.dV_2:
                case DeviceType.dV_2Cool:
                case DeviceType.dV_2PP:
                case DeviceType.dV_2PS:
                case DeviceType.dV_2_Radtke:
                case DeviceType.LEO1x:
                case DeviceType.LEO3:
                case DeviceType.ECO1:
                case DeviceType.Lex1:
                case DeviceType.S30X:
                case DeviceType.S30X2:
                case DeviceType.S30X2_Cond:
                case DeviceType.Unknown:
                    return $"{_prefixUnknown}-{deviceType}-{serialNumber}";
                default:
                    return $"{_prefixUnknown}-{deviceType}-{serialNumber}";
            }
        }

        /// <summary>
        /// Returns uniqueSerialNumber of the device
        /// </summary>
        /// <param name="header"></param>
        /// <param name="eui">must be set if the device is an ADT</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumber(MeasurementFileFormatHeader header, string eui = "")
        {
            return GenerateUniqueSerialNumber(header.DeviceType, header.SerialNumber, eui);
        }

        /// <summary>
        /// Returns uniqueSerialNumber of the device
        /// </summary>
        /// <param name="serialNumber">Serial number of the device</param>
        /// <param name="eui">EUI of the device</param>
        /// <param name="deviceInfo"></param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumber(DeviceInfo deviceInfo, string serialNumber, string eui = "")
        {
            return GenerateUniqueSerialNumber(GetClassAndGroup(deviceInfo.DeviceType), serialNumber, eui);
        }

        public static string GetClassAndGroup(DeviceType deviceType)
        {
            var device = DeviceInfo.GetDevices().FirstOrDefault(x => x.DeviceType == deviceType);
            return device != null ? $"{device.Class}.{device.Group}" : string.Empty;
        }

        public static string GetPrefix(string deviceType)
        {
            return GetPrefix(DeviceInfo.GetInfo(deviceType).DeviceType);
        }

        public static string GetPrefix(DeviceType type)
        {
            switch (type)
            {
                case DeviceType.ADT1:
                case DeviceType.ADT1_cellular:
                    return _prefixADT;
                case DeviceType.GSM1:
                case DeviceType.GSM2:
                case DeviceType.GSM3:
                    return _prefixGSM;
                case DeviceType.ARC1:
                case DeviceType.ARC1_lora:
                    return _prefixARC;
                case DeviceType.DCX22:
                case DeviceType.DCX18ECO:
                case DeviceType.DCX22AA:
                case DeviceType.DCX_CTD:
                case DeviceType.Leo5:
                case DeviceType.LeoRecord:
                case DeviceType.Bt_Transmitter:
                    return _prefixLogger;
                case DeviceType.ConverterK114:
                case DeviceType.ConverterK114_BT:
                case DeviceType.ConverterK114_M:
                    return _prefixConverter;
                case DeviceType.LEO1_2:
                case DeviceType.LeoIsler:
                case DeviceType.LeoVolvo:
                case DeviceType.LeoGuehring:
                case DeviceType.Castello:
                case DeviceType.dV_2:
                case DeviceType.dV_2Cool:
                case DeviceType.dV_2PP:
                case DeviceType.dV_2PS:
                case DeviceType.dV_2_Radtke:
                case DeviceType.LEO1x:
                case DeviceType.LEO3:
                case DeviceType.ECO1:
                case DeviceType.Lex1:
                case DeviceType.S30X:
                case DeviceType.S30X2:
                case DeviceType.S30X2_Cond:
                case DeviceType.Unknown:
                    return _prefixUnknown;
                default:
                    return _prefixUnknown;
            }
        }
    }
}
