using System;
using Newtonsoft.Json;

namespace JsonToBusinessObjects.Conversion.LoRa;

public partial class LoraMessageNetmore
{
    [JsonProperty("devEui")]
    public string DevEui { get; set; }

    [JsonProperty("sensorType")]
    public string SensorType { get; set; }

    [JsonProperty("messageType")]
    public string MessageType { get; set; }

    [JsonProperty("timestamp")]
    public DateTimeOffset Timestamp { get; set; }

    [JsonProperty("payload")]
    public string Payload { get; set; }

    [JsonProperty("fCntUp")]
    public long FCntUp { get; set; }

    [JsonProperty("toa")]
    public object Toa { get; set; }

    [JsonProperty("freq")]
    public long Freq { get; set; }

    [JsonProperty("batteryLevel")]
    public long BatteryLevel { get; set; }

    [JsonProperty("ack")]
    public bool Ack { get; set; }

    [JsonProperty("spreadingFactor")]
    public long SpreadingFactor { get; set; }

    [JsonProperty("dr")]
    public long Dr { get; set; }

    [JsonProperty("rssi")]
    public long Rssi { get; set; }

    [JsonProperty("snr")]
    public string Snr { get; set; }

    [JsonProperty("gatewayIdentifier")]
    public long GatewayIdentifier { get; set; }

    [JsonProperty("fPort")]
    public long FPort { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }

    [JsonProperty("tags")]
    public Tags Tags { get; set; }

    [JsonProperty("gateways")]
    public GatewayNetmore[] Gateways { get; set; }
}

public class GatewayNetmore
{
    [JsonProperty("rssi")]
    public long Rssi { get; set; }

    [JsonProperty("snr")]
    public string Snr { get; set; }

    [JsonProperty("gatewayIdentifier")]
    public long GatewayIdentifier { get; set; }

    [JsonProperty("gwEui")]
    public string GwEui { get; set; }

    [JsonProperty("mac")]
    public string Mac { get; set; }

    [JsonProperty("antenna")]
    public long Antenna { get; set; }
}

public class Tags
{
    [JsonProperty("building")]
    public string[] Building { get; set; }

    [JsonProperty("type")]
    public string[] Type { get; set; }
}

public partial class LoraMessageNetmore
{
    public static LoraMessageNetmore FromJson(string json) => JsonConvert.DeserializeObject<LoraMessageNetmore>(json, JsonConversionHelper.Settings);
}

public static partial class Serialize
{
    public static string ToJson(this LoraMessageNetmore self) => JsonConvert.SerializeObject(self, JsonConversionHelper.Settings);
}