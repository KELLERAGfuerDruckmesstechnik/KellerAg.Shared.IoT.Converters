using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonToBusinessObjects.Conversion.LoRa
{
    public partial class LoraMessageTheThingsNetwork
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("counter")]
        public long Counter { get; set; }

        [JsonProperty("dev_id")]
        public string DevId { get; set; }

        [JsonProperty("downlink_url")]
        public string DownlinkUrl { get; set; }

        [JsonProperty("hardware_serial")]
        public string HardwareSerial { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("payload_fields")]
        public object PayloadFields { get; set; }

        [JsonProperty("payload_raw")]
        public string PayloadRaw { get; set; }

        [JsonProperty("port")]
        public long Port { get; set; }
    }


    /// <summary>
    /// optional fields
    /// </summary>
    public class PayloadFields
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("Channel_1")]
        public double Channel1 { get; set; }

        [JsonProperty("Channel_2")]
        public double Channel2 { get; set; }

        [JsonProperty("Channel_3")]
        public double Channel3 { get; set; }

        [JsonProperty("Channel_4")]
        public double Channel4 { get; set; }

        [JsonProperty("ct")]
        public long Ct { get; set; }

        [JsonProperty("func")]
        public long Func { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("coding_rate")]
        public string CodingRate { get; set; }

        [JsonProperty("data_rate")]
        public string DataRate { get; set; }

        [JsonProperty("frequency")]
        public double Frequency { get; set; }

        [JsonProperty("gateways")]
        public List<Gateway> Gateways { get; set; }

        [JsonProperty("modulation")]
        public string Modulation { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }

    public class Gateway
    {
        [JsonProperty("channel")]
        public long Channel { get; set; }

        [JsonProperty("gtw_id")]
        public string GtwId { get; set; }

        [JsonProperty("gtw_trusted")]
        public bool? GtwTrusted { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("rssi")]
        public double Rssi { get; set; }

        [JsonProperty("snr")]
        public double Snr { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }

    public partial class LoraMessageTheThingsNetwork
    {
        public static LoraMessageTheThingsNetwork FromJson(string json) => JsonConvert.DeserializeObject<LoraMessageTheThingsNetwork>(json, JsonConversionHelper.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this LoraMessageTheThingsNetwork self) => JsonConvert.SerializeObject(self, JsonConversionHelper.Settings);
    }
}
