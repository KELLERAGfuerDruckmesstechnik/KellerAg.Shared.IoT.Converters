// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using JsonToBusinessObjects.Conversion.LoRa;
//
//    var data = LoraMessageActility.FromJson(jsonString);
//
// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonToBusinessObjects.Conversion.LoRa
{
    public partial class LoraMessageActility
    {
        [JsonProperty("DevEUI_uplink")]
        public DevEUIUplink DevEUIUplink { get; set; }
    }

    public class DevEUIUplink
    {
        [JsonProperty("ADRbit")]
        public string ADRbit { get; set; }

        [JsonProperty("AppSKey")]
        public string AppSKey { get; set; }

        [JsonProperty("Channel")]
        public string Channel { get; set; }

        [JsonProperty("CustomerData")]
        public CustomerData CustomerData { get; set; }

        [JsonProperty("CustomerID")]
        public string CustomerID { get; set; }

        [JsonProperty("DevAddr")]
        public string DevAddr { get; set; }

        [JsonProperty("DevEUI")]
        public string DevEUI { get; set; }

        [JsonProperty("DevLrrCnt")]
        public string DevLrrCnt { get; set; }

        [JsonProperty("FCntDn")]
        public string FCntDn { get; set; }

        [JsonProperty("FCntUp")]
        public string FCntUp { get; set; }


        /// <summary>
        /// Actility: 1=Measurements ,  2=Alarms,  3=Config
        /// </summary>
        [JsonProperty("FPort")]
        public string FPort { get; set; }

        [JsonProperty("InstantPER")]
        public string InstantPER { get; set; }

        [JsonProperty("Late")]
        public string Late { get; set; }

        [JsonProperty("Lrcid")]
        public string Lrcid { get; set; }

        [JsonProperty("LrrRSSI")]
        public string LrrRSSI { get; set; }

        [JsonProperty("LrrSNR")]
        public string LrrSNR { get; set; }

        [JsonProperty("Lrrid")]
        public string Lrrid { get; set; }

        [JsonProperty("Lrrs")]
        public Lrrs Lrrs { get; set; }

        [JsonProperty("MType")]
        public string MType { get; set; }

        [JsonProperty("MeanPER")]
        public string MeanPER { get; set; }

        [JsonProperty("mic_hex")]
        public string MicHex { get; set; }

        [JsonProperty("ModelCfg")]
        public string ModelCfg { get; set; }

        [JsonProperty("payload_hex")]
        public string PayloadHex { get; set; }

        [JsonProperty("SpFact")]
        public string SpFact { get; set; }

        [JsonProperty("SubBand")]
        public string SubBand { get; set; }

        [JsonProperty("Time")]
        public string Time { get; set; }
    }

    public class Lrrs
    {
        [JsonProperty("Lrr")]
        public List<Lrr> Lrr { get; set; }
    }

    public class Lrr
    {
        [JsonProperty("Chain")]
        public string Chain { get; set; }

        [JsonProperty("LrrESP")]
        public string LrrESP { get; set; }

        [JsonProperty("LrrRSSI")]
        public string LrrRSSI { get; set; }

        [JsonProperty("LrrSNR")]
        public string LrrSNR { get; set; }

        [JsonProperty("Lrrid")]
        public string Lrrid { get; set; }
    }

    public class CustomerData
    {
        [JsonProperty("alr")]
        public Alr Alr { get; set; }

        [JsonProperty("loc")]
        public Loc Loc { get; set; }
    }

    public class Loc
    {
        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lon")]
        public string Lon { get; set; }
    }

    public class Alr
    {
        [JsonProperty("pro")]
        public string Pro { get; set; }

        [JsonProperty("ver")]
        public string Ver { get; set; }
    }

    public partial class LoraMessageActility
    {
        public static LoraMessageActility FromJson(string json) => JsonConvert.DeserializeObject<LoraMessageActility>(json, JsonConversionHelper.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this LoraMessageActility self) => JsonConvert.SerializeObject(self, JsonConversionHelper.Settings);
    }


}
