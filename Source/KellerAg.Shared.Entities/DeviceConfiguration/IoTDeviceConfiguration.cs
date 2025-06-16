
using System;

namespace KellerAg.Shared.Entities.DeviceConfiguration
{
    public class StoreableDeviceConfigurationModel : Storeable<DeviceConfigurationModel>
    {
        //Looks like this is needed to use the type in xaml....
    }

    public class Storeable<T> where T : class
    {
        public Storeable(T model)
        {
            Initialize();
            Model = model;
        }

        public Storeable()
        {
            Initialize();
        }

        private void Initialize()
        {
            Id = Guid.NewGuid().ToString("N");
            Version = 1;
        }

        public string Id { get; set; }

        public int Version { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public T Model { get; set; }
    }

    public class DeviceConfigurationModel
    {
        public virtual int DeviceConfigurationVersion { get; set; }

        /// <summary>
        /// IoT-Lora: 1
        /// IoT-Cellular: 2
        /// </summary>
        public virtual int DeviceConfigurationType { get; set; }

        public int DeviceClass { get; set; }
        public int DeviceGroup { get; set; }

    }
    public abstract class IoTDeviceConfigurationModel : DeviceConfigurationModel
    {
        public bool MeasureFunctionEnabled { get; set; }
        public bool AlarmFunctionEnabled { get; set; }
        public bool InfoFunctionEnabled { get; set; }
        public bool WaterLevelEnabled { get; set; }
        public WaterCalculationConfigurationModel WaterCalculation { get; set; }
        public IoTHardwareConfigurationModel HardwareConfiguration { get; set; }
    }

    public class LoRaDeviceConfigurationModel : IoTDeviceConfigurationModel
    {
        public override int DeviceConfigurationVersion => 1;
        public override int DeviceConfigurationType => 1;
        public MeasureFunctionConfigurationModel Measure { get; set; }
        public AlarmFunctionConfigurationModel Alarm { get; set; }
        public InfoFunctionConfigurationModel Info { get; set; }

        public LinkCheckFunctionConfigurationModel LinkCheck { get; set; }
        public bool LinkCheckFunctionEnabled { get; set; }

        public RejoinFunctionConfigurationModel Rejoin { get; set; }
        public bool RejoinFunctionEnabled { get; set; }

        public LoRaConnectionConfigurationModel LoRaSettings { get; set; }
        public LocationInfoConfigurationModel LocationInfo { get; set; }
    }

    public class CellularDeviceConfigurationModel : IoTDeviceConfigurationModel
    {
        public override int DeviceConfigurationVersion => 1;
        public override int DeviceConfigurationType => 2;

        public CellularMeasureFunctionConfigurationModel Measure { get; set; }
        public CellularAlarmFunctionConfigurationModel Alarm { get; set; }
        public CellularInfoFunctionConfigurationModel Info { get; set; }

        public DataConnectionFunctionConfigurationModel DataConnection { get; set; }
        public CellularCheckFunctionConfigurationModel CellularCheck { get; set; }
        public bool DataConnectionEnabled { get; set; }

        public bool CellularCheckFunctionEnabled { get; set; }

        /// <summary>
        /// This should never be disabled. If disabled data will be sent in plaintext and will not be able to be processed by other services
        /// This property is only read from the device, never written. The value is basically a duplicate from the EventType in Event (0 = false, 1-4 = true)
        /// </summary>
        public bool RecordDataTransferEnabled { get; set; }

        /// <summary>
        /// Only works with <see cref="RecordDataTransferEnabled"/> enabled
        /// </summary>
        public int SendAfterAmountOfFilesReached { get; set; }

        /// <summary>
        /// Only works with <see cref="RecordDataTransferEnabled"/> enabled
        /// </summary>
        public EventFunctionConfigurationModel Event { get; set; }

        /// <summary>
        /// Only works with <see cref="RecordDataTransferEnabled"/> enabled
        /// This can be changed by the device if the value of "Event.EventType >= 2" -> better control the enable/disable of events with "Event.EventType = 1" (off)
        /// </summary>
        public bool EventFunctionEnabled { get; set; }

        public FtpConnectionConfigurationModel FtpConnection { get; set; }

        public MailConnectionConfigurationModel MailConnection { get; set; }

        public InternetConnectionConfigurationModel InternetConnection { get; set; }

        public CellularIdentificationConfigurationModel LocationInfo { get; set; }

    }

    public class IoTHardwareConfigurationModel
    {
        /// <summary>
        /// type 0 (RS485) - type 13 (2x[P1;P2;TOB1;TOB2] & Baro & Dig.Inp.1/2 = Counter Inp. & Volt)
        /// </summary>
        public int ConnectedDeviceType { get; set; }

