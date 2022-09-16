using KellerAg.Shared.Entities.Device;
using System.Collections.Generic;

namespace KellerAg.Shared.Entities.Channel
{
    public static class ChannelTypesHelper
    {
        private static readonly ChannelType[] DefaultTypes = { ChannelType.Pd_P1P2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };

        public static ChannelType[] GetDeviceStandardChannelsStatic(DeviceType deviceType, int? ioTDeviceTypeId)
        {
            switch (deviceType)
            {
                case DeviceType.LeoRecord:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.P1_Min, ChannelType.P1_Max };
                case DeviceType.Leo5:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.P1_Min, ChannelType.P1_Max };
                case DeviceType.Bt_Transmitter:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case DeviceType.DCX22:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.CounterIn, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case DeviceType.DCX22AA:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case DeviceType.DCX18ECO:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case DeviceType.DCX_CTD:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.ConductivityTc, ChannelType.ConductivityRaw };
                case DeviceType.GSM1:
                case DeviceType.GSM2:
                case DeviceType.GSM3:
                case DeviceType.ARC1:
                case DeviceType.ARC1_lora:
                    return ioTDeviceTypeId.HasValue ? GetArcDeviceStandardChannelsStatic(ioTDeviceTypeId.Value) : DefaultTypes;
                case DeviceType.ADT1:
                case DeviceType.ADT1_cellular:
                    return ioTDeviceTypeId.HasValue ? GetAdtDeviceStandardChannelsStatic(ioTDeviceTypeId.Value) : DefaultTypes;
                default:
                    return null;
            }
        }

        private static ChannelType[] GetArcDeviceStandardChannelsStatic(int ioTDeviceTypeId)
        {
            switch (ioTDeviceTypeId)
            {
                case 0:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 1:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 2:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 3:
                    return new[] { ChannelType.Pd_P1PBaro, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 4:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.VoltIn1, ChannelType.VoltIn2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 5:
                    return new[] { ChannelType.Pd_P1PBaro, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.VoltIn1, ChannelType.VoltIn2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 6:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.VoltIn1, ChannelType.VoltIn2, ChannelType.P1_2, ChannelType.P1_3, ChannelType.P1_4, ChannelType.P1_5, ChannelType.CounterIn };
                case 7:
                    return new[] { ChannelType.Undefined, ChannelType.PBaro, ChannelType.TBaro, ChannelType.VoltIn1, ChannelType.VoltIn2, ChannelType.SDI12_CH1, ChannelType.SDI12_CH2, ChannelType.SDI12_CH3, ChannelType.SDI12_CH4, ChannelType.SDI12_CH5, ChannelType.SDI12_CH6, ChannelType.SDI12_CH7, ChannelType.SDI12_CH8, ChannelType.SDI12_CH9, ChannelType.SDI12_CH10 };
                case 8:
                    return new[] { ChannelType.P1, ChannelType.TOB1, ChannelType.P1_2, ChannelType.TOB1_2, ChannelType.P1_3, ChannelType.TOB1_3, ChannelType.P1_4, ChannelType.TOB1_4, ChannelType.P1_5, ChannelType.TOB1_5, ChannelType.VoltIn1, ChannelType.VoltIn2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.CounterIn };
                case 9:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T_Conductivity, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.VoltIn1, ChannelType.VoltIn2, ChannelType.ConductivityTc, ChannelType.ConductivityRaw, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 10:
                    return new[] { ChannelType.Pd_P1PBaro, ChannelType.P1, ChannelType.P2, ChannelType.T_Conductivity, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.VoltIn1, ChannelType.VoltIn2, ChannelType.ConductivityTc, ChannelType.ConductivityRaw, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 11:
                    return new[] { ChannelType.P1, ChannelType.TOB1, ChannelType.ConductivityTc, ChannelType.T_Conductivity, ChannelType.P1_2, ChannelType.TOB1_2, ChannelType.ConductivityTc_2, ChannelType.T_Conductivity_2, ChannelType.P1_3, ChannelType.TOB1_3, ChannelType.ConductivityTc_3, ChannelType.T_Conductivity_3, ChannelType.PBaro, ChannelType.TBaro, ChannelType.CounterIn };
                case 12:
                    return new[] { ChannelType.Pd_P1PBaro, ChannelType.P1, ChannelType.P2, ChannelType.T_Conductivity, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.VoltIn1, ChannelType.VoltIn2, ChannelType.AquaMasterFlowRate, ChannelType.AquaMasterPressure, ChannelType.AquaMasterCustomFlowUnits, ChannelType.AquaMasterExternalSupplyVoltage, ChannelType.CounterIn };
                case 13:
                    return new[] { ChannelType.P1, ChannelType.P2, ChannelType.TOB1, ChannelType.TOB2, ChannelType.P1_2, ChannelType.P2_2, ChannelType.TOB1_2, ChannelType.TOB2_2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.VoltIn1, ChannelType.VoltIn2, ChannelType.CounterIn, ChannelType.Undefined, ChannelType.Undefined };
                default:
                    return DefaultTypes;
            }
        }

        private static ChannelType[] GetAdtDeviceStandardChannelsStatic(int ioTDeviceTypeId)
        {
            switch (ioTDeviceTypeId)
            {
                case 0:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 1:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 2:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 3:
                    return new[] { ChannelType.Pd_P1PBaro, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 4:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 5:
                    return new[] { ChannelType.Pd_P1PBaro, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 6:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.P1_2, ChannelType.P1_3, ChannelType.P1_4, ChannelType.P1_5, ChannelType.Undefined };
                case 7:
                    return new[] { ChannelType.Undefined, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 8:
                    return new[] { ChannelType.P1, ChannelType.TOB1, ChannelType.P1_2, ChannelType.TOB1_2, ChannelType.P1_3, ChannelType.TOB1_3, ChannelType.P1_4, ChannelType.TOB1_4, ChannelType.P1_5, ChannelType.TOB1_5, ChannelType.Undefined, ChannelType.Undefined, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined };
                case 9:
                    return new[] { ChannelType.Pd_P1P2, ChannelType.P1, ChannelType.P2, ChannelType.T_Conductivity, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.ConductivityTc, ChannelType.ConductivityRaw, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 10:
                    return new[] { ChannelType.Pd_P1PBaro, ChannelType.P1, ChannelType.P2, ChannelType.T_Conductivity, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.ConductivityTc, ChannelType.ConductivityRaw, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 11:
                    return new[] { ChannelType.P1, ChannelType.TOB1, ChannelType.ConductivityTc, ChannelType.T_Conductivity, ChannelType.P1_2, ChannelType.TOB1_2, ChannelType.ConductivityTc_2, ChannelType.T_Conductivity_2, ChannelType.P1_3, ChannelType.TOB1_3, ChannelType.ConductivityTc_3, ChannelType.T_Conductivity_3, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined };
                case 12:
                    return new[] { ChannelType.Pd_P1PBaro, ChannelType.P1, ChannelType.P2, ChannelType.T_Conductivity, ChannelType.TOB1, ChannelType.TOB2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                case 13:
                    return new[] { ChannelType.P1, ChannelType.P2, ChannelType.TOB1, ChannelType.TOB2, ChannelType.P1_2, ChannelType.P2_2, ChannelType.TOB1_2, ChannelType.TOB2_2, ChannelType.PBaro, ChannelType.TBaro, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined, ChannelType.Undefined };
                default:
                    return DefaultTypes;
            }
        }

        public static List<ChannelType> NotPhysicalChannels => new List<ChannelType> { ChannelType.P1_Max, ChannelType.P1_Min, ChannelType.P2_Max, ChannelType.P2_Min, ChannelType.CounterIn };
    }
}
