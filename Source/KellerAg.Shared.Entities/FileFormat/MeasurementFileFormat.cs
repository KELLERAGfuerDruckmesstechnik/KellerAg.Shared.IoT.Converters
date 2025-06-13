using KellerAg.Shared.Entities.Calculations;
using KellerAg.Shared.Entities.Channel;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KellerAg.Shared.Entities.FileFormat
{
    public class MeasurementFileFormat
    {
        public int Version => 1;

        public MeasurementFileFormatHeader Header { get; set; }

        public List<Measurements> Body { get; set; }
    }

    [DataContract]
    public class Measurements
    {
        [DataMember(Name = "t")]  //https://stackify.com/top-11-json-performance-usage-tips/
        public System.DateTime Time { get; set; }
        [DataMember(Name = "v")]
        public double?[] Values { get; set; }
    }

    public class MeasurementFileFormatHeader
    {
        /// <summary>
        /// The list of all MeasurementDefinitionIds such as 1="Pd (P1-P2)", 2="P1", 3="P2", 4="T", ...
        /// </summary>
        public int[] MeasurementDefinitionsInBody { get; set; }

        /// <summary>
        /// The list of alternative names of the channels.
        /// 7 is normally "PBaro". Some customer may change it to "Air Pressure".
        /// In this case MeasurementDefinitionsInBodyAlternativeNames[6]=="Air Pressure".
        /// If the default name (eg. "PBaro") should be used then MeasurementDefinitionsInBodyAlternativeNames[6]==null.
        ///
        /// In the cloud backend I do this:
        ///  int maxMeasurementDefinitionNumber = pageSettings.AlternativeChannelNames.Max(c => c.MeasurementDefinition);
        ///      measurementFileFormat.Header.MeasurementDefinitionsInBodyAlternativeNames = new string[maxMeasurementDefinitionNumber];
        ///      foreach (var alternativeChannelName in pageSettings.AlternativeChannelNames)
        ///      {
        ///          if (alternativeChannelName.Alternative == null)
        ///          {
        ///              measurementFileFormat.Header.MeasurementDefinitionsInBodyAlternativeNames[alternativeChannelName.MeasurementDefinition - 1] = null;
        ///          }
        ///          else
        ///          {
        ///              measurementFileFormat.Header.MeasurementDefinitionsInBodyAlternativeNames[alternativeChannelName.MeasurementDefinition - 1] = alternativeChannelName.Alternative;
        ///          }
        ///      }
        /// </summary>
        [ItemCanBeNull]
        [Obsolete("Alternative channel names should be handled by the individual applications and not by the file format")]
        public string[] MeasurementDefinitionsInBodyAlternativeNames { get; set; }

        /// <summary>
        /// Unique Record Id- Generated with UniqueSerialNumber + FirstMeasurementUTC + LastMeasurementUTC (Datetime Format: yyyyMMddHHmmss)
        /// </summary>
        public string RecordId { get; set; }

        /// <summary>
        /// Based on Device-ID and Serial Number. See MeasurementFileFormatHelper.GenerateUniqueSerialNumber() in
        /// </summary>
        public string UniqueSerialNumber { get; set; }

        /// <summary>
        /// Every device has a serial number. But every product line starts each devices with 1. Therefore, this serial number is not unique
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Give a good clear name. A good as possible. e.g Cloud: "Eulach 10 - Winterthur / GSM: +41774692307"  or Desktop: "LEO-5 (Serial#1234)"
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Mobile needs device typ for identification --> XX.YY (ex. 30.05)"
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// The UTC time the file will be created
        /// /// The time difference between this und CreationDateTimeDeviceTime is the time zone difference (+/- device inaccuracy)
        /// </summary>
        public System.DateTime CreationDateTimeUTC { get; set; }

        /// <summary>
        /// The device time the file will be created.
        /// The time difference between this und CreationDateTimeUTC is the time zone difference (+/- device inaccuracy)
        /// </summary>
        public System.DateTime CreationDateTimeDeviceTime { get; set; }

        /// <summary>
        /// IANA Time Zone Name  e.g. "Europe/Zurich" or "Etc/GMT-1"
        /// Must be one of DateTimeHelper.GetIanaTimeZoneNames()
        /// Best use: IanaTimeZoneName = DateTimeHelper.GetLocalSystemIanaTimeZoneName()
        ///
        /// Use it for:
        /// DateTimeHelper dth = new DateTimeHelper(TimeDifferenceLocalTimeToUtcText);
        /// DateTime dt = dth.LocalizeDateTime("2018-10-28T17:10:42"); //A string in UTC
        /// See DateTimeTest.cs
        /// </summary>
        public string IanaTimeZoneName { get; set; }

        /// <summary>
        /// Should be .Body.First().Time
        /// </summary>
        public System.DateTime FirstMeasurementUTC { get; set; }

        /// <summary>
        /// Should be .Body.Last().Time
        /// </summary>
        public System.DateTime LastMeasurementUTC { get; set; }

        /// <summary>
        /// Which SW created this file?
        /// </summary>
        public Origin CreationOrigin { get; set; }

        /// <summary>
        /// We might encrypt the body in the future. With this field we prevent
        /// </summary>
        public bool IsBodyCompressed { get; set; }

        public InternalMemoryInfo MemoryInfo { get; set; }

        /// <summary>
        /// Information about the source measurements of the compensated channels.
        /// Only is initialized when a channel is compensated with channels from another measurement
        /// </summary>
        public CompensationInfo[] CompensationSourcesInfo { get; set; }

        /// <summary>
        /// Information for RemoteTransmissionUnits
        /// </summary>
        public RemoteTransmissionUnitInfo RemoteTransmissionUnitInfo { get; set; }

        public MeasurementFileFormatCustomAttributes CustomAttributes { get; set; }

        public MeasurementFileFormatWaterCalculationStoredInDevice WaterCalculationStoredInDeviceSettings { get; set; }

        public List<MeasurementFileFormatChannelCalculation> ChannelCalculations { get; set; }
    }

    public class CompensationInfo
    {
        public string SourceUniqueSerialNumber { get; set; }

        public int[] GeneratedMeasurementDefinitionIds { get; set; }

        public System.DateTime CompensationDateUTC { get; set; } 

    }

    public class MeasurementFileFormatCustomAttributes
    {
        /// <summary>
        /// Custom record Name, editable by user
        /// </summary>
        public string RecordName { get; set; }

        /// <summary>
        /// Custom Notes for record, editable by user
        /// </summary>
        public string RecordNotes { get; set; }
    }

    public class InternalMemoryInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public double SizeItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte HighPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte LowPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte EndHighPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte EndLowPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double SizeRecord { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RecordTextSize { get; set; }
    }

    public class MeasurementFileFormatWaterCalculationStoredInDevice
    {
        public MeasurementFileFormatWaterLevel WaterLevelCalculation { get; set; }

        public MeasurementFileFormatOverflow OverflowCalculation { get; set; }

        public object TankCalculation { get; set; }
    }

    public class MeasurementFileFormatWaterLevel
    {

        public WaterLevelType WaterLevelType { get; set; }  //Why not MeasurementDefId??

        /// <summary>
        /// First Pressure: Can be the pressure difference, too
        /// Id represents the MeasurementDefinitionId
        /// </summary>
        public int HydrostaticPressureChannelId { get; set; }

        /// <summary>
        /// Second Pressure. Can be null, too.
        /// Id represents the MeasurementDefinitionId
        /// </summary>
        public int? BarometricPressureChannelId { get; set; }

        public bool UseBarometricPressureToCompensate { get; set; }

        public double Offset { get; set; }

        public double Density { get; set; }

        public double Gravity { get; set; }

        public double InstallationLength { get; set; }

        public double HeightOfWellhead { get; set; }
    }

    public class MeasurementFileFormatOverflow
    {
        public OverflowType OverflowType { get; set; }

        public int HydrostaticPressureChannelId { get; set; }

        public int? BarometricPressureChannelId { get; set; }

        public bool UseBarometricPressureToCompensate { get; set; }

        public double Offset { get; set; }

        public double Density { get; set; }

        public double Gravity { get; set; }

        public double WallHeight { get; set; }

        public double FormFactor { get; set; }

        public double FormAngle { get; set; }

        public double FormWidth { get; set; }
    }

    public class MeasurementFileFormatChannelCalculation
    {
        public virtual Dictionary<CalculationParameter, string> CalculationParameters { get; set; }

        /// <summary>
        /// Represents ...
        /// </summary>
        public virtual int CalculationTypeId { get; set; }

        public ChannelInfo ChannelInfo { get; set; }
    }

    public class RemoteTransmissionUnitInfo
    {
        /// <summary>
        /// This Id indicates which channels can be measured by the sensors connected to the remote transmission device (sometimes called: DeviceTypeId). Range 0-13
        /// </summary>
        public int ConnectionTypeId { get; set; }
    }

    public enum WaterLevelType
    {
        HeightOfWater = 34,
        DepthToWater = 35,
        HeightOfWaterAboveSeaLevel = 36
    }

    public enum OverflowType
    {
        Poleni,
        Thomson,
        Venturi
    }

    /// <summary>
    /// 'Origin' stands for the best known source of logger/software that originally gathered the measurements.
    /// Measurement collected by the Logger5 then converted to the Measurement-FileFormat
    /// by the Desktop app and then uploaded to the Cloud-DB ideally has "Logger5" as Origin
    /// </summary>
    public enum Origin
    {
        PressureSuite,  //generic term. Better than "unknown"
        Script,
        Logger4,
        Logger5,
        PressureSuiteDesktop,
        PressureSuiteMobile,
        PressureSuiteCloud,
        PressureSuiteDesktopLiveMeasurement,
        PressureSuiteMobileLiveMeasurement
    }

}
