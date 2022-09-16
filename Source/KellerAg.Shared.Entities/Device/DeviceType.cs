using System.Diagnostics.CodeAnalysis;

namespace KellerAg.Shared.Entities.Device
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum DeviceType
    {
        Unknown,
        Castello,
        DCX22,
        DCX18ECO,
        DCX22AA,
        DCX_CTD,
        dV_2,
        dV_2Cool,
        dV_2PP,
        dV_2PS,
        dV_2_Radtke,
        ConverterK114,
        ConverterK114_M,
        ConverterK114_BT,
        LEO1_2,
        LeoVolvo,
        LeoIsler,
        LeoGuehring,
        LEO1x,
        LEO3,
        ECO1,
        Leo5,
        LeoRecord,
        Lex1,
        S30X,
        S30X2,
        S30X2_Cond,
        GSM1,
        GSM2,
        GSM3,
        /// <summary>
        /// ARC1 - Cellular
        /// </summary>
        ARC1,
        /// <summary>
        /// ADT1 - LoRa
        /// </summary>
        ADT1,
        /// <summary>
        /// ADT1 - Cellular
        /// </summary>
        ADT1_cellular,
        /// <summary>
        /// ARC1 - LoRa
        /// </summary>
        ARC1_lora,
        Bt_Transmitter,
    }
}
