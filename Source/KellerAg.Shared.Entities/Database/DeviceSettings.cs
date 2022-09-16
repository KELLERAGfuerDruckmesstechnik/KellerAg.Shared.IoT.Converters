namespace Entities.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Indicates the configuration state.
    /// E.g.
    /// 10 + 20 : The configuration is meant to be programmed but it is unknown if it is already programmed onto the device
    /// 30      : The configuration has been programmed and the ACK is recognized or '0'. This is the "active" configuration
    /// 99      : There is a newer acknowledged configuration and therefore this is obsolete
    /// </summary>
    public enum SendState
    {
        /// <summary>
        /// Unknown state
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// It has not been programmed into the device.
        /// </summary>
        PendingForProgramming = 10,

        /// <summary>
        /// It has been programmed into the device. The message with the ACK is on the way to the KOLIBRI server to be stored into the DB
        /// </summary>
        PendingForServerAcknowledge = 20,

        /// <summary>
        /// The message is acknowledged, stored into the DB and the most recent configuration used in the device as far as we know it
        /// </summary>
        ConfirmedActiveness = 30,

        /// <summary>
        /// There is another more recent configuration programmed into the device. Therefore, this configuration is obsolete.
        /// </summary>
        ObsoleteActiveness = 99
    }

    /// <summary>
    /// The Configuration entity.
    /// Important: for the properties, do not use the same name as for Device, otherwise the ordering won't work.
    /// </summary>
    public class DeviceSettings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Indicates the configuration state.
        /// E.g.
        /// 10 + 20 : The configuration is meant to be programmed but it is unknown if it is already programmed onto the device
        /// 30      : The configuration has been programmed and the ACK is recognized or '0'. This is the "active" configuration
        /// 99      : There is a newer acknowledged configuration and therefore this is obsolete
        /// </summary>
        public SendState State { get; set; }

        /// <summary>
        /// Indicate if it was sent from the WebApp or manually through GSMSetup/GSMDatamanager
        /// The ACK Id is either a number (Sent from the API) which is smaller than 65000 (Id%65000==Ack-Id) or it is 0 (Sent from GSMSetup/Datamanager)
        /// </summary>
        public bool IsSentFromAPI { get; set; }

        // We need to identify the device when we search for the active DeviceConfiguration.
        // Let's use the serial number for now as the deviceId is unknown during creation.
        // Alternatively, we can use the DeviceId and update the number after creation.
        public string UniqueSerialNumber { get; set; }


        /// <summary>
        /// SHOULD ALWAYS BE ON
        /// #c/x (read only) AND #c/y (write only), Bit Position 0
        /// </summary>
        public bool? LockTimerMeasurement { get; set; }

        /// <summary>
        /// #c/x (read only) AND #c/y (write only), Bit Position 1
        /// </summary>
        public bool? LockTimerAlarm { get; set; }

        /// <summary>
        /// #c/x (read only) AND #c/y (write only), Bit Position 2
        /// </summary>
        public bool? LockTimerInfo { get; set; }

        /// <summary>
        /// SHOULD ALWAYS BE ON 
        /// #c/x (write only) AND #c/y (read only), Bit Position 3
        /// </summary>
        public bool? LockTimerCheck { get; set; }

        /// <summary>
        /// SHOULD ALWAYS BE OFF
        /// #c/x (read only) AND #c/y (write only), Bit Position 4
        /// </summary>
        public bool? LockTimerDial { get; set; }


        //------------------------------------------------------------------------------------------
        // General Settings

        /// <summary>
        /// #b/r
        /// </summary>
        [StringLength(20)]
        public string GeneralNetworkName { get; set; }

        /// <summary>
        /// #b/s
        /// </summary>
        [StringLength(30)]
        public string GeneralOwnTelNumber { get; set; }

        /// <summary>
        /// #b/t
        /// </summary>
        [StringLength(50)]
        public string GeneralLocationName { get; set; }


        /// <summary>
        /// DB only
        /// </summary>
        [StringLength(100)]
        public string GeneralLocationAddressText { get; set; }

        /// <summary>
        /// #I/n
        /// </summary>
        [StringLength(30)]
        public string GeneralGsmSerialNumber { get; set; }

        /// <summary>
        /// #I/f
        /// </summary>
        [StringLength(16)]
        public string GeneralGsmSoftwareVersion { get; set; }

        /// <summary>
        /// #b/0
        /// </summary>
        [StringLength(20)]
        public string GeneralLongitudeText { get; set; }

        /// <summary>
        /// #b/1
        /// </summary>
        [StringLength(20)]
        public string GeneralLatitudeText { get; set; }

        /// <summary>
        /// #b/2
        /// </summary>
        [StringLength(20)]
        public string GeneralAltitudeText { get; set; }

        /// <summary>
        /// #d/0
        /// </summary>
        public decimal? GeneralGpsLongitude { get; set; }

        /// <summary>
        /// #d/1
        /// </summary>
        public decimal? GeneralGpsLatitude { get; set; }

        /// <summary>
        /// #d/2
        /// </summary>
        public decimal? GeneralGpsAltitude { get; set; }

        /// <summary>
        /// #b/3, Readonly, recalibracte with #e/l
        /// </summary>
        [StringLength(20)]
        public string GeneralCellLocateLongitudeText { get; set; }

        /// <summary>
        /// #b/4, Readonly, recalibracte with #e/l
        /// </summary>
        [StringLength(20)]
        public string GeneralCellLocateLatitudeText { get; set; }

        /// <summary>
        /// #b/5, Readonly, recalibracte with #e/l
        /// </summary>
        [StringLength(20)]
        public string GeneralCellLocateAltitudeText { get; set; }


        // Hardware

        /// <summary>
        /// #c/6
        /// </summary>
        public byte? HardwareConnectionType { get; set; }

        /// <summary>
        /// #c/4, 0 = Deactivated | 1 = +12V | 2 = +5V | 3 = +3.9V | 4 = all On
        /// </summary>
        public byte? HardwarePowerExternalDevice { get; set; }

        /// <summary>
        /// #c/m, Bit Position 0
        /// </summary>
        public bool? HardwareMeasureSaveChannel0 { get; set; }

        /// <summary>
        /// #c/m, Bit Position 1
        /// </summary>
        public bool? HardwareMeasureSaveChannel1 { get; set; }

        /// <summary>
        /// #c/m, Bit Position 2
        /// </summary>
        public bool? HardwareMeasureSaveChannel2 { get; set; }

        /// <summary>
        /// #c/m, Bit Position 3
        /// </summary>
        public bool? HardwareMeasureSaveChannel3 { get; set; }

        /// <summary>
        /// #c/m, Bit Position 4
        /// </summary>
        public bool? HardwareMeasureSaveChannel4 { get; set; }

        /// <summary>
        /// #c/m, Bit Position 5
        /// </summary>
        public bool? HardwareMeasureSaveChannel5 { get; set; }

        /// <summary>
        /// #c/m, Bit Position 6
        /// </summary>
        public bool? HardwareMeasureSaveChannel6 { get; set; }

        /// <summary>
        /// #c/m, Bit Position 7
        /// </summary>
        public bool? HardwareMeasureSaveChannel7 { get; set; }

        /// <summary>
        /// #c/p, Bit Position 0
        /// </summary>
        public bool? HardwareMeasureSaveChannel8 { get; set; }

        /// <summary>
        /// #c/p, Bit Position 1
        /// </summary>
        public bool? HardwareMeasureSaveChannel9 { get; set; }

        /// <summary>
        /// #c/p, Bit Position 2
        /// </summary>
        public bool? HardwareMeasureSaveChannel10 { get; set; }

        /// <summary>
        /// #c/p, Bit Position 3
        /// </summary>
        public bool? HardwareMeasureSaveChannel11 { get; set; }

        /// <summary>
        /// #c/p, Bit Position 4
        /// </summary>
        public bool? HardwareMeasureSaveChannel12 { get; set; }

        /// <summary>
        /// #c/p, Bit Position 5
        /// </summary>
        public bool? HardwareMeasureSaveChannel13 { get; set; }

        /// <summary>
        /// #c/p, Bit Position 6
        /// </summary>
        public bool? HardwareMeasureSaveChannel14 { get; set; }

        /// <summary>
        /// #c/p, Bit Position 7
        /// </summary>
        public bool? HardwareMeasureSaveChannel15 { get; set; }

        /// <summary>
        /// #c/e, min = 0 | max = 5184000
        /// </summary>
        public int? HardwareDataConnectionTimer { get; set; }

        /// <summary>
        /// #c/k, min = 1 | max = 2592000
        /// </summary>
        public int? HardwareDataConnectionInterval { get; set; }

        /// <summary>
        /// #c/5
        /// </summary>
        public byte? HardwareSupportedSensorTypes { get; set; }

        /// <summary>
        /// #f/3, min = 0 | max = 254
        /// </summary>
        public int? HardwarePreOnTime { get; set; }

        /// <summary>
        /// #d/g, SMS only
        /// </summary>
        public decimal? HardwareMultiplierPressureChannels { get; set; }

        /// <summary>
        /// #d/f, SMS only
        /// </summary>
        public decimal? HardwareMultiplierTemperatureChannels { get; set; }

        /// <summary>
        /// #c/v, SMS only
        /// </summary>
        public byte? HardwareResolutionPressureChannels { get; set; }

        /// <summary>
        /// #c/w, SMS only
        /// </summary>
        public byte? HardwareResolutionTemperatureChannels { get; set; }

        /// <summary>
        /// #b/k
        /// </summary>
        [StringLength(30)]
        public string HardwareDataConnectionCallNumber { get; set; }


        // Measurement

        /// <summary>
        /// #c/a, min = 0 | max = 5184000
        /// </summary>
        public int? MeasurementTimer { get; set; }

        /// <summary>
        /// #c/g, min = 1 | max = 2592000
        /// </summary>
        public int? MeasurementInterval { get; set; }

        /// <summary>
        /// #c/z, Bit Position 0
        /// </summary>
        public bool? MeasurementSendSms { get; set; }

        /// <summary>
        /// #c/z, Bit Position 4
        /// </summary>
        public bool? MeasurementSendMail { get; set; }

        /// <summary>
        /// #f/z, Bit Position 0
        /// LETS NOT DO THIS ANYMORE FOR KOLIBRI
        /// </summary>
        public bool? MeasurementSendFTP { get; set; }

        /// <summary>
        /// #b/a
        /// </summary>
        [StringLength(50)]
        public string MeasurementMailAddress { get; set; }

        /// <summary>
        /// #b/m
        /// </summary>
        [StringLength(30)]
        public string MeasurementSmsNumber { get; set; }

        /// <summary>
        /// #b/u
        /// </summary>
        [StringLength(160)]
        public string MeasurementSmsText { get; set; }

        /// <summary>
        /// #c/q
        /// </summary>
        public byte? MeasurementSendMailAfterX { get; set; }

        /// <summary>
        /// #c/o
        /// </summary>
        public byte? MeasurementSendSmsAfterX { get; set; }

        /// <summary>
        /// #f/q
        /// LETS NOT DO THIS ANYMORE FOR KOLIBRI
        /// </summary>
        public byte? MeasurementSendFtpAfterX { get; set; }




        // GPRS

        /// <summary>
        /// #a/a
        /// </summary>
        [StringLength(50)]
        public string GprsAPN { get; set; }

        /// <summary>
        /// #a/b
        /// </summary>
        [StringLength(50)]
        public string GprsID { get; set; }

        /// <summary>
        /// #a/c
        /// </summary>
        [StringLength(50)]
        public string GprsPassword { get; set; }

        /// <summary>
        /// #a/d
        /// </summary>
        [StringLength(50)]
        public string GprsDNS { get; set; }

        /// <summary>
        /// #b/j
        /// </summary>
        [StringLength(10)]
        public string GprsSimPin { get; set; }

        /// <summary>
        /// #b/q
        /// </summary>
        [StringLength(30)]
        public string GprsSmsServiceCenterNr { get; set; }

        /// <summary>
        /// #b/g
        /// </summary>
        [StringLength(10)]
        public string GprsPasswordForQuerySms { get; set; }

        /// <summary>
        /// #c/0, 0 = 9600bps (V.32) | 1 = 9600bps (V.34) | 2 = 9600bps (V.110)
        /// </summary>
        public byte? GprsModemProtocol { get; set; }

        /// <summary>
        /// #a/o
        /// </summary>
        public string CellularModuleId { get; set; }
        /// <summary>
        /// #a/p
        /// </summary>
        public string CellularModuleRevisionId { get; set; }
        /// <summary>
        /// #a/q
        /// </summary>
        public string CellularModuleSerialNumberIMEI { get; set; }
        /// <summary>
        /// #a/r
        /// </summary>
        public string CellularSIMCardId { get; set; }
        /// <summary>
        /// #a/s
        /// </summary>
        public string CellularSIMCardSubscriberId { get; set; }


        //FTP
        /// <summary>
        /// #k/a
        /// </summary>
        [StringLength(50)]
        public string FtpServerName { get; set; }

        /// <summary>
        /// #k/h
        /// </summary>
        [StringLength(50)]
        public string FtpServerPath { get; set; }

        /// <summary>
        /// #k/b
        /// </summary>
        [StringLength(50)]
        public string FtpUsername { get; set; }

        /// <summary>
        /// #k/d
        /// </summary>
        [StringLength(50)]
        public string FtpAccount { get; set; }

        /// <summary>
        /// #k/c
        /// </summary>
        [StringLength(50)]
        public string FtpPassword { get; set; }

        /// <summary>
        /// #k/f, min = 0 | max = 65535
        /// </summary>
        public int? FtpPort { get; set; }

        /// <summary>
        /// #c/A
        /// </summary>
        public bool? FtpUseTLS { get; set; }

        /// <summary>
        /// #k/e, min = 0 | max = 65535
        /// </summary>
        public int? FtpSourceControlPort { get; set; }

        /// <summary>
        /// #k/g, min = 0 | max = 65535
        /// </summary>
        public int? FtpSourceDataPort { get; set; }

        /// <summary>
        /// #c/B
        /// </summary>
        public bool? FtpUseActiveMode { get; set; }

        // Mail
        /// <summary>
        /// #a/e
        /// </summary>
        [StringLength(50)]
        public string MailSmtpShowedName { get; set; }

        /// <summary>
        /// #a/f
        /// </summary>
        [StringLength(50)]
        public string MailPop3Username { get; set; }

        /// <summary>
        /// #a/g
        /// </summary>
        [StringLength(50)]
        public string MailPop3Password { get; set; }

        /// <summary>
        /// #c/1, Bit Position 0
        /// </summary>
        public bool? MailSmtpUseSSL { get; set; }

        /// <summary>
        /// #c/1, Bit Position 1
        /// </summary>
        public bool? MailPop3UseSSL { get; set; }

        /// <summary>
        /// #c/2
        /// </summary>
        public bool? MailUseAlternativeSMTPLogin { get; set; }

        /// <summary>
        /// #a/n
        /// </summary>
        [StringLength(50)]
        public string MailReturnAddress { get; set; }

        /// <summary>
        /// #a/h
        /// </summary>
        [StringLength(50)]
        public string MailOptSmtpUsername { get; set; }

        /// <summary>
        /// #a/i
        /// </summary>
        [StringLength(50)]
        public string MailOptSmtpPassword { get; set; }

        /// <summary>
        /// #a/l
        /// </summary>
        [StringLength(50)]
        public string MailSmtpServer { get; set; }

        /// <summary>
        /// #a/m, min = 0 | max = 65535
        /// </summary>
        public int? MailSmtpPort { get; set; }

        /// <summary>
        /// #a/j
        /// </summary>
        [StringLength(50)]
        public string MailPop3Server { get; set; }

        /// <summary>
        /// #a/k, min = 0 | max = 65535
        /// </summary>
        public int? MailPop3Port { get; set; }


        // Info
        /// <summary>
        /// #c/c, min = 0 | max = 5184000
        /// </summary>
        public int? InfoTimer { get; set; }

        /// <summary>
        /// #c/i, min = 1 | max = 2592000
        /// </summary>
        public int? InfoInterval { get; set; }

        /// <summary>
        /// #c/z, Bit Position 2
        /// </summary>
        public bool? InfoSendSms { get; set; }

        /// <summary>
        /// #c/z, Bit Position 6
        /// </summary>
        public bool? InfoSendMail { get; set; }

        /// <summary>
        /// #f/z, Bit Position 2
        /// </summary>
        public bool? InfoSendFTP { get; set; }

        /// <summary>
        /// #b/c
        /// </summary>
        [StringLength(50)]
        public string InfoMailAddress { get; set; }

        /// <summary>
        /// #b/o
        /// </summary>
        [StringLength(30)]
        public string InfoSmsNumber { get; set; }


        // Check
        /// <summary>
        /// #c/d, min = 0 | max = 5184000
        /// </summary>
        public int? CheckTimer { get; set; }

        /// <summary>
        /// #c/j, min = 1 | max = 2592000
        /// </summary>
        public int? CheckInterval { get; set; }

        /// <summary>
        /// #c/z, Bit Position 3
        /// </summary>
        public bool? CheckSendSms { get; set; }

        /// <summary>
        /// #c/z, Bit Position 7
        /// </summary>
        public bool? CheckSendMail { get; set; }

        /// <summary>
        /// #f/z, Bit Position 3
        /// </summary>
        public bool? CheckSendFTP { get; set; }

        /// <summary>
        /// #b/w
        /// </summary>
        [StringLength(160)]
        public string CheckAnswerSmsText { get; set; }

        // Alarm
        /// <summary>
        /// #c/b, min = 0 | max = 5184000
        /// </summary>
        public int? AlarmTimer { get; set; }

        /// <summary>
        /// #c/h, min = 1 | max = 2592000
        /// </summary>
        public int? AlarmInterval { get; set; }

        /// <summary>
        /// #c/z, Bit Position 1
        /// </summary>
        public bool? AlarmSendSms { get; set; }

        /// <summary>
        /// #c/z, Bit Position 5
        /// </summary>
        public bool? AlarmSendMail { get; set; }

        /// <summary>
        /// #f/z, Bit Position 1
        /// </summary>
        public bool? AlarmSendFTP { get; set; }

        /// <summary>
        /// #b/b
        /// </summary>
        [StringLength(50)]
        public string AlarmMailAddress { get; set; }

        /// <summary>
        /// #b/n
        /// </summary>
        [StringLength(30)]
        public string AlarmSmsNumber { get; set; }

        /// <summary>
        /// #b/v
        /// </summary>
        [StringLength(160)]
        public string AlarmSmsText { get; set; }

        /// <summary>
        /// #c/t
        /// </summary>
        public byte? AlarmSendXTimes { get; set; }

        /// <summary>
        /// #c/s, 1 = On / Off | 2 = Delta | 3 = Digital Input
        /// </summary>
        public byte? AlarmType { get; set; }

        /// <summary>
        /// #c/r
        /// </summary>
        public byte? AlarmChannelNumber { get; set; }

        /// <summary>
        /// #d/a
        /// </summary>
        public decimal? AlarmOnThreshold { get; set; }

        /// <summary>
        /// #d/b
        /// </summary>
        public decimal? AlarmOffThreshold { get; set; }

        /// <summary>
        /// #d/c
        /// </summary>
        public decimal? AlarmDeltaThreshold { get; set; }

        // Event
        /// <summary>
        /// #f/n, 0 = deactivated | 1 = activated | 2 = On / Off | 3 = Delta
        /// </summary>
        public byte? EventType { get; set; }

        /// <summary>
        /// #f/a, min = 0 | max = 5184000
        /// </summary>
        public int? EventMeasureTimer { get; set; }

        /// <summary>
        /// #f/g, min = 1 | max = 2592000
        /// </summary>
        public int? EventCheckInterval { get; set; }

        /// <summary>
        /// #f/h, min = 1 | max = 2592000
        /// </summary>
        public int? EventMeasureInterval { get; set; }

        /// <summary>
        /// #f/m
        /// </summary>
        public byte? EventChannel { get; set; }

        /// <summary>
        /// #d/i
        /// </summary>
        public decimal? EventOnValueThreshold { get; set; }

        /// <summary>
        /// #d/j
        /// </summary>
        public decimal? EventOffValueThreshold { get; set; }

        /// <summary>
        /// #d/k
        /// </summary>
        public decimal? EventDeltaValueThreshold { get; set; }

        /// <summary>
        /// #f/o, min = 1 | max = 30
        /// </summary>
        public int? EventSendMailXTimes { get; set; }


        // Water Level Calculation
        /// <summary>
        /// #d/m
        /// </summary>
        public decimal? WaterLevelCalculationEnable { get; set; }

        /// <summary>
        /// #d/n
        /// </summary>
        public decimal? WaterLevelCalculationLength { get; set; }

        /// <summary>
        /// #d/o
        /// </summary>
        public decimal? WaterLevelCalculationHeight { get; set; }

        /// <summary>
        /// #d/p
        /// </summary>
        public decimal? WaterLevelCalculationOffset { get; set; }

        /// <summary>
        /// #d/q
        /// </summary>
        public decimal? WaterLevelCalculationDensity { get; set; }

        /// <summary>
        /// #d/r
        /// </summary>
        public decimal? WaterLevelCalculationWidth { get; set; }

        /// <summary>
        /// #d/s
        /// </summary>
        public decimal? WaterLevelCalculationAngle { get; set; }

        /// <summary>
        /// #d/t
        /// </summary>
        public decimal? WaterLevelCalculationFormFactor { get; set; }

        /// <summary>
        /// #d/u
        /// </summary>
        public decimal? WaterLevelCalculationMinCalcHeight { get; set; }

        /// <summary>
        /// #c/3, 0 = none | 1 = A (0.85-0.88) | 2 = B (0.87-0.95) | 3 = C (1.13-1.27) | 4 = D (1.11)
        /// 5 = E (1.30) | 6 = F (1.37) | 7 = Venturi (1.75-2.02) | 8 = Open tube (0.529)
        /// </summary>
        public byte? WaterLevelCalculationFormType { get; set; }

        /// <summary>
        /// #c/7
        /// </summary>
        public bool? WaterLevelCalculationIsAbsoluteSensor { get; set; }

        /// <summary>
        /// #c/8, 0 = P1-P2 | 1 = P1-PBaro | 2 = P1 relative | 3 = not defined
        /// </summary>
        public byte? WaterLevelCalculationCalculateFrom { get; set; }

        /// <summary>
        /// #c/9, 0 = Height of water above level sensor (E) | 1 = Distance to water surface (F)
        /// 2 = Height of water above sea level (G) | 3 = Overflow (Poleni) | 4 = Overflow (Thomson)
        /// </summary>
        public byte? WaterLevelCalculationConversionTo { get; set; }


        /* Missing READ WRITE
         
        LoRaUplinkMode
        LoRaPowerExternalDevice (vs HardwarePowerExternalDevice)
        LoRaBatteryCapacity
        LoRaADR
        LoRaDR
        LoRaPowerIndex
        LoRaRadioBand
        
        LoRaMainTime
        LoRaMainTimeCorrectionInSec

        LoRaApplicationEUI
        LoRaApplicationKey
        LoRaDeviceAddress
        LoRaNetworkSessionKey
        LoRaAppSessionKey


        Was mit read-only fields??? Wo speichern?
        
        LoRaModuleType
        SerialNumber
        BatteryVoltage
        RelHumidity
        OffsetBarometer
        FirmwareVersionShort
        DeviceEUI

        KellerSensorInformation ...>9 fields


         */
    }
}
