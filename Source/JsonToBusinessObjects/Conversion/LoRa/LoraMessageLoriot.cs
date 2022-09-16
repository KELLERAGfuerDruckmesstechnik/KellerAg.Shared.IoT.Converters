// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using JsonToBusinessObjects.Conversion.LoRa;
//
//    var data = LoraMessageLoriot.FromJson(jsonString);

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonToBusinessObjects.Conversion.LoRa
{
    /// <summary>
    /// See: https://docs.loriot.io/display/LNS/Uplink+Data+Message
    /// </summary>
    public partial class LoraMessageLoriot
    {

        /// <summary>
        /// cmd: rx, (rx-extended), gw
        /// </summary>
        [JsonProperty("cmd")]
        public string Cmd { get; set; }

        /// <summary>
        /// cmd: rx, (rx-extended), gw
        /// </summary>
        [JsonProperty("EUI")]
        public string Eui { get; set; }

        /// <summary>
        /// cmd: rx, (rx-extended), gw
        /// </summary>
        [JsonProperty("ts")]
        public double Ts { get; set; }

        /// <summary>
        /// cmd: rx, (rx-extended), gw
        /// </summary>
        [JsonProperty("ack")]
        public bool Ack { get; set; }

        /// <summary>
        /// cmd: rx, (rx-extended), gw
        /// </summary>
        [JsonProperty("fcnt")]
        public double Fcnt { get; set; }

        /// <summary>
        /// cmd: rx, (rx-extended), gw
        /// </summary>
        [JsonProperty("port")]
        public double Port { get; set; }

        /// <summary>
        /// cmd: rx, (rx-extended), gw
        /// data payload (APPSKEY encrypted hex string)
        /// only present if APPSKEY is not assigned to device
        /// </summary>
        [JsonProperty("encdata", Required = Required.Default/*not required*/)]
        public string EncData { get; set; }

        /// <summary>
        /// cmd: rx, (rx-extended), gw
        /// data payload (decrypted, plaintext hex string)
        /// only present if APPSKEY is assigned to device
        /// </summary>
        [JsonProperty("data", Required = Required.Default/*not required*/)]
        public string Data { get; set; }

        /// <summary>
        /// cmd: rx basic
        /// </summary>
        [JsonProperty("bat", Required = Required.Default/*not required*/)]
        public double? Bat { get; set; }

        /// <summary>
        /// rx-extended, gw
        /// in Hz
        /// </summary>
        [JsonProperty("freq", Required = Required.Default/*not required*/)]
        public double? Freq { get; set; }

        /// <summary>
        /// rx-extended, gw
        /// </summary>
        [JsonProperty("dr", Required = Required.Default/*not required*/)]
        public string Dr { get; set; }

        /// <summary>
        /// rx-extended
        /// in dBm (integer)
        /// </summary>
        [JsonProperty("rssi", Required = Required.Default/*not required*/)]
        public double? Rssi { get; set; }

        /// <summary>
        /// rx-extended
        /// in dB
        /// </summary>
        [JsonProperty("snr", Required = Required.Default/*not required*/)]
        public double? Snr { get; set; }

        /// <summary>
        /// ?
        /// </summary>
        [JsonProperty("toa", Required = Required.Default/*not required*/)]
        public double? Toa { get; set; }

        /// <summary>
        /// gw
        /// </summary>
        [JsonProperty("gws", Required = Required.Default/*not required*/)]
        public List<LoriotGateway> Gws { get; set; }

    }


    public class LoriotGateway
    {
        [JsonProperty("rssi", Required = Required.Default/*not required*/)]
        public double Rssi { get; set; }

        [JsonProperty("snr", Required = Required.Default/*not required*/)]
        public double Snr { get; set; }

        [JsonProperty("ts", Required = Required.Default/*not required*/)]
        public double Ts { get; set; }

        [JsonProperty("gweui", Required = Required.Default/*not required*/)]
        public string Gweui { get; set; }

        [JsonProperty("tmms", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default/*not required*/)]
        public double? Tmms { get; set; }

        [JsonProperty("time", Required = Required.Default/*not required*/)]
        public DateTimeOffset? Time { get; set; }

        [JsonProperty("ant", Required = Required.Default/*not required*/)]
        public double? Ant { get; set; }

        [JsonProperty("lat", Required = Required.Default/*not required*/)]
        public double? Lat { get; set; }

        [JsonProperty("lon", Required = Required.Default/*not required*/)]
        public double? Lon { get; set; }
    }

    public partial class LoraMessageLoriot
    {
        public static LoraMessageLoriot FromJson(string json) => JsonConvert.DeserializeObject<LoraMessageLoriot>(json, JsonConversionHelper.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this LoraMessageLoriot self) => JsonConvert.SerializeObject(self, JsonConversionHelper.Settings);
    }
}
