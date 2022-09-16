namespace KellerAg.Shared.LoRaPayloadConverter
{
    public interface IPayloadConverter
    {
        /// <summary>
        /// Downstream messages like 'Settings' going down to the device
        /// </summary>
        /// <param name="downstreamInfo"></param>
        /// <returns></returns>
        string Convert(PayloadInformation downstreamInfo);


        /// <summary>
        /// Actility Upstream messages like 'Measurement' going from the devices to the cloud
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="port">Port number</param>
        /// <returns></returns>
        PayloadInformation ConvertFromActility(string payload, int port);

        /// <summary>
        /// TTN Upstream messages like 'Measurement' going from the devices to the cloud
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="port">Port number</param>
        /// <returns></returns>
        PayloadInformation ConvertFromTheThingNetwork(string payload, int port);

        /// <summary>
        /// Loriot Upstream messages like 'Measurement' going from the devices to the cloud
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="port">Port number</param>
        /// <returns></returns>
        PayloadInformation ConvertFromLoriot(string payload, int port);
    }
}