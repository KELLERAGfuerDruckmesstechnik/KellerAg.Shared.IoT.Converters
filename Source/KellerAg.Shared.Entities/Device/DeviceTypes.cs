using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using KellerAg.Shared.Entities.Channel;

namespace KellerAg.Shared.Entities.Device
{
    //TODO: LURA: merge with DeviceInfo
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DeviceTypes : Dictionary<DeviceType, string>
    {
        //Pictures should be in \Resources\Devices\ preverably as jpg (faster than png) and with a resolution of 318x500 or same factor
        //Except Converter: They have 467x254
        public static DeviceTypes PictureFileName { get; } = new DeviceTypes()
        {
            { DeviceType.Unknown         , "dcx_22aaCombo.jpg"},
            { DeviceType.Castello        , "Castello.jpg"},
            { DeviceType.DCX22           , "dcx_22.jpg"},
            { DeviceType.DCX18ECO        , "dcx_18.jpg"},
            { DeviceType.DCX22AA         , "dcx_22aaCombo.jpg"},
            { DeviceType.DCX_CTD         , "dcx_22aaCombo.jpg"},
            { DeviceType.dV_2            , "dV-2.jpg"},
            { DeviceType.dV_2Cool        , "dV-2.jpg"},
            { DeviceType.dV_2PP          , "dV-22_PP.jpg"},
            { DeviceType.dV_2PS          , "dV-2_PS.jpg"},
            { DeviceType.dV_2_Radtke     , "dV-2.jpg"},
            { DeviceType.ConverterK114   , "k114.jpg"},
            { DeviceType.ConverterK114_M , "k114.jpg"},
            { DeviceType.ConverterK114_BT, "k114_bt.jpg"},
            { DeviceType.LEO1_2          , "LEO2.jpg"},
            { DeviceType.LeoVolvo        , "LEO1.jpg"},
            { DeviceType.LeoIsler        , "LEO1.jpg"},
            { DeviceType.LeoGuehring     , "LEO1.jpg"},
            { DeviceType.LEO1x           , "LEO1.jpg"},
            { DeviceType.LEO3            , "LEO3.jpg"},
            { DeviceType.ECO1            , "ECO1.jpg"},
            { DeviceType.Leo5            , "LEO5.jpg"},
            { DeviceType.LeoRecord       , "LEO_Record.jpg"},
            { DeviceType.Lex1            , "LEX1.jpg"},
            { DeviceType.S30X            , "36w.jpg"},
            { DeviceType.S30X2           , "36xiw.jpg"},
            { DeviceType.S30X2_Cond      , "36xiw_ctd.jpg"},
            { DeviceType.ADT1            , "ADT1.jpg"},
            { DeviceType.ADT1_cellular   , "ADT1.jpg"},
            { DeviceType.ARC1            , "ARC1.jpg"},
            { DeviceType.ARC1_lora       , "ARC1.jpg"},
            { DeviceType.GSM1            , "ARC1.jpg"},
            { DeviceType.GSM2            , "ARC1.jpg"},
            { DeviceType.GSM3            , "ARC1.jpg"},
            //TODO: BtTransmitter
            { DeviceType.Bt_Transmitter       , "BT_Transmitter.jpg"},
        };


        private static DeviceTypes ProductUrl_EN { get; } = new DeviceTypes()
        {
            { DeviceType.Unknown         , "http://www.keller-druck.com/home_e/prod_hm_e.asp"},
            { DeviceType.Castello        , "http://www.keller-druck.com/home_e/paprod_e/castello_e.asp"},
            { DeviceType.DCX22           , "http://www.keller-druck.com/home_e/paprod_e/dcx22_e.asp"},
            { DeviceType.DCX18ECO        , "http://www.keller-druck.com/home_e/paprod_e/dcx18_e.asp"},
            { DeviceType.DCX22AA         , "http://www.keller-druck.com/home_e/paprod_e/dcx22aa_e.asp"},
            { DeviceType.DCX_CTD         , "http://www.keller-druck.com/home_e/paprod_e/dcx22aa_ctd_e.asp"},
            { DeviceType.dV_2            , "http://www.keller-druck.com/home_e/paprod_e/dV2_e.asp"},
            { DeviceType.dV_2Cool        , "http://www.keller-druck.com/home_e/paprod_e/dv2cool_e.asp"},
            { DeviceType.dV_2PP          , "http://www.keller-druck.com/home_e/paprod_e/pa22ps_e.asp"},
            { DeviceType.dV_2PS          , "http://www.keller-druck.com/home_e/paprod_e/hm_switches_e.asp"},
            { DeviceType.dV_2_Radtke     , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.ConverterK114   , "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.ConverterK114_M , "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.ConverterK114_BT, "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.LEO1_2          , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoVolvo        , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoIsler        , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoGuehring     , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LEO1x           , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LEO3            , "http://www.keller-druck.com/home_e/paprod_e/leo3_e.asp"},
            { DeviceType.ECO1            , "http://www.keller-druck.com/home_e/paprod_e/eco1_e.asp"},
            { DeviceType.Leo5            , "http://www.keller-druck.com/home_e/paprod_e/leo5_e.asp"},
            { DeviceType.LeoRecord       , "http://www.keller-druck.com/home_e/paprod_e/leorec_e.asp"},
            { DeviceType.Lex1            , "http://www.keller-druck.com/home_e/paprod_e/lex1_e.asp"},
            { DeviceType.S30X            , "http://www.keller-druck.com/home_e/paprod_e/hm_level_e.asp"},
            { DeviceType.S30X2           , "http://www.keller-druck.com/home_e/paprod_e/hm_level_e.asp"},
            { DeviceType.S30X2_Cond      , "http://www.keller-druck.com/home_e/paprod_e/36xiwctd_e.asp"},
            { DeviceType.ADT1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ADT1_cellular   , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ARC1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.ARC1_lora       , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM2            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM3            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            //TODO: BtTransmitter
            { DeviceType.Bt_Transmitter  , "http://www.keller-druck.com/home_e/paprod_e/leorec_e.asp"},
        };



        private static DeviceTypes ProductUrl_DE { get; } = new DeviceTypes()
        {
            { DeviceType.Unknown         , "http://www.keller-druck.com/home_g/prod_hm_g.asp"},
            { DeviceType.Castello        , "http://www.keller-druck.com/home_g/paprod_g/castello_g.asp"},
            { DeviceType.DCX22           , "http://www.keller-druck.com/home_g/paprod_g/dcx22_g.asp"},
            { DeviceType.DCX18ECO        , "http://www.keller-druck.com/home_g/paprod_g/dcx18_g.asp"},
            { DeviceType.DCX22AA         , "http://www.keller-druck.com/home_g/paprod_g/dcx22aa_g.asp"},
            { DeviceType.DCX_CTD         , "http://www.keller-druck.com/home_g/paprod_g/dcx22aa_ctd_g.asp"},
            { DeviceType.dV_2            , "http://www.keller-druck.com/home_g/paprod_g/dV2_g.asp"},
            { DeviceType.dV_2Cool        , "http://www.keller-druck.com/home_g/paprod_g/dv2cool_g.asp"},
            { DeviceType.dV_2PP          , "http://www.keller-druck.com/home_g/paprod_g/pa22ps_g.asp"},
            { DeviceType.dV_2PS          , "http://www.keller-druck.com/home_g/paprod_g/hm_switches_g.asp"},
            { DeviceType.dV_2_Radtke     , "http://www.keller-druck.com/home_g/paprod_g/hm_manos_g.asp"},
            { DeviceType.ConverterK114   , "http://www.keller-druck.com/home_g/paprod_g/converters_g.asp"},
            { DeviceType.ConverterK114_M , "http://www.keller-druck.com/home_g/paprod_g/converters_g.asp"},
            { DeviceType.ConverterK114_BT, "http://www.keller-druck.com/home_g/paprod_g/converters_g.asp"},
            { DeviceType.LEO1_2          , "http://www.keller-druck.com/home_g/paprod_g/hm_manos_g.asp"},
            { DeviceType.LeoVolvo        , "http://www.keller-druck.com/home_g/paprod_g/hm_manos_g.asp"},
            { DeviceType.LeoIsler        , "http://www.keller-druck.com/home_g/paprod_g/hm_manos_g.asp"},
            { DeviceType.LeoGuehring     , "http://www.keller-druck.com/home_g/paprod_g/hm_manos_g.asp"},
            { DeviceType.LEO1x           , "http://www.keller-druck.com/home_g/paprod_g/hm_manos_g.asp"},
            { DeviceType.LEO3            , "http://www.keller-druck.com/home_g/paprod_g/leo3_g.asp"},
            { DeviceType.ECO1            , "http://www.keller-druck.com/home_g/paprod_g/eco1_g.asp"},
            { DeviceType.Leo5            , "http://www.keller-druck.com/home_g/paprod_g/leo5_g.asp"},
            { DeviceType.LeoRecord       , "http://www.keller-druck.com/home_g/paprod_g/leorec_g.asp"},
            { DeviceType.Lex1            , "http://www.keller-druck.com/home_g/paprod_g/lex1_g.asp"},
            { DeviceType.S30X            , "http://www.keller-druck.com/home_g/paprod_g/hm_level_g.asp"},
            { DeviceType.S30X2           , "http://www.keller-druck.com/home_g/paprod_g/hm_level_g.asp"},
            { DeviceType.S30X2_Cond      , "http://www.keller-druck.com/home_g/paprod_g/36xiwctd_g.asp"},
            { DeviceType.ADT1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ADT1_cellular   , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ARC1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.ARC1_lora            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM2            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM3            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            //TODO: BtTransmitter
            { DeviceType.Bt_Transmitter  , "http://www.keller-druck.com/home_e/paprod_e/leorec_e.asp"},
        };


        //todo es
        private static DeviceTypes ProductUrl_ES { get; } = new DeviceTypes()
        {
            { DeviceType.Unknown         , "http://www.keller-druck.es/es/productos"},
            { DeviceType.Castello        , "http://www.keller-druck.es/es/productos/indicadores_digitales/castello"},
            { DeviceType.DCX22           , "http://www.keller-druck.com/home_e/paprod_e/dcx22_e.asp"},
            { DeviceType.DCX18ECO        , "http://www.keller-druck.com/home_e/paprod_e/dcx18_e.asp"},
            { DeviceType.DCX22AA         , "http://www.keller-druck.com/home_e/paprod_e/dcx22aa_e.asp"},
            { DeviceType.DCX_CTD         , "http://www.keller-druck.com/home_e/paprod_e/dcx22aa_ctd_e.asp"},
            { DeviceType.dV_2            , "http://www.keller-druck.com/home_e/paprod_e/dV2_e.asp"},
            { DeviceType.dV_2Cool        , "http://www.keller-druck.com/home_e/paprod_e/dv2cool_e.asp"},
            { DeviceType.dV_2PP          , "http://www.keller-druck.com/home_e/paprod_e/pa22ps_e.asp"},
            { DeviceType.dV_2PS          , "http://www.keller-druck.com/home_e/paprod_e/hm_switches_e.asp"},
            { DeviceType.dV_2_Radtke     , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.ConverterK114   , "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.ConverterK114_M , "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.ConverterK114_BT, "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.LEO1_2          , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoVolvo        , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoIsler        , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoGuehring     , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LEO1x           , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LEO3            , "http://www.keller-druck.com/home_e/paprod_e/leo3_e.asp"},
            { DeviceType.ECO1            , "http://www.keller-druck.com/home_e/paprod_e/eco1_e.asp"},
            { DeviceType.Leo5            , "http://www.keller-druck.com/home_e/paprod_e/leo5_e.asp"},
            { DeviceType.LeoRecord       , "http://www.keller-druck.com/home_e/paprod_e/leorec_e.asp"},
            { DeviceType.Lex1            , "http://www.keller-druck.com/home_e/paprod_e/lex1_e.asp"},
            { DeviceType.S30X            , "http://www.keller-druck.com/home_e/paprod_e/hm_level_e.asp"},
            { DeviceType.S30X2           , "http://www.keller-druck.com/home_e/paprod_e/hm_level_e.asp"},
            { DeviceType.S30X2_Cond      , "http://www.keller-druck.com/home_e/paprod_e/36xiwctd_e.asp"},
            { DeviceType.ADT1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ADT1_cellular   , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ARC1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.ARC1_lora       , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM2            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM3            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            //TODO: BtTransmitter
            { DeviceType.Bt_Transmitter  , "http://www.keller-druck.com/home_e/paprod_e/leorec_e.asp"},
        };

        //todo pt
        private static DeviceTypes ProductUrl_PT { get; } = new DeviceTypes()
        {
            { DeviceType.Unknown         , "http://www.keller-druck.pt/pt/produtos"},
            { DeviceType.Castello        , "http://www.keller-druck.pt/pt/produtos/displays_digitais/castello"},
            { DeviceType.DCX22           , "http://www.keller-druck.com/home_e/paprod_e/dcx22_e.asp"},
            { DeviceType.DCX18ECO        , "http://www.keller-druck.com/home_e/paprod_e/dcx18_e.asp"},
            { DeviceType.DCX22AA         , "http://www.keller-druck.com/home_e/paprod_e/dcx22aa_e.asp"},
            { DeviceType.DCX_CTD         , "http://www.keller-druck.com/home_e/paprod_e/dcx22aa_ctd_e.asp"},
            { DeviceType.dV_2            , "http://www.keller-druck.com/home_e/paprod_e/dV2_e.asp"},
            { DeviceType.dV_2Cool        , "http://www.keller-druck.com/home_e/paprod_e/dv2cool_e.asp"},
            { DeviceType.dV_2PP          , "http://www.keller-druck.com/home_e/paprod_e/pa22ps_e.asp"},
            { DeviceType.dV_2PS          , "http://www.keller-druck.com/home_e/paprod_e/hm_switches_e.asp"},
            { DeviceType.dV_2_Radtke     , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.ConverterK114   , "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.ConverterK114_M , "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.ConverterK114_BT, "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.LEO1_2          , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoVolvo        , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoIsler        , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoGuehring     , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LEO1x           , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LEO3            , "http://www.keller-druck.com/home_e/paprod_e/leo3_e.asp"},
            { DeviceType.ECO1            , "http://www.keller-druck.com/home_e/paprod_e/eco1_e.asp"},
            { DeviceType.Leo5            , "http://www.keller-druck.com/home_e/paprod_e/leo5_e.asp"},
            { DeviceType.LeoRecord       , "http://www.keller-druck.com/home_e/paprod_e/leorec_e.asp"},
            { DeviceType.Lex1            , "http://www.keller-druck.com/home_e/paprod_e/lex1_e.asp"},
            { DeviceType.S30X            , "http://www.keller-druck.com/home_e/paprod_e/hm_level_e.asp"},
            { DeviceType.S30X2           , "http://www.keller-druck.com/home_e/paprod_e/hm_level_e.asp"},
            { DeviceType.S30X2_Cond      , "http://www.keller-druck.com/home_e/paprod_e/36xiwctd_e.asp"},
            { DeviceType.ADT1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ADT1_cellular   , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ARC1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.ARC1_lora       , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM2            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM3            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            //TODO: BtTransmitter
            { DeviceType.Bt_Transmitter  , "http://www.keller-druck.com/home_e/paprod_e/leorec_e.asp"},
        };

        //todo fr
        private static DeviceTypes ProductUrl_FR { get; } = new DeviceTypes()
        {
            { DeviceType.Unknown         , "http://www.keller-druck.com/home_f/prod_hm_f.asp"},
            { DeviceType.Castello        , "http://www.keller-druck.com/home_f/paprod_f/castello_f.asp"},
            { DeviceType.DCX22           , "http://www.keller-druck.com/home_e/paprod_e/dcx22_e.asp"},
            { DeviceType.DCX18ECO        , "http://www.keller-druck.com/home_e/paprod_e/dcx18_e.asp"},
            { DeviceType.DCX22AA         , "http://www.keller-druck.com/home_e/paprod_e/dcx22aa_e.asp"},
            { DeviceType.DCX_CTD         , "http://www.keller-druck.com/home_e/paprod_e/dcx22aa_ctd_e.asp"},
            { DeviceType.dV_2            , "http://www.keller-druck.com/home_e/paprod_e/dV2_e.asp"},
            { DeviceType.dV_2Cool        , "http://www.keller-druck.com/home_e/paprod_e/dv2cool_e.asp"},
            { DeviceType.dV_2PP          , "http://www.keller-druck.com/home_e/paprod_e/pa22ps_e.asp"},
            { DeviceType.dV_2PS          , "http://www.keller-druck.com/home_e/paprod_e/hm_switches_e.asp"},
            { DeviceType.dV_2_Radtke     , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.ConverterK114   , "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.ConverterK114_M , "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.ConverterK114_BT, "http://www.keller-druck.com/home_e/paprod_e/converters_e.asp"},
            { DeviceType.LEO1_2          , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoVolvo        , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoIsler        , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LeoGuehring     , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LEO1x           , "http://www.keller-druck.com/home_e/paprod_e/hm_manos_e.asp"},
            { DeviceType.LEO3            , "http://www.keller-druck.com/home_e/paprod_e/leo3_e.asp"},
            { DeviceType.ECO1            , "http://www.keller-druck.com/home_e/paprod_e/eco1_e.asp"},
            { DeviceType.Leo5            , "http://www.keller-druck.com/home_e/paprod_e/leo5_e.asp"},
            { DeviceType.LeoRecord       , "http://www.keller-druck.com/home_e/paprod_e/leorec_e.asp"},
            { DeviceType.Lex1            , "http://www.keller-druck.com/home_e/paprod_e/lex1_e.asp"},
            { DeviceType.S30X            , "http://www.keller-druck.com/home_e/paprod_e/hm_level_e.asp"},
            { DeviceType.S30X2           , "http://www.keller-druck.com/home_e/paprod_e/hm_level_e.asp"},
            { DeviceType.S30X2_Cond      , "http://www.keller-druck.com/home_e/paprod_e/36xiwctd_e.asp"},
            { DeviceType.ADT1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ADT1_cellular   , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/adt1-tube"},
            { DeviceType.ARC1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.ARC1_lora       , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM1            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM2            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            { DeviceType.GSM3            , "http://keller-druck.com/en/products/data-loggers/remote-transmission-units-with-data-logger/arc1-tube"},
            //TODO: BtTransmitter
            { DeviceType.Bt_Transmitter  , "http://www.keller-druck.com/home_e/paprod_e/leorec_e.asp"},
        };

        public static DeviceTypes ProductUrl(languages language = languages.en)
        {
            switch (language)
            {
                case languages.de:
                    return ProductUrl_DE;
                case languages.en:
                    return ProductUrl_EN;
                case languages.fr:
                    return ProductUrl_FR;
                case languages.es:
                    return ProductUrl_ES;
                case languages.pt:
                    return ProductUrl_PT;
                default:
                    return ProductUrl_EN;
            }
        }

        public static bool IsConverter(DeviceType type) => type == DeviceType.ConverterK114 ||
                                                           type == DeviceType.ConverterK114_BT ||
                                                           type == DeviceType.ConverterK114_M;

        public static bool IsIotDevice(DeviceType type) => IsLoRaDevice(type) ||
                                                           IsCellularDevice(type);
        public static bool IsLoggerDevice(DeviceType type) => type == DeviceType.LeoRecord ||
                                                           type == DeviceType.Leo5 ||
                                                           type == DeviceType.DCX18ECO ||
                                                           type == DeviceType.DCX22 ||
                                                           type == DeviceType.DCX22AA ||
                                                           type == DeviceType.DCX_CTD ||
                                                           IsIotDevice(type);

        public static bool IsLoRaDevice(DeviceType type) => type == DeviceType.ARC1_lora ||
                                                            type == DeviceType.ADT1;

        public static bool IsCellularDevice(DeviceType type) => type == DeviceType.GSM1 ||
                                                            type == DeviceType.GSM2 ||
                                                            type == DeviceType.GSM3 ||
                                                            type == DeviceType.ARC1 ||
                                                            type == DeviceType.ADT1_cellular;

        public static bool IsCDTDevice(IDevice device)
        {
            bool? cdtChannelsEnabled = device?.DeviceInfo?.ChannelTypes?.Any(
                                                            channel => channel == ChannelType.ConductivityRaw ||
                                                            channel == ChannelType.ConductivityTc ||
                                                            channel == ChannelType.ConductivityTc_2 ||
                                                            channel == ChannelType.ConductivityTc_3 ||
                                                            channel == ChannelType.T_Conductivity ||
                                                            channel == ChannelType.T_Conductivity_2 ||
                                                            channel == ChannelType.T_Conductivity_3);
                                                            // ABB AquaMaster would probably also need a long delay

            return cdtChannelsEnabled.HasValue && cdtChannelsEnabled.Value;
        }

        // Zero is only available for Manometers
        public static bool HasZeroFunction(DeviceType type) => type == DeviceType.LeoRecord ||
                                                               type == DeviceType.Leo5 ||
                                                               type == DeviceType.LEO1_2 ||
                                                               type == DeviceType.LEO1x ||
                                                               type == DeviceType.LEO3 ||
                                                               type == DeviceType.LeoIsler ||
                                                               type == DeviceType.LeoGuehring ||
                                                               type == DeviceType.LeoVolvo ||
                                                               type == DeviceType.Lex1;

        //TODO: check with marcel if this could be read from the device instead of hardcode
        public static int DevicePageSize(DeviceType type) => type == DeviceType.Bt_Transmitter ? 256 : 64;

        public enum languages
        {
            de,
            en,
            fr,
            pt,
            es
        }
    }
}
