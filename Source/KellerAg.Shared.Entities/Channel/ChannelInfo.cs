using KellerAg.Shared.Entities.Units;
using System;
using System.Linq;

namespace KellerAg.Shared.Entities.Channel
{
    public class ChannelInfo
    {
        public ChannelType ChannelType { get; set; }

        public int MeasurementDefinitionId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ColorCode { get; set; }

        public UnitType UnitType { get; set; }

        public ChannelInfo()
        {

        }
        public ChannelInfo(ChannelType channelType, int measurementDefinitionId, string name, string description, string colorCode, UnitType unitType)
        {
            ChannelType = channelType;
            MeasurementDefinitionId = measurementDefinitionId;
            Name = name;
            Description = description;
            ColorCode = colorCode;
            UnitType = unitType;
        }

        private static ChannelInfo[] Channels { get; set; } = null;

        public static ChannelInfo[] GetChannels()
        {
            return GetNewChannelsInstance();
        }



        /// *** * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * ***
        /// *** WARNING: PLEASE KEEP THIS FILE IN SYNC WITH Entities.Data.MeasurementDefinition
        /// ***          AND THE SQL DB
        /// ***          AND ChannelType.cs
        /// *** * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *** 
        internal static ChannelInfo[] GetNewChannelsInstance()
        {
            return new[]{
                    new ChannelInfo(ChannelType.Undefined      , (int)ChannelType.Undefined      , "Undefined"       , "", "#FFFFFF", UnitType.Unknown),
                    new ChannelInfo(ChannelType.Pd_P1P2        , (int)ChannelType.Pd_P1P2        , "Pd (P1-P2)"      , "", "#87a100", UnitType.Pressure),
                    new ChannelInfo(ChannelType.P1             , (int)ChannelType.P1             , "P1"              , "", "#107c10", UnitType.Pressure),
                    new ChannelInfo(ChannelType.P2             , (int)ChannelType.P2             , "P2"              , "", "#003b00", UnitType.Pressure),
                    new ChannelInfo(ChannelType.T              , (int)ChannelType.T              , "T"               , "", "#ff8c00", UnitType.Temperature),
                    new ChannelInfo(ChannelType.TOB1           , (int)ChannelType.TOB1           , "TOB1"            , "", "#f44335", UnitType.Temperature),
                    new ChannelInfo(ChannelType.TOB2           , (int)ChannelType.TOB2           , "TOB2"            , "", "#d50000", UnitType.Temperature),
                    new ChannelInfo(ChannelType.PBaro          , (int)ChannelType.PBaro          , "PBaro"           , "", "#007b7b", UnitType.Pressure),
                    new ChannelInfo(ChannelType.TBaro          , (int)ChannelType.TBaro          , "TBaro"           , "", "#992639", UnitType.Temperature),
                    new ChannelInfo(ChannelType.VoltIn1        , (int)ChannelType.VoltIn1        , "Volt Inp. 1"     , "", "#512da8", UnitType.Voltage),
                    new ChannelInfo(ChannelType.VoltIn2        , (int)ChannelType.VoltIn2        , "Volt Inp. 2"     , "", "#ffa000", UnitType.Voltage),
                    new ChannelInfo(ChannelType.Pd_P1PBaro     , (int)ChannelType.Pd_P1PBaro     , "Pd (P1-PBaro)"   , "", "#87a100", UnitType.Pressure),
                    new ChannelInfo(ChannelType.ConductivityTc , (int)ChannelType.ConductivityTc , "Conductivity Tc" , "", "#f94f00", UnitType.Conductivity),
                    new ChannelInfo(ChannelType.ConductivityRaw, (int)ChannelType.ConductivityRaw, "Conductivity raw", "", "#5d4037", UnitType.Conductivity),
                    new ChannelInfo(ChannelType.T_Conductivity , (int)ChannelType.T_Conductivity , "T (Conductivity)", "", "#ff8c00", UnitType.Temperature),
                    new ChannelInfo(ChannelType.P1_2           , (int)ChannelType.P1_2           , "P1 (2)"          , "", "#00b200", UnitType.Pressure),
                    new ChannelInfo(ChannelType.P1_3           , (int)ChannelType.P1_3           , "P1 (3)"          , "", "#00D800", UnitType.Pressure),
                    new ChannelInfo(ChannelType.P1_4           , (int)ChannelType.P1_4           , "P1 (4)"          , "", "#32FF32", UnitType.Pressure),
                    new ChannelInfo(ChannelType.P1_5           , (int)ChannelType.P1_5           , "P1 (5)"          , "", "#003f00", UnitType.Pressure),
                    new ChannelInfo(ChannelType.CounterIn      , (int)ChannelType.CounterIn      , "Counter input"   , "", "#000000", UnitType.Length),
                    new ChannelInfo(ChannelType.SDI12_CH1      , (int)ChannelType.SDI12_CH1      , "SDI12 CH1"       , "", "#000000", UnitType.Unknown),
                    new ChannelInfo(ChannelType.SDI12_CH2      , (int)ChannelType.SDI12_CH2      , "SDI12 CH2"       , "", "#212121", UnitType.Unknown),
                    new ChannelInfo(ChannelType.SDI12_CH3      , (int)ChannelType.SDI12_CH3      , "SDI12 CH3"       , "", "#454545", UnitType.Unknown),
                    new ChannelInfo(ChannelType.SDI12_CH4      , (int)ChannelType.SDI12_CH4      , "SDI12 CH4"       , "", "#737373", UnitType.Unknown),
                    new ChannelInfo(ChannelType.SDI12_CH5      , (int)ChannelType.SDI12_CH5      , "SDI12 CH5"       , "", "#a2a2a2", UnitType.Unknown),
                    new ChannelInfo(ChannelType.SDI12_CH6      , (int)ChannelType.SDI12_CH6      , "SDI12 CH6"       , "", "#2196f3", UnitType.Unknown),
                    new ChannelInfo(ChannelType.SDI12_CH7      , (int)ChannelType.SDI12_CH7      , "SDI12 CH7"       , "", "#1a75bf", UnitType.Unknown),
                    new ChannelInfo(ChannelType.SDI12_CH8      , (int)ChannelType.SDI12_CH8      , "SDI12 CH8"       , "", "#005ca8", UnitType.Unknown),
                    new ChannelInfo(ChannelType.SDI12_CH9      , (int)ChannelType.SDI12_CH9      , "SDI12 CH9"       , "", "#003866", UnitType.Unknown),
                    new ChannelInfo(ChannelType.SDI12_CH10     , (int)ChannelType.SDI12_CH10     , "SDI12 CH10"      , "", "#66baff", UnitType.Unknown),
                    new ChannelInfo(ChannelType.TOB1_2         , (int)ChannelType.TOB1_2         , "TOB1 (2)"        , "", "#d50000", UnitType.Temperature),
                    new ChannelInfo(ChannelType.TOB1_3         , (int)ChannelType.TOB1_3         , "TOB1 (3)"        , "", "#9E0000", UnitType.Temperature),
                    new ChannelInfo(ChannelType.TOB1_4         , (int)ChannelType.TOB1_4         , "TOB1 (4)"        , "", "#5E0000", UnitType.Temperature),
                    new ChannelInfo(ChannelType.TOB1_5         , (int)ChannelType.TOB1_5         , "TOB1 (5)"        , "", "#aa4444", UnitType.Temperature),

                    new ChannelInfo(ChannelType.mH20_E         , (int)ChannelType.mH20_E         , "mH20 (E)"        , "", "#8c45ef", UnitType.Length),
                    new ChannelInfo(ChannelType.mH20_F         , (int)ChannelType.mH20_F         , "mH20 (F)"        , "", "#2196f3", UnitType.Length),
                    new ChannelInfo(ChannelType.mH20_G         , (int)ChannelType.mH20_G         , "mH20 (G)"        , "", "#3f51b5", UnitType.Length),

                    new ChannelInfo(ChannelType.mH20_Pbaro     , (int)ChannelType.mH20_Pbaro     , "mH20 (Pbaro)"    , "", "#FFFFFF", UnitType.Length),
                    new ChannelInfo(ChannelType.mH20_P1P2      , (int)ChannelType.mH20_P1P2      , "mH20 (P1-P2)"    , "", "#FFFFFF", UnitType.Length),
                    new ChannelInfo(ChannelType.mH20_P1P3      , (int)ChannelType.mH20_P1P3      , "mH20 (P1-P3)"    , "", "#FFFFFF", UnitType.Length),
                    new ChannelInfo(ChannelType.mH20_P1P4      , (int)ChannelType.mH20_P1P4      , "mH20 (P1-P4)"    , "", "#FFFFFF", UnitType.Length),
                    new ChannelInfo(ChannelType.mH20_P1P5      , (int)ChannelType.mH20_P1P5      , "mH20 (P1-P5)"    , "", "#FFFFFF", UnitType.Length),

                    new ChannelInfo(ChannelType.ConductivityTc_2 , (int)ChannelType.ConductivityTc_2  , "Conductivity Tc (2)" , "", "#000000", UnitType.Conductivity),  //todo colors
                    new ChannelInfo(ChannelType.ConductivityTc_3 , (int)ChannelType.ConductivityTc_3  , "Conductivity Tc (3)" , "", "#000000", UnitType.Conductivity),
                    new ChannelInfo(ChannelType.T_Conductivity_2 , (int)ChannelType.T_Conductivity_2  , "T (Conductivity) (2)", "", "#ff8c00", UnitType.Temperature),
                    new ChannelInfo(ChannelType.T_Conductivity_3 , (int)ChannelType.T_Conductivity_3  , "T (Conductivity) (3)", "", "#ff8c00", UnitType.Temperature),

                    new ChannelInfo(ChannelType.P2_2             , (int)ChannelType.P2_2              , "P2 (2)"    , "", "#000000", UnitType.Pressure),
                    new ChannelInfo(ChannelType.TOB2_2           , (int)ChannelType.TOB2_2            , "TOB2 (2)"  , "", "#d50000", UnitType.Temperature),

                    new ChannelInfo(ChannelType.AquaMasterFlowRate             , (int)ChannelType.AquaMasterFlowRate              , "AquaMaster Flow Rate"               , "", "#000000", UnitType.Unknown),
                    new ChannelInfo(ChannelType.AquaMasterPressure             , (int)ChannelType.AquaMasterPressure              , "AquaMaster Pressure"                , "", "#212121", UnitType.Unknown),
                    new ChannelInfo(ChannelType.AquaMasterCustomFlowUnits      , (int)ChannelType.AquaMasterCustomFlowUnits       , "AquaMaster Custom Flow Units"       , "", "#454545", UnitType.Unknown),
                    new ChannelInfo(ChannelType.AquaMasterExternalSupplyVoltage, (int)ChannelType.AquaMasterExternalSupplyVoltage , "AquaMaster External Supply Voltage" , "", "#737373", UnitType.Unknown),

                    new ChannelInfo(ChannelType.TankContent1                   , (int)ChannelType.TankContent1             , "Tank Content 1", "", "#8c45ef", UnitType.Volume),
                    new ChannelInfo(ChannelType.TankContent2                   , (int)ChannelType.TankContent2             , "Tank Content 2", "", "#2196f3", UnitType.Volume),
                    new ChannelInfo(ChannelType.TankContent3                   , (int)ChannelType.TankContent3             , "Tank Content 3", "", "#3f51b5", UnitType.Volume),

                    new ChannelInfo(ChannelType.MultiSensor1                   , (int)ChannelType.MultiSensor1             , "Multi Sensor Channel 1", "Multi Sensor Channel 1", "#8c45ef", UnitType.Length), //Right now, only WaterCalculation can be set -> unit=m
                    new ChannelInfo(ChannelType.MultiSensor2                   , (int)ChannelType.MultiSensor2             , "Multi Sensor Channel 2", "Multi Sensor Channel 2", "#2196f3", UnitType.Length), //Right now, only WaterCalculation can be set -> unit=m
                    new ChannelInfo(ChannelType.MultiSensor3                   , (int)ChannelType.MultiSensor3             , "Multi Sensor Channel 3", "Multi Sensor Channel 3", "#3f51b5", UnitType.Length), //Right now, only WaterCalculation can be set -> unit=m
                    new ChannelInfo(ChannelType.MultiSensor4                   , (int)ChannelType.MultiSensor4             , "Multi Sensor Channel 4", "Multi Sensor Channel 4", "#87a100", UnitType.Length), //Right now, only WaterCalculation can be set -> unit=m
                    new ChannelInfo(ChannelType.MultiSensor5                   , (int)ChannelType.MultiSensor5             , "Multi Sensor Channel 5", "Multi Sensor Channel 5", "#107c10", UnitType.Length), //Right now, only WaterCalculation can be set -> unit=m

                    new ChannelInfo(ChannelType.Converter_Voltage_In           , (int)ChannelType.Converter_Voltage_In     , "mH20 (Pbaro)"    , "", "#000000", UnitType.Voltage),
                    new ChannelInfo(ChannelType.Converter_Voltage_Out          , (int)ChannelType.Converter_Voltage_Out    , "mH20 (P1-P2)"    , "", "#000000", UnitType.Voltage),
                    new ChannelInfo(ChannelType.Converter_Voltage_USB          , (int)ChannelType.Converter_Voltage_USB    , "mH20 (P1-P3)"    , "", "#000000", UnitType.Voltage),
                    new ChannelInfo(ChannelType.Converter_Current_Out          , (int)ChannelType.Converter_Current_Out    , "mH20 (P1-P4)"    , "", "#000000", UnitType.Voltage),

                    new ChannelInfo(ChannelType.P1_Min                         , (int)ChannelType.P1_Min                   , "P1 Min"          , "", "#000000", UnitType.Pressure),
                    new ChannelInfo(ChannelType.P1_Max                         , (int)ChannelType.P1_Max                   , "P1 Max"          , "", "#000000", UnitType.Pressure),
                    new ChannelInfo(ChannelType.P2_Min                         , (int)ChannelType.P2_Min                   , "P2 Min"          , "", "#000000", UnitType.Pressure),
                    new ChannelInfo(ChannelType.P2_Max                         , (int)ChannelType.P2_Max                   , "P2 Max"          , "", "#000000", UnitType.Pressure),

                    new ChannelInfo(ChannelType.FlowCalculation1               , (int)ChannelType.FlowCalculation1                   , "Flow Calculation 1"          , "", "#2196f3", UnitType.Flow),
                    new ChannelInfo(ChannelType.FlowCalculation2               , (int)ChannelType.FlowCalculation2                   , "Flow Calculation 2"          , "", "#005ca8", UnitType.Flow),
                    new ChannelInfo(ChannelType.FlowCalculation3               , (int)ChannelType.FlowCalculation3                   , "Flow Calculation 3"          , "", "#003866", UnitType.Flow),

                    new ChannelInfo(ChannelType.Calculation                    , (int)ChannelType.Calculation              , "Calculation"     , "Calculated Channel", "#000000", UnitType.Unknown)
        };
        }


