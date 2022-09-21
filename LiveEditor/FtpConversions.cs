using JsonToBusinessObjects.Conversion;
using JsonToBusinessObjects.DataContainers;
using KellerAg.Shared.Entities.Database;
using KellerAg.Shared.IoT.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LiveEditor
{
    public class FtpConversions
    {
        private readonly IConvert _converter = new IoTConvert();

        public string GsmCommunicationText { get; set; }
        public JObject GsmCommunicationToJson { get; set; }
        public string GsmCommunicationToJsonText { get; set; }
        public ConversionResult BusinessObjectResult { get; set; }
        public BusinessObjectRoot BusinessObject { get; set; }
        public string BusinessObjectJson { get; set; }
        public bool IsFileAConfigurationMessage { get; set; }
        public DeviceSettings DeviceConfiguration { get; set; }
        public string DeviceConfigurationJson { get; set; }

        private int[] ChannelIds { get; set; }
        public int[] MeasurementDefinitionIds { get; set; }
        public List<BlazorMeasurement> Measurements { get; set; }

        public void UpdateFtpTextToDtoConversion(string text)
        {

            Console.WriteLine("Start UpdateFtpTextToDtoConversion()");

            bool hasExceptionHappened = false;
            try
            {
                GsmCommunicationText = text;
                GsmCommunicationToJson = _converter.GsmCommunicationToJsonObject(text);
                GsmCommunicationToJsonText = GsmCommunicationToJson.ToString();
            }
            catch (Exception e)
            {
                GsmCommunicationToJsonText = $"GsmCommunicationToJson Conversion didn't work: {e}/{e.InnerException}";
                hasExceptionHappened = true;
            }

            try
            {
                if (!hasExceptionHappened)
                {
                    BusinessObjectResult = _converter.GsmCommunicationJsonToBusinessObject(GsmCommunicationToJson);
                    BusinessObject = BusinessObjectResult.BusinessObjectRoot;
                    BusinessObjectJson = JsonConvert.SerializeObject(BusinessObject, Formatting.Indented);
                }
                else
                {
                    BusinessObjectJson = $"GsmCommunicationJsonToBusinessObject Conversion didn't work.";
                }
            }
            catch (Exception e)
            {
                BusinessObjectJson = $"GsmCommunicationJsonToBusinessObject Conversion didn't work: {e}/{e.InnerException}";
                hasExceptionHappened = true;
            }

            if (!hasExceptionHappened)
            {
                IsFileAConfigurationMessage = IsConfigurationMessage(BusinessObject);
                if (IsFileAConfigurationMessage)
                {
                    DeviceConfiguration = _converter.BusinessObjectToDeviceConfiguration(BusinessObject);
                    DeviceConfigurationJson = JsonConvert.SerializeObject(DeviceConfiguration, Formatting.Indented);
                }
                else
                {
                    DeviceConfiguration = null;
                    
                    ChannelIds = BusinessObject.Measurements.DataPointsByChannel.Keys.ToArray();

                    var measurementValuesByTimeStamp = new Dictionary<DateTime, float[]>();
                    for (var i = 0; i < ChannelIds.Length; i++)
                    {
                        KeyValuePair<int, List<DataPoint>> dataPointsPerChannel = BusinessObject.Measurements.DataPointsByChannel.ToArray()[i];
                        foreach (DataPoint dataPoint in dataPointsPerChannel.Value)
                        {
                            if (!measurementValuesByTimeStamp.ContainsKey(dataPoint.Time))
                            {
                                measurementValuesByTimeStamp.Add(dataPoint.Time, new float[ChannelIds.Length]);
                            }
                            measurementValuesByTimeStamp[dataPoint.Time][i] = dataPoint.Value;
                        }
                    }
                    Measurements = new List<BlazorMeasurement>();
                    foreach ((DateTime id, float[] values) in measurementValuesByTimeStamp)
                    {
                        Measurements.Add(new BlazorMeasurement
                        {
                            Time = id, Values = values
                        });
                    }

                }
            }
            else
            {
                DeviceConfigurationJson = "Conversion didn't work with given data";
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
            //todo
        }
    }
}