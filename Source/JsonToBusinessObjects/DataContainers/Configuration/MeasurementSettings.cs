namespace JsonToBusinessObjects.DataContainers.Configuration
{
    // ReSharper disable InconsistentNaming
    public class MeasurementSettings
    {
        public byte MeasureAndSaveCH0_7 { get; set; }
        public byte MeasureAndSaveCH8_15 { get; set; }
        public uint Timer0Measure { get; set; }
        public uint Timer1Alarm { get; set; }
        public uint Timer2Info { get; set; }
        public uint Timer3Check { get; set; }
        public uint Timer4DataConnection { get; set; }
        public uint Interval0Measure { get; set; }
        public uint Interval1Alarm { get; set; }
        public uint Interval2Info { get; set; }
        public uint Interval3Check { get; set; }
        public uint Interval4DataConnection { get; set; }
        public byte SendSmsAfterXMeasurements { get; set; }

        /// <summary>
        /// #c/q
        /// </summary>
        public byte SendMailAfterXMeasurements { get; set; }
        public byte AlarmChannelNumber { get; set; }
        public byte AlarmType { get; set; }
        public byte SendAlarmXTimes { get; set; }
        public byte ResolutionPressureChannels { get; set; }
        public byte ResolutionTemperatureChannels { get; set; }
        public byte LockTimerOnlyCheck { get; set; }
        public byte LockTimersWithoutCheck { get; set; }
        public byte SendSmsEmail { get; set; }
        public byte ModemProtocol { get; set; }
        public byte AccountSetting { get; set; }
        public byte ServerConfig { get; set; }
        public byte OflFormType { get; set; }
        public byte PowerExternalDevice { get; set; }
        public byte SupportedConnectionTypes { get; set; }
        public byte ConnectionType { get; set; }
        public byte ConfigBytes0 { get; set; }
        public byte CalcChannels { get; set; }
        public byte CalcConversionTo { get; set; }
        public byte SslTlsFtpEnable { get; set; }

        /// <summary>
        /// Since 18.09
        /// READ-Only
        /// 0: Passive Mode
        /// 1: Active Mode
        /// </summary>
        public byte FtpMode { get; set; }
    }
}