        /// <summary>
        /// 0 = off
        /// 1 = 12 V
        /// 2 = 5 V
        /// 3 = 3.7 V
        /// 4 = All on
        /// </summary>
        public int PowerSupply { get; set; }

        public int ActiveChannels { get; set; }

        public int PreOnTime { get; set; }
    }

    public class EventFunctionConfigurationModel
    {
        public DateTime NextDetection { get; set; }
        public TimeSpan EventDetectionInterval { get; set; }
        public TimeSpan OnActiveInterval { get; set; }

        /// <summary>
        /// 0 = Inactive -> This should never be the case. If 0, the data will be sent in plaintext and will not be able to be processed by other services
        /// 1 = Active
        /// 2 = On/Off
        /// 3 = Delta/Save
        /// 4 = Delta/Send
        /// </summary>
        public int EventType { get; set; }
        public int EventChannel { get; set; }
        public double TriggerOnValue { get; set; }
        public double TriggerOffValue { get; set; }
        public double DeltaValue { get; set; }

        /// <summary>
        /// Record data is sent at the latest after this amount of files are reached
        /// </summary>
        public int MaxFileBatchSize { get; set; }

    }

    public class WaterCalculationConfigurationModel
    {
        public double HeightOfWellheadAboveSea { get; set; }
        public double InstallationDepth { get; set; }
        public double Density { get; set; }
        public double Offset { get; set; }
        public int Channel { get; set; }

        /// <summary>
        /// Height of water
        /// Distance to water surface
        /// Height above sea level
        /// Overflow poleni
        /// Overflow thomson
        /// </summary>
        public int CalculationType { get; set; }

        public double OverflowPoleniWidth { get; set; }
        public double OverflowThomsonAngle { get; set; }
        public double OverflowMinimumCalculationHeight { get; set; }
        public double OverflowFormFactor { get; set; }
    }

    public class LocationInfoConfigurationModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public string NetworkName { get; set; }
        public string LocationName { get; set; }
    }

    public class CellularIdentificationConfigurationModel : LocationInfoConfigurationModel
    {
        public string Id { get; set; }
    }

    public class CellularCheckFunctionConfigurationModel : IntervalConfigurationModel
    {
        public bool CheckMail { get; set; }
        public bool CheckFtp { get; set; }
        public bool CheckSms { get; set; }
        public string SmsAccessPw { get; set; }
        public string SmsText { get; set; }
        /// <summary>
        /// 0 = Deactivated
        /// 1 = Update after each connection
        /// 2 = Update only when difference larger than <see cref="TimeSynchronizationThreshold"/>
        /// </summary>
        private int TimeSynchronizationMode { get; set; }

        private TimeSpan TimeSynchronizationThreshold { get; set; }
    }

    public class InternetConnectionConfigurationModel
    {
        public string Apn { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DnsServer { get; set; }
        public string SmsServiceCenterNumber { get; set; }
        public string SimPin { get; set; }
    }

    public class MailConnectionConfigurationModel
    {
        /// <summary>
        /// Displayed as sender
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// return address
        /// </summary>
        public string ReturnMailAddress { get; set; }

        public string PopServerAddress { get; set; }
        public int? PopServerPort { get; set; }
        public string PopServerUserId { get; set; }
        public string PopServerPassword { get; set; }
        public bool PopServerUseSsl { get; set; }

        public bool UseAlternateSmtpLogin { get; set; }
        public string SmtpServerAddress { get; set; }
        public int? SmtpServerPort { get; set; }
        public string SmtpServerUserId { get; set; }
        public string SmtpServerPassword { get; set; }
        public bool SmtpServerUseSsl { get; set; }


    }


    public class FtpConnectionConfigurationModel
    {
        public string AccountName { get; set; }
        /// <summary>
        /// URL or IP
        /// </summary>
        public string HostAddress { get; set; }
        /// <summary>
        /// E-Mail
        /// </summary>
        public string UserId { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// Default 21
        /// </summary>
        public int? SourceControlPort { get; set; }
        /// <summary>
        /// Default 21
        /// </summary>
        public int? DestinationControlPort { get; set; }
        public string FtpDirectory { get; set; }
        public bool ActiveMode { get; set; }
        /// <summary>
        /// Use SSL/TLS
        /// </summary>
        public bool UseSecureProtocol { get; set; }
        /// <summary>
        /// 0 = SSLv3
        /// 1 = TLSv1.0
        /// 2 = TLSv1.1
        /// 3 = TLSv1.2
        /// 4 = TLSv1.3
        /// </summary>
        public int TlsVersion { get; set; }
    }

