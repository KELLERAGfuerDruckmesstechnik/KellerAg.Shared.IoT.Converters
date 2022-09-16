using System.Diagnostics.CodeAnalysis;

namespace KellerAg.Shared.Entities.Channel
{
    /// *** * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * ***
    /// *** WARNING: PLEASE KEEP THIS FILE IN SYNC WITH Entities.Data.MeasurementDefinition ***
    /// ***          and ChannelInfo[] GetChannels() and the SQL DB
    /// *** * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *** 
    /// 
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum ChannelType
    {
        Undefined = 0,
        Pd_P1P2 = 1,
        P1,
        P2,
        T,
        TOB1,
        TOB2,
        PBaro,
        TBaro,
        VoltIn1,
        VoltIn2,
        Pd_P1PBaro,
        ConductivityTc,
        ConductivityRaw,
        T_Conductivity,
        P1_2,
        P1_3,
        P1_4,
        P1_5,
        CounterIn,
        SDI12_CH1,
        SDI12_CH2,
        SDI12_CH3,
        SDI12_CH4,
        SDI12_CH5,
        SDI12_CH6,
        SDI12_CH7,
        SDI12_CH8,
        SDI12_CH9,
        SDI12_CH10,
        TOB1_2,
        TOB1_3,
        TOB1_4,
        TOB1_5,


        /// <summary>
        /// E: Height of Water calculation (MeasurementDefId:34)
        /// </summary>
        mH20_E = 34,

        /// <summary>
        /// F: Depth to Water calculation (MeasurementDefId:35)
        /// </summary>
        mH20_F,

        /// <summary>
        /// //G: Height of Water related to ASL  (MeasurementDefId:36)
        /// </summary>
        mH20_G,

        mH20_Pbaro,
        mH20_P1P2,
        mH20_P1P3,
        mH20_P1P4,
        mH20_P1P5 = 41,

        ConductivityTc_2,
        ConductivityTc_3,
        T_Conductivity_2,
        T_Conductivity_3,

        P2_2,
        TOB2_2 = 47,

        AquaMasterFlowRate = 48,
        AquaMasterPressure,
        AquaMasterCustomFlowUnits,
        AquaMasterExternalSupplyVoltage,

        TankContent1 = 52,
        TankContent2,
        TankContent3,

        MultiSensor1 = 55,
        MultiSensor2 = 56,
        MultiSensor3 = 57,
        MultiSensor4 = 58,
        MultiSensor5 = 59,

        Converter_Voltage_In = 60,
        Converter_Voltage_Out,
        Converter_Voltage_USB,
        Converter_Current_Out,

        P1_Min = 64,
        P1_Max,
        P2_Min,
        P2_Max,

        FlowCalculation1 = 68,
        FlowCalculation2,
        FlowCalculation3,

        /// <summary>
        /// Generic Calculation channel type used by the KELLER FileFormat to
        /// identify all kinds of calculation including water calculations
        /// </summary>
        Calculation = 100
    }
}
