using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
// ReSharper disable InconsistentNaming

namespace KellerAg.Shared.LoRaPayloadConverter
{
    #nullable enable
    public class PayloadInformation
    {
        /// <summary>
        /// This information is not part of the Payload.
        /// 1	Measurement (FunctionCode should be 1)
        /// 2	Alarm (FunctionCode should be 1)
        /// 3	Configuration (FunctionCode one of 31,32,51,52,61,62,71,72,81)
        /// 4	Info(Battery, Humidity, device time, ...) (FunctionCode most likely 12)
        /// 5	Answer on a request (Can be any FunctionCode)
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// If false then there was an error in the conversion of the payload and, therefore, it might be a payload from a non-KELLER device
        /// </summary>
        public bool IsSupportedDevice { get; set; }

        /// <summary>
        /// Gives information about which LoRa Network has been used
        /// </summary>
        public OriginLoRaNetwork OriginLoRaNetwork { get; set; }

        //The following information are sole based on the payload string.
        //The above information has to be injected to make a conversion happen.

        /// <summary>
        /// Raw payload decoded by the various methods of TTN, Actility or Loriot
        /// </summary>
        public byte[]? DecodedPayload { get; set; }

        /// <summary>
        /// Represents the “function code”. For measurement transmission this is  0x01 which specifies all measurements being in float format. There are others like 12,31,32,51,52,61,62,71,72,81 that represents other combinations of formats
        /// When Function==12, then it is a DeviceInformation
        /// </summary>
        public FunctionCodeId FunctionCode { get; set; }

        public List<Measurement>? Measurements { get; set; }

        public InfoMessage? Info { get; set; }

        public Dictionary<IndexOfValuesOf8Bit, byte>? ValuesOf8Bit { get; set; }

        public Dictionary<IndexOfValuesOf32Bit, uint>? ValuesOf32Bit{ get; set; }

        public Dictionary<IndexOfValuesOfFloat, float>? ValuesOfFloat { get; set; }

        public Dictionary<IndexOfTextValues, ValuesOfText>? ValueOfText { get; set; }

        public KellerSensorInfo? SensorInfo { get; set; }

        public CommandMessage? Command { get; set; }

        public PayloadInformation()
        {
            FunctionCode = FunctionCodeId.UnknownMessage;
        }
        public PayloadInformation(CommandMessage command)
        {
            Command = command;
            FunctionCode = FunctionCodeId.CommandMessage;
        }
    }




    [JsonConverter(typeof(StringEnumConverter))]
    public enum FunctionCodeId
    {
        UnknownMessage             = 0,

        MeasurementMessage         = 1,

        InfoMessage                = 12,

        ValuesOf8BitMessage        = 31,
        ValuesOf8BitStreamMessage  = 32,

        ValuesOf32BitMessage       = 51,
        ValuesOf32BitStreamMessage = 52,

        ValuesOfFloatMessage       = 61,
        ValuesOfFloatStreamMessage = 62,

        ValuesOfTextMessage        = 71,
        ValuesOfTextStreamMessage  = 72,

        SensorInformationMessage   = 81,

        CommandMessage             = 90
    }

    public class Measurement
    {
        /// <summary>
        /// Represents the Device Type (or “Connection Type”). Please see https://docs.kolibricloud.ch/sending-technology/lora-technology/keller-lora-payload/. The value is not zero-based (sorry)
        /// </summary>
        public int ConnectionDeviceType { get; set; }

        public int MeasurementDefinitionId { get; set; }

        public int ChannelNumber { get; set; }

        public float Value { get; set; }
    }

    public class InfoMessage
    {
        /// <summary>
        /// Unused. Reserved for future usage. Now it should be always 1.
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 12011300132600000001254D4FCA40A4FC2B0D10
        ///    battery_voltage: 0x40A4FC2B in float: 5.1557822227
        ///     (5.16 V)
        /// </summary>
        public float? BatteryVoltage { get; set; }

        public byte? BatteryCapacity { get; set; }

        public byte? HumidityPercentage { get; set; }

        public string? DeviceClassGroupText { get; set; }

        public string? SwVersionText { get; set; }

        public int SerialNumber { get; set; }
        
