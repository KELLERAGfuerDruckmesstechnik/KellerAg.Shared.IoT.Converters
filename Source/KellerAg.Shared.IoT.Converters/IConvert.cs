namespace KellerAg.Shared.IoT.Converters
{
    using System.Collections.Generic;
    using JsonToBusinessObjects.Conversion;
    using JsonToBusinessObjects.DataContainers;
    using KellerAg.Shared.Entities.Database;
    using KellerAg.Shared.LoRaPayloadConverter;
    using Newtonsoft.Json.Linq;

    public interface IConvert
    {
        /// <summary>
        /// Mobile Communication: From Device-Transmission to DTO: Step 1 (txt to json text)
        /// </summary>
        /// <param name="gsmCommunicationText"></param>
        /// <returns>JSON text</returns>
        string GsmCommunicationToJson(string gsmCommunicationText);


        /// <summary>
        /// Mobile Communication: From Device-Transmission to DTO: Step 1 (txt to jObject)
        /// </summary>
        /// <param name="gsmCommunicationText"></param>
        /// <returns>Json as JObject</returns>
        JObject GsmCommunicationToJsonObject(string gsmCommunicationText);

        /// <summary>
        /// Mobile Communication: From Device-Transmission to DTO: Step 2 (JObject to ConversionResult)
        /// </summary>
        /// <param name="gsmCommunicationJson"></param>
        /// <returns></returns>
        ConversionResult GsmCommunicationJsonToBusinessObject(JObject gsmCommunicationJson);

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

        /// <summary>
        /// CSV Export: Converts measurement data to TXT1 format (TOPKAPI compatible).
        /// Each line contains the date/time (month/day/year hour:minute:second, e.g. 09/20/2004 00:00:00) followed by space-separated
        /// measurement values for each channel ordered by channel number.
        /// The first value is accessible in TOPKAPI using #F1, the second using #F2, etc.
        /// </summary>
        /// <param name="businessObjectRoot">The business object containing channel measurement data.</param>
        /// <returns>A string in TXT1 tabulated ASCII format.</returns>
        string BusinessObjectToTxt1(BusinessObjectRoot businessObjectRoot);

        /// <summary>
        /// CSV Export: Converts measurement data to TXT2 format (TOPKAPI compatible).
        /// Each line contains the timestamp (YYMMDDhhmmss), a variable identifier, and a single value.
        /// Values are accessible in TOPKAPI using #Fn or #En addresses.
        /// </summary>
        /// <param name="businessObjectRoot">The business object containing channel measurement data.</param>
        /// <param name="variableNames">
        /// Optional ordered list of variable identifiers for each channel (sorted by channel number).
        /// Each name must start with a letter and may only contain letters, numbers, underscores, and periods.
        /// Defaults to "CH{channelNumber}" when null or when fewer names than channels are provided.
        /// </param>
        /// <returns>A string in TXT2 tabulated ASCII format.</returns>
        string BusinessObjectToTxt2(BusinessObjectRoot businessObjectRoot, IReadOnlyList<string> variableNames = null);
    }
}