    public class LoRaConnectionConfigurationModel
    {
        /// <summary>
        /// 0 = ABP
        /// 1 = OTAA
        /// </summary>
        public int ActivationMethod { get; set; }
        public string DeviceEui { get; set; }
        public string ApplicationEui { get; set; }
        public string AppKey { get; set; }
        public string DeviceAddress { get; set; }
        public string NetworkSessionKey { get; set; }
        public string AppSessionKey { get; set; }
        /// <summary>
        /// 0 = unconfirmed
        /// 1 = confirmed
        /// </summary>
        public int UpLinkMode { get; set; }
        /// <summary>
        /// 0 = ADR off
        /// 1 = ADR on
        /// </summary>
        public int AdaptiveDataRate { get; set; }
        /// <summary>
        /// 0 SF12 / 125kHz
        /// 1 SF11 / 125kHz
        /// 2 SF10 / 125kHz
        /// 3 SF9 / 125kHz
        /// 4 SF8 / 125kHz
        /// 5 SF7 / 125kHz
        /// 6 SF7 / 250kHz
        /// 7 FSK / 50kbps
        /// </summary>
        public int DataRate { get; set; }
        /// <summary>
        /// 0 16dBm
        /// 1 14dBm
        /// 2 12dBm
        /// 3 10dBm
        /// 4 8dBm
        /// 5 6dBm
        /// 6 4dBm
        /// 7 2dBm
        /// </summary>
        public int PowerIndex { get; set; }
        /// <summary>
        /// 0 AS923
        /// 1 AU915
        /// 5 EU868
        /// 6 KR920
        /// 7 IN865
        /// 8 US915
        /// 9 US915-HYBRID
        /// </summary>
        public int RadioBand { get; set; }
    }

    public class AlarmFunctionConfigurationModel : IntervalConfigurationModel
    {
        public double AlarmOnValue { get; set; }
        public double AlarmOffValue { get; set; }
        /// <summary>
        /// Change per interval
        /// </summary>
        public double AlarmDeltaValue { get; set; }
        public int AlarmChannel { get; set; }
        /// <summary>
        /// 0 = Alarm off
        /// 1 = On / Off
        /// 2 = Delta / interval
        /// 3 = Switch input 2 (nc)
        /// </summary>
        public int AlarmType { get; set; }

        /// <summary>
        /// While alarm is active, Alarm is sent this amount of times
        /// </summary>
        public int SendAlarmAmountOfTimes { get; set; }
    }

    public class DataConnectionFunctionConfigurationModel : IntervalConfigurationModel
    {
        public string Number { get; set; }
        /// <summary>
        /// 0 = Analog V.32
        /// 1 = Analog V.34
        /// 2 = ISDN V.110
        /// </summary>
        public int ConnectionProtocol { get; set; }
    }

    public class RejoinFunctionConfigurationModel : IntervalConfigurationModel
    {
    }

    public class InfoFunctionConfigurationModel : IntervalConfigurationModel
    {
    }

    public class MeasureFunctionConfigurationModel : IntervalConfigurationModel
    {
    }

    public class CellularMeasureFunctionConfigurationModel : MeasureFunctionConfigurationModel
    {
        public string SmsNumber { get; set; }
        public string SmsText { get; set; }
        public bool ActiveSms { get; set; }
        /// <summary>
        /// Sends SMS after this amount of measurements
        /// </summary>
        public int SendAmountSms { get; set; }
        public string MailAddress { get; set; }
        public bool ActiveMail { get; set; }
        /// <summary>
        /// Sends Mail after this amount of measurements
        /// </summary>
        public int SendAmountMail { get; set; }
        public bool ActiveFtp { get; set; }
        /// <summary>
        /// Sends to FTP after this amount of measurements
        /// </summary>
        public int SendAmountFtp { get; set; }
    }

    public class CellularInfoFunctionConfigurationModel : InfoFunctionConfigurationModel
    {
        public string SmsNumber { get; set; }
        public bool ActiveSms { get; set; }
        public string MailAddress { get; set; }
        public bool ActiveMail { get; set; }
        public bool ActiveFtp { get; set; }
    }

    public class CellularAlarmFunctionConfigurationModel : AlarmFunctionConfigurationModel
    {
        public string SmsNumber { get; set; }
        public string SmsText { get; set; }
        public bool ActiveSms { get; set; }
        public string MailAddress { get; set; }
        public bool ActiveMail { get; set; }
        public bool ActiveFtp { get; set; }
    }

    public class LinkCheckFunctionConfigurationModel : IntervalConfigurationModel
    {
    }

    public abstract class IntervalConfigurationModel
    {
        public TimeSpan Interval { get; set; }
        public DateTime NextAction { get; set; }
    }
}
