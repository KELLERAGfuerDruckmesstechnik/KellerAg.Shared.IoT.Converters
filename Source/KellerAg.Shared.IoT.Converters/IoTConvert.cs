namespace KellerAg.Shared.IoT.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DeviceConfigurationToGsmCommunication;
    using DeviceConfigurationToPayloadInformation;
    using GsmCommunicationToJson;
    using JsonToBusinessObjects.Conversion;
    using JsonToBusinessObjects.Conversion.GsmArc;
    using JsonToBusinessObjects.Conversion.LoRa;
    using JsonToBusinessObjects.DataContainers;
    using KellerAg.Shared.Entities.Database;
    using KellerAg.Shared.LoRaPayloadConverter;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class IoTConvert : IConvert
    {
        /// <inheritdoc />
        public string GsmCommunicationToJson(string gsmCommunicationText)
        {
            var converter = new GsmCommunicationToJsonConverter();
            JObject json = converter.Convert(gsmCommunicationText);
            return json.ToString();
        }

        /// <inheritdoc />
        public ConversionResult GsmCommunicationJsonToBusinessObject(string gsmCommunicationJson)
        {
            var converter = new JsonToBusinessObjectsConverterForGsm();
            ConversionResult result = converter.Convert(gsmCommunicationJson);
            return result;
        }

        /// <inheritdoc />
        public DeviceSettings BusinessObjectToDeviceConfiguration(BusinessObjectRoot businessObject)
        {
            var uniqueSerialNumber = Converters.BusinessObjectToDeviceConfiguration.SetExtractedUniqueSerialNumber(businessObject);

            DeviceSettings receivedSettings = Converters.BusinessObjectToDeviceConfiguration.CreateConfiguration(
                uniqueSerialNumber,
                businessObject.Configuration.GprsSettings,
                businessObject.Configuration.TextEmailSmsLocSettings,
                businessObject.Configuration.MeasurementSettings,
                businessObject.Configuration.FloatingPointMeasurementSettings,
                businessObject.Configuration.FtpSettings,
                businessObject.Configuration.MeasurementSettings2);
            return receivedSettings;
        }


        /// <inheritdoc />
        public List<string> DeviceConfigurationToLoRaPayloads(string deviceConfigurationDifferenceJson)
        {
            DeviceSettings deviceConfigurationDifferences =
                JsonConvert.DeserializeObject<DeviceSettings>(deviceConfigurationDifferenceJson);
            List<string> payloads = DeviceConfigurationToLoRaPayloads(deviceConfigurationDifferences);
            return payloads;
        }

        /// <inheritdoc />
        public List<string> DeviceConfigurationToLoRaPayloads(DeviceSettings deviceConfigurationDifferences)
        {
            DeviceConfigurationToPayloadInformationConverter converter = new DeviceConfigurationToPayloadInformationConverter();
            List<PayloadInformation> payloadInfos = converter.Convert(deviceConfigurationDifferences);
            List<string> payloads = new List<string>();
            foreach (PayloadInformation payloadInformation in payloadInfos)
            {
                payloads.AddRange(PayloadConverter.ConvertToActility(payloadInformation)); //Non-Base64 version
            }
            return payloads;
        }

        /// <inheritdoc />
        public string DeviceConfigurationToGsmCommunication(DeviceSettings deviceConfigurations)
        {
            var converter = new DeviceConfigurationToGsmCommunicationConverter();
            string gsmCommunication = converter.Convert(deviceConfigurations);
            return gsmCommunication;
        }

        /// <inheritdoc />
        public string DeviceConfigurationToGsmCommunication(string deviceConfigurationDifferenceJson)
        {
            DeviceSettings deviceConfigurationDifferences = JsonConvert.DeserializeObject<DeviceSettings>(deviceConfigurationDifferenceJson);
            return DeviceConfigurationToGsmCommunication(deviceConfigurationDifferences);
        }


        /// <inheritdoc />
        public PayloadInformation LoRaPayloadToLoRaMessage(string loRaPayload, int port)
        {
            if (IsAllHexChars(loRaPayload))
            {
                // We assume that when it is all hex characters it is not Base64 encoded. Therefore, we assume it is either from Actility or from Loriot encoded.
                // We use Actility, but it could also be from loriot. The output might be not be correct.
                var payloadInformationActility = LoRaPayloadConverter.PayloadConverter.ConvertFromActility(loRaPayload, port);
                return payloadInformationActility;
            }
            else
            {
                var payloadInformationTTN = LoRaPayloadConverter.PayloadConverter.ConvertFromTheThingNetwork(loRaPayload, port);
                return payloadInformationTTN;
            }

            ////Loriot.io
            //{
            //    var payloadInformationLoriot = LoRaPayloadConverter.PayloadConverter.ConvertFromLoriot(loRaPayload, port);
            //    return payloadInformationLoriot;
            //}

            // Helper method to check if the payload only consists of hex characters. Otherwise, it is assumed that it is a Base64 encrypted.
            bool IsAllHexChars(string payload)
            {
                return payload.All("0123456789abcdefABCDEF".Contains);
            }
        }


        /// <inheritdoc />
        public PayloadInformation LoRaPayloadToLoRaMessage(string loRaPayload, int port, int network)
        {
            switch (network)
            {
                case 0:
                    {
                        var payloadInformationTTN = LoRaPayloadConverter.PayloadConverter.ConvertFromTheThingNetwork(loRaPayload, port);
                        return payloadInformationTTN;
                    }
                case 1:
                    {
                        var payloadInformationActility = LoRaPayloadConverter.PayloadConverter.ConvertFromActility(loRaPayload, port);
                        return payloadInformationActility;
                    }
                case 2:
                    {
                        var payloadInformationLoriot = LoRaPayloadConverter.PayloadConverter.ConvertFromLoriot(loRaPayload, port);
                        return payloadInformationLoriot;
                    }
                default:
                    throw new PlatformNotSupportedException(
                        $"The network with id {network} is not supported. Choose either 0 (TTN), 1 (Actility based Network) or 2 (Loriot.io)");
            }
        }

        /// <inheritdoc />
        public BusinessObjectRoot LoRaJsonMessageToBusinessObject(string jsonMessage)
        {
            var converter = new JsonToBusinessObjectsConverterForLoRa();
            ConversionResult result = converter.Convert(jsonMessage);
            //here you might to check result.HasErrors
            return result.BusinessObjectRoot;
        }
    }
}
