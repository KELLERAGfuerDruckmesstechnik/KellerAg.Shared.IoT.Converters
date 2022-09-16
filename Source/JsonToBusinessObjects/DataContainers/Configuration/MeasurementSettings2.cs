namespace JsonToBusinessObjects.DataContainers.Configuration
{
    // ReSharper disable InconsistentNaming
    public class MeasurementSettings2
    {
      // #f /a-/3
        
        /// <summary>
        /// #f/a
        /// </summary>
        public uint TimerEvent { get; set; }
        /// <summary>
        /// #f/g
        /// </summary>
        public uint IntervalEventCheck { get; set; }
        /// <summary>
        /// #f/h
        /// </summary>
        public uint IntervalEventMeasure { get; set; }
        /// <summary>
        /// #f/m
        /// </summary>
        public byte EventChannel { get; set; }
        /// <summary>
        /// #f/n
        /// </summary>
        public byte EventType { get; set; }
        /// <summary>
        /// #f/o
        /// </summary>
        public byte SendAfterYFilesWithRecordData { get; set; }
        /// <summary>
        /// #f/q
        /// </summary>
        public byte SendToFtpAfterXCollectedMeasurements { get; set; }
        /// <summary>
        /// #f/z
        /// Bit 0 : Measure
        /// Bit 1 : Alarm
        /// Bit 2 : Info
        /// Bit 3 : Check - CHECK Function must always be active, when this is OFF, then Check-EMail is Activated
        /// </summary>
        public byte SendTypeFtp { get; set; }
        /// <summary>
        /// #f/3
        /// Gives power to external sensor defined seconds before read out
        /// </summary>
        public byte HardwarePreOnTimeInSeconds { get; set; }

    }
}