        /// <summary>
        /// Please prefer GetCalculationChannelInfoInstance(ChannelType type) to auto-init members
        /// </summary>
        /// <returns>Default Calculation with unknown unit, color and name</returns>
        public static ChannelInfo GetCalculationChannelInfoInstance()
        {
            return new ChannelInfo(ChannelType.Calculation, (int)ChannelType.Calculation, "Calculation", "Calculated Channel", "#000000", UnitType.Unknown);
        }

        /// <summary>
        /// Please prefer GetCalculationChannelInfoInstance(ChannelType type) to auto-init members
        /// </summary>
        /// <returns>Default Calculation with unknown unit, color and name</returns>
        public static ChannelInfo GetCalculationChannelInfoInstance(string name, string description, string colorCode, UnitType unitType)
        {
            return new ChannelInfo(ChannelType.Calculation, (int)ChannelType.Calculation, name, description, colorCode, unitType);
        }

        public static ChannelInfo GetCalculationChannelInfoInstance(ChannelType type)
        {
            ChannelInfo targetChannelInfo = GetChannelInfo(type);

            // channelInfo.MeasurementDefinitionId, channelInfo.ChannelType SHOULD BE 100 for Calculation

            return GetCalculationChannelInfoInstance(targetChannelInfo.Name, targetChannelInfo.Description, targetChannelInfo.ColorCode, targetChannelInfo.UnitType);
        }

