using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KellerAg.Shared.LoRaPayloadConverter.Tests
{
    [TestClass()]
    public class PayloadInformationJsonFormatTests
    {
        [TestMethod()]
        public void ConvertFromTheThingNetworkTest()
        {
            var pi = new PayloadInformation
            {
                DecodedPayload = new byte[]
                {
                    0xAF, 0xFE
                },
                Port = 42,
                FunctionCode = 0,
                IsSupportedDevice = true,
                OriginLoRaNetwork = OriginLoRaNetwork.TheThingsNetwork,
                Measurements = new List<Measurement>
                {
                    new Measurement
                    {
                        ConnectionDeviceType = 2,
                        MeasurementDefinitionId = 1,
                        ChannelNumber = 11,
                        Value = 1.234f
                    },
                    new Measurement
                    {
                        ConnectionDeviceType = 2,
                        MeasurementDefinitionId = 2,
                        ChannelNumber = 02,
                        Value = 5.678f
                    }
                },
                Info          = new InfoMessage(),
                ValuesOf8Bit  = new Dictionary<IndexOfValuesOf8Bit, byte>(),
                ValuesOf32Bit = new Dictionary<IndexOfValuesOf32Bit, uint>(),
                ValuesOfFloat = null,
                ValueOfText   = null,
                SensorInfo    = new KellerSensorInfo
                {
                    SensorCount = 2,
                    SensorType = SensorType.RS485,
                    SensorType1Data = new SensorType1
                    {
                        SensorClass   = 12,
                        SensorGroup   = 34,
                        SwVersionYear = 12,
                        SwVersionWeek = 50,
                        SerialNumber  = 846753
                    },
                    SensorType2Data = null
                }
            };

            pi.ValuesOf8Bit.Add(IndexOfValuesOf8Bit.DeviceGroup, 12);
            pi.ValuesOf8Bit.Add(IndexOfValuesOf8Bit.SWVersionYear, 19);

            pi.ValuesOf32Bit.Add(IndexOfValuesOf32Bit.SerialNumber, 4242);
            pi.ValuesOf32Bit.Add(IndexOfValuesOf32Bit.MainTime, 1212);

            pi.ValuesOfFloat = new Dictionary<IndexOfValuesOfFloat, float>
            {
                {IndexOfValuesOfFloat.GPSCoordinateAltitude, 1.234f},
                {IndexOfValuesOfFloat.AlarmDeltaThreshold  , float.MinValue},
                {IndexOfValuesOfFloat.GPSCoordinateLatitude, float.MaxValue}
            };


            string output = JsonConvert.SerializeObject(pi, Formatting.Indented);

            //var settings = new JsonSerializerSettings
            //{
            //    Formatting = Formatting.Indented
            //};
            //settings.Converters.Add(new StringEnumConverter
            //{
            //    AllowIntegerValues = true,
            //  //  CamelCaseText = false,
            //  //  NamingStrategy = null
            //});

            //string output2 = JsonConvert.SerializeObject(pi, settings);


            var sb = new System.Text.StringBuilder(1348);
            sb.AppendLine(@"{");
            sb.AppendLine(@"  ""Port"": 42,");
            sb.AppendLine(@"  ""IsSupportedDevice"": true,");
            sb.AppendLine(@"  ""OriginLoRaNetwork"": ""TheThingsNetwork"",");
            sb.AppendLine(@"  ""DecodedPayload"": ""r/4="",");
            sb.AppendLine(@"  ""FunctionCode"": ""UnknownMessage"",");
            sb.AppendLine(@"  ""Measurements"": [");
            sb.AppendLine(@"    {");
            sb.AppendLine(@"      ""ConnectionDeviceType"": 2,");
            sb.AppendLine(@"      ""MeasurementDefinitionId"": 1,");
            sb.AppendLine(@"      ""ChannelNumber"": 11,");
            sb.AppendLine(@"      ""Value"": 1.234");
            sb.AppendLine(@"    },");
            sb.AppendLine(@"    {");
            sb.AppendLine(@"      ""ConnectionDeviceType"": 2,");
            sb.AppendLine(@"      ""MeasurementDefinitionId"": 2,");
            sb.AppendLine(@"      ""ChannelNumber"": 2,");
            sb.AppendLine(@"      ""Value"": 5.678");
            sb.AppendLine(@"    }");
            sb.AppendLine(@"  ],");
            sb.AppendLine(@"  ""Info"": {");
            sb.AppendLine(@"    ""Type"": 0,");
            sb.AppendLine(@"    ""BatteryVoltage"": null,");
            sb.AppendLine(@"    ""BatteryCapacity"": null,");
            sb.AppendLine(@"    ""HumidityPercentage"": null,");
            sb.AppendLine(@"    ""DeviceClassGroupText"": null,");
            sb.AppendLine(@"    ""SwVersionText"": null,");
            sb.AppendLine(@"    ""SerialNumber"": 0,");
            sb.AppendLine(@"    ""DeviceLocalDateTime"": ""0001-01-01T00:00:00""");
            sb.AppendLine(@"  },");
            sb.AppendLine(@"  ""ValuesOf8Bit"": {");
            sb.AppendLine(@"    ""DeviceGroup"": 12,");
            sb.AppendLine(@"    ""SWVersionYear"": 19");
            sb.AppendLine(@"  },");
            sb.AppendLine(@"  ""ValuesOf32Bit"": {");
            sb.AppendLine(@"    ""SerialNumber"": 4242,");
            sb.AppendLine(@"    ""MainTime"": 1212");
            sb.AppendLine(@"  },");
            sb.AppendLine(@"  ""ValuesOfFloat"": {");
            sb.AppendLine(@"    ""GPSCoordinateAltitude"": 1.234,");
            sb.AppendLine(@"    ""AlarmDeltaThreshold"": -3.4028235E+38,");
            sb.AppendLine(@"    ""GPSCoordinateLatitude"": 3.4028235E+38");
            sb.AppendLine(@"  },");
            sb.AppendLine(@"  ""ValueOfText"": null,");
            sb.AppendLine(@"  ""SensorInfo"": {");
            sb.AppendLine(@"    ""SensorCount"": 2,");
            sb.AppendLine(@"    ""SensorType"": ""RS485"",");
            sb.AppendLine(@"    ""SensorType1Data"": {");
            sb.AppendLine(@"      ""SensorClass"": 12,");
            sb.AppendLine(@"      ""SensorGroup"": 34,");
            sb.AppendLine(@"      ""SwVersionYear"": 12,");
            sb.AppendLine(@"      ""SwVersionWeek"": 50,");
            sb.AppendLine(@"      ""SerialNumber"": 846753");
            sb.AppendLine(@"    },");
            sb.AppendLine(@"    ""SensorType2Data"": null");
            sb.AppendLine(@"  },");
            sb.AppendLine(@"  ""Command"": null");
            sb.Append(@"}");
            var expected = sb.ToString();
            Assert.IsTrue(output.Length > 10);
            Assert.AreEqual(output, expected);

            /*
                {
                  "DecodedPayload": "r/4=",
                  "Port": 42,
                  "FunctionCode": "UnknownMessage",
                  "IsSupportedDevice": true,
                  "OriginLoRaNetwork": "TheThingsNetwork",
                  "Measurements": [
                    {
                      "ConnectionDeviceType": 2,
                      "MeasurementDefinitionId": 1,
                      "ChannelNumber": 11,
                      "Value": 1.234
                    },
                    {
                      "ConnectionDeviceType": 2,
                      "MeasurementDefinitionId": 2,
                      "ChannelNumber": 2,
                      "Value": 5.678
                    }
                  ],
                  "Info": {
                    "Type": 0,
                    "BatteryVoltage": null,
                    "BatteryCapacity": null,
                    "HumidityPercentage": null,
                    "DeviceClassGroupText": null,
                    "SwVersionText": null,
                    "SerialNumber": 0,
                    "DeviceLocalDateTime": "0001-01-01T00:00:00"
                  },
                  "ValuesOf8Bit": {
                    "DeviceGroup": 12,
                    "SWVersionYear": 19
                  },
                  "ValuesOf32Bit": {
                    "SerialNumber": 4242,
                    "MainTime": 1212
                  },
                  "ValuesOfFloat": {
                    "GPSCoordinateAltitude": 1.234,
                    "AlarmDeltaThreshold": -3.40282347E+38,
                    "GPSCoordinateLatitude": 3.40282347E+38
                  },
                  "ValueOfText": null,
                  "SensorInfo": {
                    "SensorCount": 2,
                    "SensorType": "RS485",
                    "SensorType1Data": {
                      "SensorClass": 12,
                      "SensorGroup": 34,
                      "SwVersionYear": 12,
                      "SwVersionWeek": 50,
                      "SerialNumber": 846753
                    },
                    "SensorType2Data": null
                  },
                  "Command": null
                }
            */
            var payloadInfoObjectLoadedFromJson = JsonConvert.DeserializeObject<PayloadInformation>(expected);

            payloadInfoObjectLoadedFromJson.Should().BeEquivalentTo(pi);

        }
    }
}