        /// <summary>
        /// Represents the DateTime used by the device that has a local time. It is not UTC.
        /// </summary>
        public DateTime DeviceLocalDateTime { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndexOfValuesOf8Bit
    {

        Unknown                          = 0,
        /// <summary>
        /// READ-ONLY
        /// </summary>
        DeviceClass                      = 1,
        /// <summary>
        /// READ-ONLY
        /// </summary>
        DeviceGroup                      = 2,
        /// <summary>
        /// READ-ONLY
        /// </summary>
        SWVersionYear                    = 3,
        /// <summary>
        /// READ-ONLY
        /// </summary>
        SWVersionWeek                    = 4,
        /// <summary>
        /// READ-ONLY
        /// </summary>
        SupportedMaxConnectionDeviceType = 5,

        /// <summary>
        /// first two bits define "join mode" (ABP or OTAA) and "message type" (confirmed or unconfirmed)
        /// </summary>
        UplinkMode                       = 6,
        DeviceType                       = 7,

        /// <summary>
        /// 1: power enabled
        /// 0: power disabled
        /// </summary>
        IsPowerForExternalDeviceEnabled  = 8,
        PowerPreOnTimeInSec              = 9,

        /// <summary>
        /// Bit pos 0: Measure
        /// Bit pos 1: Alarm
        /// Bit pos 2: Info
        /// </summary>
        LockTimer                        = 10,

        /// <summary>
        /// 8..15
        /// </summary>
        MeasureSaveChannelsHigh          = 11,
        /// <summary>
        /// 0..7
        /// </summary>
        MeasureSaveChannelsLow           = 12,

        EventChannel                     = 13,
        EventType                        = 14,
        AlarmChannel                     = 15,

        /// <summary>
        /// 1: On/Off alarm selected
        /// 2: Delta alarm selected
        /// </summary>
        AlarmType                        = 16,

        BatteryCapacityPercentage        = 17,

        /// <summary>
        /// 0-> ADR OFF
        /// 1-> ADR ON
        /// </summary>
        IsAdaptiveDataRateOn             = 18,

        /// <summary>
        /// 0-> SF12 / 125kHz
        /// 1-> SF11 / 125kHz
        /// 2-> SF10 / 125kHz
        /// 3-> SF9  / 125kHz
        /// 4-> SF8  / 125kHz
        /// 5-> SF7  / 125kHz
        /// Can be different for other regions (RadioBandRegion)
        /// </summary>
        DataRate = 19,

        /// <summary>
        /// 0-> 16dBm
        /// 1-> 14dBm (Default)
        /// 2-> 12dBm
        /// 3-> 10dBm
        /// 4-> 8dBm
        /// 5-> 6dBm
        /// 6-> 4dBm
        /// 7-> 2dBm
        /// Can be different for other regions (RadioBandRegion)
        /// </summary>
        PowerIndex = 20,

        /// <summary>
        /// 0-> AS923 (Asia)
        /// 1-> AU915 (Australia)
        /// 5-> EU868 (Europe / Default)
        /// 6-> KR920 (Korea)
        /// 7-> IN865 (India)
        /// 8-> US915 (USA)
        /// 9-> US915-HYBRID (USA)
        /// </summary>
        RadioBandRegion = 21,

        /// <summary>
        /// READ-ONLY
        /// <para>
        /// 0-> Unknown
        /// 1-> RN2483
        /// 2-> RN2903
        /// 3-> ABZ-093
        /// </para>
        /// </summary>
        LoRaModuleType                   = 22
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndexOfValuesOf32Bit
    {
        Unknown                    = 0,

        /// <summary>
        /// READ-ONLY
        /// </summary>
        SerialNumber               = 1,

        MainTime                   = 2,
        MainTimeCorrectionInSec    = 3,

        NextMeasureDateTime        = 4,
        NextAlarmDateTime          = 5,
        NextInfoDateTime           = 6,
        NextEventMeasuringDateTime = 7,

        MeasureIntervalInSec       = 8,
        AlarmIntervalInSec         = 9,
        InfoIntervalInSec          = 10,

        /// <summary>
        /// Not used
        /// </summary>
        EventCheckIntervalInSec    = 11,

        /// <summary>
        /// Not used
        /// </summary>
        EventMeasureIntervalInSec  = 12
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndexOfValuesOfFloat
    {
        Unknown                                            = 0,
        AlarmOnThreshold                                   = 1,
        AlarmOffThreshold                                  = 2,
        AlarmDeltaThreshold                                = 3,
        EventOnThreshold                                   = 4,
        EventOffThreshold                                  = 5,
        EventDeltaThreshold                                = 6,
        WaterLevelConfigurationEnable                      = 7,
        WaterLevelConfigurationLength_B                    = 8,
        WaterLevelConfigurationHeight_A                    = 9,
        WaterLevelConfigurationOffset_F                    = 10,
        WaterLevelConfigurationDensity                     = 11,
        WaterLevelConfigurationWidth_b                     = 12,
        WaterLevelConfigurationAngle_F                     = 13,
        WaterLevelConfigurationFormFactor_m                = 14,
        WaterLevelConfigurationMinimumCalculationHeight_h  = 15,
        WaterLevelConfigurationReserve1                    = 16,
        WaterLevelConfigurationReserve2                    = 17,
        WaterLevelConfigurationReserve3                    = 18,
        WaterLevelConfigurationReserve4                    = 19,
        WaterLevelConfigurationReserve5                    = 20,
        GPSCoordinateLongitude                             = 21,
        GPSCoordinateLatitude                              = 22,
        GPSCoordinateAltitude                              = 23,
        /// <summary>
        /// READ-ONLY
        /// </summary>
        BatteryVoltage                                     = 24,
        /// <summary>
        /// READ-ONLY
        /// </summary>
        RelativeHumidityPercentage                         = 25,
        /// <summary>
        /// READ-ONLY
        /// </summary>
        OffsetBarometer                                    = 26
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndexOfTextValues
    {
        Unknown              = 0,

        /// <summary>
        /// READ-ONLY
        /// </summary>
        FirmwareVersionShort = 1,

        /// <summary>
        /// READ-ONLY
        /// </summary>
        DeviceEUI            = 2,
        ApplicationEUI       = 3,
        ApplicationKey       = 4,
        DeviceAddress        = 5,
        NetworkSessionKey    = 6,
        AppSessionKey        = 7
    }

    public class ValuesOfText
    {
        /// <summary>
        /// Should be always complete. It is possible to only have a part of the text when having limited bandwidth (eg. in USA)
        /// </summary>
        public bool HasReachedEndOfText { get; set; }

        /// <summary>
        /// Should be 0. Otherwise, it's complicated and the LoRa transmission before hast the first part of the text
        /// </summary>
        public uint CharacterStartingPosition { get; set; }

        public string? Text { get; set; }
    }

    public class KellerSensorInfo
    {
        public int SensorCount { get; set; }
        public SensorType SensorType { get; set; }

        /// <summary>
        /// RS485 information
        /// </summary>
        public SensorType1? SensorType1Data { get; set; }

        /// <summary>
        /// I2C Information
        /// </summary>
        public SensorType2? SensorType2Data { get; set; }
    }

    public class SensorType1
    {
        public byte SensorClass { get; set; }
        public byte SensorGroup { get; set; }
        public byte SwVersionYear { get; set; }
        public byte SwVersionWeek { get; set; }
        public uint SerialNumber { get; set; }
    }

    public class SensorType2
    {
        public uint UniqueId { get; set; }
        public ushort Scaling0 { get; set; }
        public float PMinVal { get; set; }
        public float PMaxVal { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SensorType
    {
        Unknown                = 0,
        RS485                  = 1,
        InterIntegratedCircuit = 2
    }

    public class CommandMessage
    {
        public CommandFunction CommandFunction { get; set; }
        public byte StartIndex { get; set; }
        public byte EndIndex { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommandFunction
    {
        Unknown                                 = 0,
        RequestMeasuredValues                   = 1,
        RequestSmallConfiguration               = 2,
        Request1ByteStream_FunctionCode32       = 3,
        Reserved                                = 4,
        Request4ByteStream_FunctionCode52       = 5,
        RequestFloatStream_FunctionCode62       = 6,
        RequestInfoMessage_FunctionCode12       = 7,
        RequestBigConfiguration                 = 8,
        RequestTextStream_FunctionCode72        = 9,
        RequestSensorInformation_FunctionCode81 = 10,

        /// <summary>
        /// This index is only needed to reconfigure the LoRa Keys and Settings.
        /// Here you have to know exactly what you are doing
        /// </summary>
        AcceptingTheConfiguration = 11
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum OriginLoRaNetwork
    {
        Unknown = 0,
        TheThingsNetwork,
        Actility,
        Loriot
    }
}