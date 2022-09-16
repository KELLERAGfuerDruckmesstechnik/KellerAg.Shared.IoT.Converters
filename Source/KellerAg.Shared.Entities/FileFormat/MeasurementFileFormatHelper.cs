using KellerAg.Shared.Entities.Device;

namespace KellerAg.Shared.Entities.FileFormat
{
    public static class MeasurementFileFormatHelper
    {
        public static string GenerateDefaultRecordName(MeasurementFileFormatHeader header)
        {
            return $"{DeviceHelper.GetPrefix(header.DeviceType)} {header.DeviceType} {header.FirstMeasurementUTC:yyyy.MM.dd HH-mm-ss}";
        }

        /// <summary>
        /// Generates and returns unique Id for a record
        /// </summary>
        /// <returns>Id of the record</returns>
        public static string GenerateRecordId(MeasurementFileFormatHeader header)
        {
            string uniqueSerialNumber;
            if (string.IsNullOrEmpty(header.UniqueSerialNumber) || header.UniqueSerialNumber == DeviceHelper.GetPrefix(DeviceType.Unknown))
            {
                uniqueSerialNumber = GenerateUniqueSerialNumber(header.DeviceType, header.SerialNumber);
            }
            else
            {
                uniqueSerialNumber = header.UniqueSerialNumber;
            }
            return uniqueSerialNumber + header.FirstMeasurementUTC.ToString("yyyyMMddHHmmss") +
                              header.LastMeasurementUTC.ToString("yyyyMMddHHmmss");
        }        
        
        /// <summary>
        /// Returns uniqueSerialNumber of the GSM device
        /// </summary>
        /// <param name="serialNumber">Serial number of the device</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumberGsm(string serialNumber)
        {
            return DeviceHelper.GenerateUniqueSerialNumberGsm(serialNumber);
        }

        /// <summary>
        /// Returns uniqueSerialNumber of the cellular device (ARC1 and ADT1-NB)
        /// </summary>
        /// <param name="deviceType">Device type in form CLASS.Group (XX.XX)</param>
        /// <param name="serialNumber">Serial number of the device</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumberCellular(string deviceType, string serialNumber)
        {
            return DeviceHelper.GenerateUniqueSerialNumberCellular(deviceType, serialNumber);
        }

        /// <summary>
        /// Returns uniqueSerialNumber of the LoRa device
        /// </summary>
        /// <param name="deviceType">Device type in form CLASS.Group (XX.XX)</param>
        /// <param name="eui">EUI of the device</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumberLoRa(string deviceType, string eui)
        {
            return DeviceHelper.GenerateUniqueSerialNumberLoRa(deviceType, eui);
        }

        /// <summary>
        /// Returns uniqueSerialNumber of the device
        /// </summary>
        /// <param name="serialNumber">Serial number of the device</param>
        /// <param name="eui">EUI of the device</param>
        /// <param name="deviceType"></param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumber(string deviceType, string serialNumber, string eui = "")
        {
            return DeviceHelper.GenerateUniqueSerialNumber(deviceType, serialNumber, eui);
        }

        /// <summary>
        /// Returns uniqueSerialNumber of the device
        /// </summary>
        /// <param name="header"></param>
        /// <param name="eui">must be set if the device is an ADT</param>
        /// <returns>Unique serial number of the device</returns>
        public static string GenerateUniqueSerialNumber(MeasurementFileFormatHeader header, string eui = "")
        {
            return DeviceHelper.GenerateUniqueSerialNumber(header, eui);
        }
    }
}
