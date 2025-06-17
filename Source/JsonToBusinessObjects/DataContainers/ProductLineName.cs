// ReSharper disable InconsistentNaming
namespace JsonToBusinessObjects.DataContainers
{
    /// <summary>
    /// Also see: https://wiki.keller-pressure.com/display/ORGET/Ph03-Review01-CLOUD1_W1
    /// </summary>
    public enum ProductLineName
    {
        GSM   = 0, //GSM-Line (2G)
        ARC1  = 1, //ARC-Line - Mobile (2G, 3G, 4G)
        //LoRaPOC unused
        ARC1_LoRa = 3, // ARC1 - withLoRa
        ADT1_LoRa = 4, // ADT1 - with LoRa
        ADT1_NBIoT_LTEM = 5  // ADT with NB-IoT or LTE-M
    }
}