using Entities.Data;

namespace KellerAg.Shared.IoT.Converters
{
    using System.Collections.Generic;
    using JsonToBusinessObjects.Conversion;
    using JsonToBusinessObjects.DataContainers;
    using KellerAg.Shared.LoRaPayloadConverter;

    public interface IConvert
    {
        /// <summary>
        /// Mobile Communication: From Device-Transmission to DTO: Step 1 (txt to json)
        /// </summary>
        /// <param name="gsmCommunicationText"></param>
        /// <returns></returns>
        string GsmCommunicationToJson(string gsmCommunicationText);

        /// <summary>
        /// Mobile Communication: From Device-Transmission to DTO: Step 2 (json to ConversionResult)
        /// </summary>
        /// <param name="gsmCommunicationJson"></param>
        /// <returns></returns>
        ConversionResult GsmCommunicationJsonToBusinessObject(string gsmCommunicationJson);

        /// <summary>
        /// Mobile/LoRa Communication: From Device-Transmission to DTO: When there is a device
        /// configuration then this can be converted further
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        DeviceSettings BusinessObjectToDeviceConfiguration(BusinessObjectRoot businessObject);

        /// <summary>
        /// Mobile Communication: From DTO to DeviceConfiguration-Transmission
        /// The DTO (DeviceSettings) has many nullable properties. Choose only the changing ones and let the others be null.
        /// </summary>
        /// <param name="deviceSettings"></param>
        /// <returns>Text content that can be used in a text file to store in a FTP folder to be read by a device. The device will update itself with this configuration parameters.</returns>
        string DeviceConfigurationToGsmCommunication(DeviceSettings deviceSettings);

        /// <summary>
        /// Mobile Communication: From serialized DTO to DeviceConfiguration-Transmission
        /// he DTO (DeviceSettings) has many nullable properties. Choose only the changing ones and let the others be null.
        /// </summary>
        /// <param name="deviceConfigurationDifferenceJson">The DTO (DeviceSettings) has to be serialized to be used. </param>
        /// <returns>Text content that can be used in a text file to store in a FTP folder to be read by a device. The device will update itself with this configuration parameters.</returns>
        string DeviceConfigurationToGsmCommunication(string deviceConfigurationDifferenceJson);

        /// <summary>
        /// LoRa Communication: From LoRaWAN-message to DTO
        /// Differs between TTN, Actility and Loriot transmission format
        /// Converts the message in a generalized object called "LoRaMessage" including the converted payload
        /// The LoRaMessage object is part of the BusinessObject
        /// </summary>
        /// <param name="jsonMessage"></param>
        /// <returns></returns>
        BusinessObjectRoot LoRaJsonMessageToBusinessObject(string jsonMessage);

        /// <summary>
        /// LoRa Communication
        /// A helper method used by LoRaJsonMessageToBusinessObject()
        /// </summary>
        /// <param name="loRaPayload"></param>
        /// <param name="port">This integer is only used to store it in the PayLoadInformation DTO but has no logic connected</param>
        /// <returns></returns>
        PayloadInformation LoRaPayloadToLoRaMessage(string loRaPayload, int port);

        /// <summary>
        /// LoRa Communication
        /// A helper method to show the decoded Payload data.
        /// For network: Choose either 0 (TTN), 1 (Actility based Network) or 2 (Loriot.io)
        /// </summary>
        /// <param name="loRaPayload"></param>
        /// <param name="port"></param>
        /// <param name="network">Choose either 0 (TTN), 1 (Actility based Network) or 2 (Loriot.io)</param>
        /// <returns></returns>
        PayloadInformation LoRaPayloadToLoRaMessage(string loRaPayload, int port, int network);

        /// <summary>
        /// LoRa Communication: From DTO to LoRaPayloads
        /// </summary>
        /// <param name="deviceConfigurationDifference"></param>
        /// <returns></returns>
        List<string> DeviceConfigurationToLoRaPayloads(DeviceSettings deviceConfigurationDifference);

        /// <summary>
        /// LoRa Communication: From DTO to LoRaPayloads
        /// </summary>
        /// <param name="deviceConfigurationDifferenceJson">DeviceSettings DTO serialized to JSON</param>
        /// <returns></returns>
        List<string> DeviceConfigurationToLoRaPayloads(string deviceConfigurationDifferenceJson);
    }
}
