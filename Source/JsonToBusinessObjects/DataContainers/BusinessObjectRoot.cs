namespace JsonToBusinessObjects.DataContainers
{
    using Configuration;
    using Conversion;

    public class BusinessObjectRoot
    {
        public int? AckNumber { get; set; }
        public ChannelDataStorage Measurements { get; set; }
        public TextContainer Texts { get; set; }
        public DeviceInformation DeviceInformation { get; set; }
        public MessageType MessageType { get; set; }
        public float[] CurrentValuesOfSelectedChannels { get; set; }
        public ConfigurationContainer Configuration { get; set; }
        public TimeContainer Time { get; set; }

        /// <summary>
        /// This is the data from a LoRa Message (both Actility and TTN). Other information are stored in Measurements,MessageType
        /// </summary>
        public Conversion.LoRa.LoRaMessage LoRaData { get; set; }
    }
}