using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Data;
using JsonToBusinessObjects.Conversion;
using JsonToBusinessObjects.DataContainers;
using KellerAg.Shared.IoT.Converters;
using Newtonsoft.Json;

namespace DemoBlazorApp
{
    public class FtpConversions
    {
        private readonly IConvert _converter = new KellerAg.Shared.IoT.Converters.IoTConvert();

        public string GsmCommunicationText { get; set; }
        public string GsmCommunicationToJson { get; set; }
        public ConversionResult BusinessObjectResult { get; set; }
        public BusinessObjectRoot BusinessObject { get; set; }
        public string BusinessObjectJson { get; set; }
        public bool IsFileAConfigurationMessage { get; set; }
        public DeviceSettings DeviceConfiguration { get; set; }
        public string DeviceConfigurationJson { get; set; }

        public int[] ChannelIds { get; set; }
        public int[] MeasurementDefinitionIds { get; set; }
        public List<BlazorMeasurement> Measurements { get; set; }

        public void UpdateFtpTextToDtoConversion(string text)
        {
            bool hasExceptionHappened = false;
            try
            {
                this.GsmCommunicationText = text;
                this.GsmCommunicationToJson = this._converter.GsmCommunicationToJson(text);
            }
            catch (Exception e)
            {
                this.GsmCommunicationToJson = $"GsmCommunicationToJson Conversion didn't work: {e}/{e.InnerException}";
                hasExceptionHappened = true;
            }

            try
            {
                if (!hasExceptionHappened)
                {
                    this.BusinessObjectResult = this._converter.GsmCommunicationJsonToBusinessObject(this.GsmCommunicationToJson);
                    this.BusinessObject = this.BusinessObjectResult.BusinessObjectRoot;
                    this.BusinessObjectJson = JsonConvert.SerializeObject(this.BusinessObject, Formatting.Indented);
                }
                else
                {
                    this.BusinessObjectJson = $"GsmCommunicationJsonToBusinessObject Conversion didn't work.";
                }
            }
            catch (Exception e)
            {
                this.BusinessObjectJson = $"GsmCommunicationJsonToBusinessObject Conversion didn't work: {e}/{e.InnerException}";
                hasExceptionHappened = true;
            }

            if (!hasExceptionHappened)
            {
                this.IsFileAConfigurationMessage = IsConfigurationMessage(this.BusinessObject);
                if (this.IsFileAConfigurationMessage)
                {
                    this.DeviceConfiguration = this._converter.BusinessObjectToDeviceConfiguration(this.BusinessObject);
                    this.DeviceConfigurationJson = JsonConvert.SerializeObject(this.DeviceConfiguration, Formatting.Indented);
                }
                else
                {
                    this.DeviceConfiguration = null;
                    
                    this.ChannelIds = this.BusinessObject.Measurements.DataPointsByChannel.Keys.ToArray();

                    Dictionary<DateTime, float[]> measurementValuesByTimeStamp = new Dictionary<DateTime, float[]>();
                    for (int i = 0; i < this.ChannelIds.Length; i++)
                    {
                        var dataPointsPerChannel = this.BusinessObject.Measurements.DataPointsByChannel.ToArray()[i];
                        foreach (var dataPoint in dataPointsPerChannel.Value)
                        {
                            if (!measurementValuesByTimeStamp.ContainsKey(dataPoint.Time))
                            {
                                measurementValuesByTimeStamp.Add(dataPoint.Time, new float[this.ChannelIds.Length]);
                            }
                            measurementValuesByTimeStamp[dataPoint.Time][i] = dataPoint.Value;
                        }
                    }
                    this.Measurements = new List<BlazorMeasurement>();
                    foreach (var (id, values) in measurementValuesByTimeStamp)
                    {
                        this.Measurements.Add(new BlazorMeasurement()
                        {
                            Time = id, Values = values
                        });
                    }

                }
            }
            else
            {
                this.DeviceConfigurationJson = "Conversion didn't work with given data";
            }
        }

        private static bool IsConfigurationMessage(BusinessObjectRoot businessObjectRoot)
        {
            Console.WriteLine("businessObjectRoot.MessageType : " + businessObjectRoot.MessageType);
            return businessObjectRoot.MessageType switch
            {
                MessageType.RecordDataMessage => false,
                MessageType.ConfigurationMessageWithAck => true,
                MessageType.ConfigurationMessageWithoutAck => true,
                MessageType.UnknownMessage => true,
                _ => false
            };
        }


        public void UpdateLoRaPayloadToDtoConversion(string text)
        {
        }
    }

    public class BlazorMeasurement
    {
        public DateTime Time { get; set; }
        public float[] Values { get; set; }
    }
}