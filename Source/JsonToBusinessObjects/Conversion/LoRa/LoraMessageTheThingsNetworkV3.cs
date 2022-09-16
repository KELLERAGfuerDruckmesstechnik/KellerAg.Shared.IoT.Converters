namespace JsonToBusinessObjects.Conversion.LoRa
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using J = Newtonsoft.Json.JsonPropertyAttribute;
    using R = Newtonsoft.Json.Required;
    using N = Newtonsoft.Json.NullValueHandling;

    public partial class LoraMessageTheThingsNetworkV3
    {
        [J("end_device_ids")]  public EndDeviceIds EndDeviceIds { get; set; }

        /// <summary>
        /// Correlation identifiers of the message
        /// </summary>
        [J("correlation_ids")] public List<string> CorrelationIds { get; set; }

        /// <summary>
        /// ISO 8601 UTC timestamp at which the message has been received by the Application Server
        /// </summary>
        [J("received_at")]     public DateTimeOffset ReceivedAt { get; set; }
        [J("uplink_message")]  public UplinkMessage UplinkMessage { get; set; }
    }

    public class EndDeviceIds
                {
        [J("device_id")]       public string DeviceId { get; set; }
        [J("application_ids")] public ApplicationIds ApplicationIds { get; set; }

        /// <summary>
        /// DevEUI of the end device
        /// </summary>
        [J("dev_eui")]         public string DevEui { get; set; }

        /// <summary>
        /// JoinEUI of the end device (also known as AppEUI in LoRaWAN versions below 1.1)
        /// </summary>
        [J("join_eui")]        public string JoinEui { get; set; }

        /// <summary>
        /// Device address known by the Network Server
        /// </summary>
        [J("dev_addr")]        public string DevAddr { get; set; }
    }

    public class ApplicationIds
    {
        [J("application_id")] public string ApplicationId { get; set; }
    }

    public class UplinkMessage
    {
        /// <summary>
        /// // Join Server issued identifier for the session keys used by this uplink
        /// </summary>
        [J("session_key_id")]   public string SessionKeyId { get; set; }
        [J("f_port")]           public int FPort { get; set; }
        [J("f_cnt")]            public int FCnt { get; set; }

        /// <summary>
        /// Frame payload (Base64)
        /// </summary>
        [J("frm_payload")]      public string FrmPayload { get; set; }

        /// <summary>
        /// Decoded payload object, decoded by the device payload formatter
        /// </summary>
        [J("decoded_payload")]  public JToken DecodedPayload { get; set; }

        /// <summary>
        /// A list of metadata for each antenna of each gateway that received this message
        /// </summary>
        [J("rx_metadata")]      public List<RxMetadatum> RxMetadata { get; set; }

        /// <summary>
        /// Settings for the transmission
        /// </summary>
        [J("settings")]         public Settings Settings { get; set; }
        [J("received_at")]      public DateTimeOffset ReceivedAt { get; set; }
        [J("consumed_airtime")] public string ConsumedAirtime { get; set; }
        [J("locations")]        public Locations Locations { get; set; }
    }

    public class DecodedPayload
    {
        [J("P1")]           public double P1 { get; set; }
        [J("PBaro")]        public double PBaro { get; set; }
        [J("T")]            public double T { get; set; }
        [J("TBaro")]        public double TBaro { get; set; }
        [J("TOB1")]         public double Tob1 { get; set; }

        // all of them ? Better make it generic
        // plus info

        [J("channel")]      public string Channel { get; set; }
        [J("channelCount")] public long ChannelCount { get; set; }
        [J("ct")]           public long Ct { get; set; }
        [J("func")]         public long Func { get; set; }
        [J("payload")]      public string Payload { get; set; }
        [J("port")]         public long Port { get; set; }
    }

    public class Locations
    {
        [J("user")]         public User User { get; set; }
    }

    public class User
    {
        [J("latitude")]  public double Latitude { get; set; }
        [J("longitude")] public double Longitude { get; set; }
        [J("altitude")]  public double Altitude { get; set; }
        [J("source")]    public string Source { get; set; }
    }

    public class RxMetadatum
    {
        [J("gateway_ids")]     public GatewayIds GatewayIds { get; set; }
        [J("time")]            public DateTimeOffset Time { get; set; }
        [J("timestamp", NullValueHandling = N.Ignore)]   public long? Timestamp { get; set; }
        [J("rssi")]            public int Rssi { get; set; }
        [J("channel_rssi")]    public int ChannelRssi { get; set; }
        [J("snr")]             public double Snr { get; set; }
        [J("location", NullValueHandling = N.Ignore)] public User Location { get; set; }
        [J("uplink_token")]    public string UplinkToken { get; set; }
        [J("packet_broker", NullValueHandling = N.Ignore)] public PacketBroker PacketBroker { get; set; }
    }

    public class GatewayIds
    {
        [J("gateway_id")]                        public string GatewayId { get; set; }
        [J("eui", NullValueHandling = N.Ignore)] public string Eui { get; set; }
    }

    public class PacketBroker
    {
        [J("message_id")]              public string MessageId { get; set; }
        [J("forwarder_net_id")]        public string ForwarderNetId { get; set; }
        [J("forwarder_tenant_id")]     public string ForwarderTenantId { get; set; }
        [J("forwarder_cluster_id")]    public string ForwarderClusterId { get; set; }
        [J("home_network_net_id")]     public string HomeNetworkNetId { get; set; }
        [J("home_network_tenant_id")]  public string HomeNetworkTenantId { get; set; }
        [J("home_network_cluster_id")] public string HomeNetworkClusterId { get; set; }
        [J("hops")]                    public List<Hop> Hops { get; set; }
    }

    public class Hop
    {
        [J("received_at")]    public DateTimeOffset ReceivedAt { get; set; }
        [J("sender_address")] public string SenderAddress { get; set; }
        [J("receiver_name")]  public string ReceiverName { get; set; }
        [J("receiver_agent")] public string ReceiverAgent { get; set; }
        [J("sender_name", NullValueHandling = N.Ignore)] public string SenderName { get; set; }
    }

    public class Settings
    {
        [J("data_rate")]       public DataRate DataRate { get; set; }
        [J("data_rate_index")] public int DataRateIndex { get; set; }
        [J("coding_rate")]     public string CodingRate { get; set; }
        [J("frequency")]       public int Frequency { get; set; }
        [J("timestamp")]       public long Timestamp { get; set; }
        [J("time")]            public DateTimeOffset Time { get; set; }
    }

    public class DataRate
    {
        [J("lora")] public Lora Lora { get; set; }
    }

    public class Lora
    {
        [J("bandwidth")]        public int Bandwidth { get; set; }
        [J("spreading_factor")] public int SpreadingFactor { get; set; }
    }

    public partial class LoraMessageTheThingsNetworkV3
    {
        public static LoraMessageTheThingsNetworkV3 FromJson(string json) => JsonConvert.DeserializeObject<LoraMessageTheThingsNetworkV3>(json, JsonConversionHelper.Settings);
    }

    public static partial class Serialize
    {
        public static string ToJson(this LoraMessageTheThingsNetworkV3 self) => JsonConvert.SerializeObject(self, JsonConversionHelper.Settings);
    }
}