        public static ChannelInfo GetChannelInfo(int measurementDefinitionId)
        {
            try
            {
                return GetNewChannelsInstance().Single(_ => _.ChannelType == (ChannelType)measurementDefinitionId);
            }
            catch (Exception)
            {
                return GetNewChannelsInstance().SingleOrDefault(_ => _.ChannelType == ChannelType.Undefined);
            }
        }

        public static ChannelInfo GetChannelInfo(ChannelType channelType)
        {
            try
            {
                return GetNewChannelsInstance().Single(_ => _.ChannelType == channelType);
            }
            catch (Exception)
            {
                return GetNewChannelsInstance().SingleOrDefault(_ => _.ChannelType == ChannelType.Undefined);
            }
        }


        public static ChannelInfo[] GetValidatedChannels(ChannelInfo[] channelDefinitions)
        {
            var userChannelDefinition = channelDefinitions.ToList();

            foreach (ChannelInfo standardChannel in GetNewChannelsInstance())
            {
                if (userChannelDefinition.SingleOrDefault(chan => chan.ChannelType == standardChannel.ChannelType) == null)
                {
                    userChannelDefinition.Add(standardChannel);
                }
            }

            return userChannelDefinition.ToArray();
        }

        /// <summary>
        /// Gets the the name of the channel. Input: 11 -> Output: "Pd (P1-PBaro)"
        /// </summary>
        /// <param name="measurementDefinitionId"></param>
        /// <returns></returns>
        public static string GetMeasurementDefinitionName(int measurementDefinitionId)
        {
            return GetChannels()
                .Where(chn => chn.MeasurementDefinitionId == measurementDefinitionId)
                .Select(chn => chn.Name)
                .Single();
        }


