using System;

namespace JsonToBusinessObjects.DataContainers
{
    public class DeviceInformation
    {
        /// <summary>
        /// Each device belongs to a product line which are f.i. "LoRa", "GSM" or the newer "ARC1"
        /// See https://wiki.keller-druck.com/display/ORGET/Ph03-Review01-CLOUD1_W1
        /// </summary>
        public ProductLineName ProductLine { get; set; }

        /// <summary>
        /// Each product line starts counting its devices from 0.
        /// That means we have several KELLER devices with the same device serial number
        /// </summary>
        public int? DeviceSerialNumber { get; set; }

        /// <summary>
        /// Merging the information from the product line and the serial number together gives us an unique serial number
        /// or
        /// EUI
        /// </summary>
        public string UniqueSerialNumber { get; set; }

        public int? SignalQuality { get; set; }
        public byte? BatteryCapacity { get; set; }
        public string GsmModuleSoftwareVersion { get; set; }
        public float? MeasuredBatteryVoltage { get; set; }
        public string DeviceIdAndClass { get; set; }
        public DateTime? DeviceLocalDateTime { get; set; }

        public byte? MeasuredHumidity { get; set; }

        /// <summary>
        /// See https://wiki.keller-druck.com/display/ORGET/Ph03-Review01-CLOUD1_W1
        /// #I/t
        ///  /t=0  2G (GSM)
        ///  /t=1  2G (GSM Compact)
        ///  /t=2  3G (UTRAN)
        ///  /t=3  2G (GSM / EGPRS)
        ///  /t=4  3G (UTRAN / HSDPA)
        ///  /t=5  3G (UTRAN / HSUPA)
        ///  /t=6  3G (UTRAN / HSDPA / HSUPA)
        ///  /t=7  4G (E-UTRAN)
        ///  /t=8  4G (CAT M1)
        ///  /t=9  4G (NB IoT)
        /// </summary>
        public int? CellularTechnologyInUse { get; set; }
    }
}