        /// <summary>
        /// Gets the the name of the channel. Input: Pd_P1P2 -> Output: "Pd (P1-PBaro)"
        /// </summary>
        /// <param name="type">Input: Pd_P1P2</param>
        /// <returns>Output: "Pd (P1-PBaro)</returns>
        public static string GetMeasurementDefinitionName(ChannelType type)
        {
            try
            {
                return GetChannels()
                .Where(chn => chn.ChannelType == type)
                .Select(chn => chn.Name)
                .Single();
            }
            catch
            {
                return "-";
            }
        }

        /// <summary>
        /// Use like this:  ChannelInfo.GetMeasurementDefinitionName(setting.CalculationParameters[CalculationParameter.HydrostaticPressureMeasurementDefinitionId])
        /// </summary>
        /// <returns>gives something like "P1"</returns>
        public static string GetMeasurementDefinitionName(string calculationMeasurementIdText)
        {
            try
            {
                if (Int32.TryParse(calculationMeasurementIdText, out int id))
                {
                    ChannelType type = GetMeasurementDefinitionType(id);
                    return GetMeasurementDefinitionName(type);
                }
            }
            catch
            {
                return "-";
            }

            return calculationMeasurementIdText;
        }


        /// <summary>
        /// Gets the ChannelType of the id. Input: 11 -> Output: UnitType.Pressure
        /// </summary>
        /// <param name="measurementDefinitionId"></param>
        /// <returns></returns>
        public static ChannelType GetMeasurementDefinitionType(int measurementDefinitionId)
        {
            return GetChannels()
                .Where(chn => chn.MeasurementDefinitionId == measurementDefinitionId)
                .Select(chn => chn.ChannelType)
                .Single();
        }

        /// <summary>
        /// Gets the UnitType of the id. Input: 11 -> Output: UnitType.Pressure
        /// </summary>
        /// <param name="measurementDefinitionId"></param>
        /// <returns></returns>
        public static UnitType GetUnitType(int measurementDefinitionId)
        {
            return GetChannels()
                .Where(chn => chn.MeasurementDefinitionId == measurementDefinitionId)
                .Select(chn => chn.UnitType)
                .Single();
        }

        /// <summary>
        /// Gets the id of the type. Input: ChannelType.P1 -> Output: 2 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetMeasurementDefinitionId(ChannelType type)
        {
            return GetChannels()
                .Where(chn => chn.ChannelType == type)
                .Select(chn => chn.MeasurementDefinitionId)
                .Single();
        }

        /// <summary>
        /// Gets the color of the type. Input: ChannelType.P1 -> Output: "#107c10"
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetMeasurementDefinitionColorCode(ChannelType type)
        {
            return GetChannels()
                .Where(chn => chn.ChannelType == type)
                .Select(chn => chn.ColorCode)
                .Single();
        }
    }
}
