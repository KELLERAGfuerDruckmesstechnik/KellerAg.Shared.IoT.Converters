using System;
using System.Collections.Generic;
using System.Linq;
using JsonToBusinessObjects.DataContainers;
using JsonToBusinessObjects.Infrastructure.Logging;
using KellerAg.Shared.LoRaPayloadConverter;
using Newtonsoft.Json.Linq;

namespace JsonToBusinessObjects.Conversion.LoRa
{
    public class JsonToBusinessObjectsConverterForLoRa : IJsonToBusinessObjectsConverter
    {
        private readonly ILogger _logger;

        public JsonToBusinessObjectsConverterForLoRa()
        {
            _logger = new InternalLogger();
        }

        private ConversionResult ConvertLoraMessageActility(LoraMessageActility message)
        {
            try
            {
                int port = message.DevEUIUplink.FPort.ToInt(_logger);
                PayloadInformation payloadInfo = PayloadConverter.ConvertFromActility(message.DevEUIUplink.PayloadHex, port);

                var loraMessage = new LoRaMessage
                {
                    Payload = message.DevEUIUplink.PayloadHex,
                    Time = message.DevEUIUplink.Time.ToDateTime(_logger),
                    EUI = message.DevEUIUplink.DevEUI,
                    Port = port,
                    CounterUp = message.DevEUIUplink.FCntUp.ToInt(_logger),
                    CounterDown = message.DevEUIUplink.FCntDn?.ToInt(_logger),
                    DataCodingRate = $"{message.DevEUIUplink.SpFact};{message.DevEUIUplink.SubBand};{message.DevEUIUplink.Channel}",
                    StrongestSourceId = message.DevEUIUplink.Lrrid,
                    SourceIdCount = message.DevEUIUplink.Lrrs.Lrr.Count,
                    RSSI = message.DevEUIUplink.LrrRSSI.ToFloat(_logger),
                    SNR = message.DevEUIUplink.LrrSNR.ToFloat(_logger),
                    Measurements = new Dictionary<int, float>(),

                    //DeviceTypeId = -1,
                    DeviceConnectionType = -1,
                    Latitude = 0,
                    Longitude = 0,
                    AdditionalNetworkInfo = null //no info available
                };

                if (payloadInfo.Measurements != null && payloadInfo.Measurements.Count > 0)
                {
                    foreach (Measurement measurement in payloadInfo.Measurements)
                    {
                        if (!loraMessage.Measurements.ContainsKey(measurement.ChannelNumber))
                        {
                            loraMessage.Measurements.Add(measurement.ChannelNumber, measurement.Value);
                        }
                        else
                        {
                            loraMessage.Measurements[measurement.ChannelNumber] = measurement.Value;
                        }
                    }
                    loraMessage.DeviceConnectionType = payloadInfo.Measurements[0].ConnectionDeviceType;
                }


                if (message.DevEUIUplink.CustomerData.Loc != null)
                {
                    loraMessage.Latitude = message.DevEUIUplink.CustomerData.Loc.Lat.ToFloat(_logger);
                    loraMessage.Longitude = message.DevEUIUplink.CustomerData.Loc.Lon.ToFloat(_logger);
                }
                else
                {
                    loraMessage.Latitude = 0f;
                    loraMessage.Longitude = 0f;
                    _logger.Warn("Longitude and Latitude are NULL.");
                }

                var businessObjectRoot =
                    new BusinessObjectRoot
                    {
                        DeviceInformation = new DeviceInformation
                        {
                            ProductLine = ProductLineName.ARC1_LoRa,
                            SignalQuality = (int?) ((loraMessage.RSSI + 113f) / 2f), //The SPA translates this back again to DBM

                            DeviceSerialNumber = payloadInfo.Info?.SerialNumber,
                            BatteryCapacity = payloadInfo.Info?.BatteryCapacity,
                            GsmModuleSoftwareVersion = payloadInfo.Info?.SwVersionText,
                            MeasuredBatteryVoltage = payloadInfo.Info?.BatteryVoltage,
                            DeviceIdAndClass = payloadInfo.Info?.DeviceClassGroupText,
                            DeviceLocalDateTime = payloadInfo.Info?.DeviceLocalDateTime,
                            MeasuredHumidity = payloadInfo.Info?.HumidityPercentage
                        },
                        LoRaData = loraMessage.ShallowCopy()
                    };

                if (!payloadInfo.IsSupportedDevice)
                {
                    _logger.Error($"Unsupported Payload: {message.DevEUIUplink.PayloadHex} from {message.DevEUIUplink.DevEUI}");
                    return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
                }

                businessObjectRoot.Measurements = new ChannelDataStorage();
                foreach (KeyValuePair<int, float> measurement in loraMessage.Measurements)
                {
                    businessObjectRoot.Measurements.StoreInChannel(measurement.Key,
                        new DataPoint(loraMessage.Time, measurement.Value));
                }

                businessObjectRoot.MessageType = GetMessageTypeFromPortNumber(loraMessage);

                return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unsupported JSON format with Payload: {message.DevEUIUplink.PayloadHex} from {message.DevEUIUplink.DevEUI} - {ex}");

                var businessObjectRoot =
                    new BusinessObjectRoot
                    {
                        DeviceInformation = new DeviceInformation
                        {
                            ProductLine = ProductLineName.ARC1_LoRa
                        }
                    };
                return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
            }
        }

        private ConversionResult ConvertLoraMessageNetmore(LoraMessageNetmore message)
        {
            try
            {
                int port = (int)message.FPort;
                PayloadInformation payloadInfo = PayloadConverter.ConvertFromNetmore(message.Payload, port);

                var loraMessage = new LoRaMessage
                {
                    Payload = message.Payload,
                    Time = message.Timestamp.UtcDateTime,
                    EUI = message.DevEui,
                    Port = port,
                    CounterUp = (int)message.FCntUp,
                    CounterDown = 0, //there is no down counter?
                    DataCodingRate = $"{message.Dr};{message.Snr};{message.Freq};{message.Toa}",
                    StrongestSourceId = message.GatewayIdentifier.ToString(),
                    SourceIdCount = message.Gateways.Length,
                    RSSI = message.Rssi,
                    SNR = message.Snr.ToFloat(_logger),
                    Measurements = new Dictionary<int, float>(),

                    //DeviceTypeId = -1,
                    DeviceConnectionType = -1,
                    Latitude = (float)message.Latitude,
                    Longitude = (float)message.Longitude,
                    AdditionalNetworkInfo = null //no info available
                };

                if (payloadInfo.Measurements != null && payloadInfo.Measurements.Count > 0)
                {
                    foreach (Measurement measurement in payloadInfo.Measurements)
                    {
                        loraMessage.Measurements[measurement.ChannelNumber] = measurement.Value;
                    }
                    loraMessage.DeviceConnectionType = payloadInfo.Measurements[0].ConnectionDeviceType;
                }

                var businessObjectRoot =
                    new BusinessObjectRoot
                    {
                        DeviceInformation = new DeviceInformation
                        {
                            ProductLine = ProductLineName.ARC1_LoRa,
                            SignalQuality = (int?)((loraMessage.RSSI + 113f) / 2f), //The SPA translates this back again to DBM

                            DeviceSerialNumber = payloadInfo.Info?.SerialNumber,
                            BatteryCapacity = payloadInfo.Info?.BatteryCapacity,
                            GsmModuleSoftwareVersion = payloadInfo.Info?.SwVersionText,
                            MeasuredBatteryVoltage = payloadInfo.Info?.BatteryVoltage,
                            DeviceIdAndClass = payloadInfo.Info?.DeviceClassGroupText,
                            DeviceLocalDateTime = payloadInfo.Info?.DeviceLocalDateTime,
                            MeasuredHumidity = payloadInfo.Info?.HumidityPercentage
                        },
                        LoRaData = loraMessage.ShallowCopy()
                    };

                if (!payloadInfo.IsSupportedDevice)
                {
                    _logger.Error($"Unsupported Payload: {message.Payload} from {message.DevEui}");
                    return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
                }

                businessObjectRoot.Measurements = new ChannelDataStorage();
                foreach (KeyValuePair<int, float> measurement in loraMessage.Measurements)
                {
                    businessObjectRoot.Measurements.StoreInChannel(measurement.Key,
                        new DataPoint(loraMessage.Time, measurement.Value));
                }

                businessObjectRoot.MessageType = GetMessageTypeFromPortNumber(loraMessage);

                return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unsupported JSON format with Payload: {message.Payload} from {message.DevEui} - {ex}");

                var businessObjectRoot =
                    new BusinessObjectRoot
                    {
                        DeviceInformation = new DeviceInformation
                        {
                            ProductLine = ProductLineName.ARC1_LoRa
                        }
                    };
                return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
            }
        }


        private ConversionResult ConvertLoraMessageTheThingsNetwork(LoraMessageTheThingsNetwork message)
        {
            try
            {
                PayloadInformation payloadInfo;
                if (String.IsNullOrEmpty(message.PayloadRaw))
                {
                    _logger.Error($"Unsupported Payload: 'null' from {message.HardwareSerial}");
                    message.PayloadRaw = "";
                    payloadInfo = new PayloadInformation();
                }
                else
                {
                    payloadInfo = PayloadConverter.ConvertFromTheThingNetwork(message.PayloadRaw, (int)message.Port);
                }

                var strongestGateway = message.Metadata.Gateways.OrderByDescending(i => i.Rssi).First();

                var loraMessage = new LoRaMessage
                {
                    Payload = message.PayloadRaw,
                    Time = message.Metadata.Time.ToDateTime(_logger),
                    DeviceConnectionType = -1,
                    Measurements = new Dictionary<int, float>(),

                    EUI = message.HardwareSerial,
                    Port = (int)message.Port,
                    CounterUp = (int)message.Counter,
                    CounterDown = 0, //there is no down counter?

                    DataCodingRate = $"{message.Metadata.DataRate};{message.Metadata.CodingRate};{message.Metadata.Frequency}",

                    Latitude = (float)strongestGateway.Latitude,
                    Longitude = (float)strongestGateway.Longitude,
                    StrongestSourceId = strongestGateway.GtwId,
                    SourceIdCount = message.Metadata.Gateways.Count,
                    RSSI = (float)strongestGateway.Rssi,
                    SNR = (float)strongestGateway.Snr,
                    AdditionalNetworkInfo = new AdditionalNetworkInfo
                    {
                        NetworkDeviceName = message.DevId,
                        NetworkApplicationName = message.AppId,
                        RegionHint = message.DownlinkUrl
                    }
                };

                if (payloadInfo.Measurements != null  && payloadInfo.Measurements.Count>0)
                {
                    foreach (Measurement measurement in payloadInfo.Measurements)
                    {
                        if (!loraMessage.Measurements.ContainsKey(measurement.ChannelNumber))
                        {
                            loraMessage.Measurements.Add(measurement.ChannelNumber, measurement.Value);
                        }
                        else
                        {
                            loraMessage.Measurements[measurement.ChannelNumber] = measurement.Value;
                        }
                    }
                    loraMessage.DeviceConnectionType = payloadInfo.Measurements[0].ConnectionDeviceType;
                }

                var businessObjectRoot =
                    new BusinessObjectRoot
                    {
                        DeviceInformation = new DeviceInformation
                        {
                            ProductLine = ProductLineName.ARC1_LoRa,
                            SignalQuality = (int?)((loraMessage.RSSI + 113f) / 2f), //The SPA translates this back again to DBM

                            DeviceSerialNumber = payloadInfo.Info?.SerialNumber,
                            BatteryCapacity = payloadInfo.Info?.BatteryCapacity,
                            GsmModuleSoftwareVersion = payloadInfo.Info?.SwVersionText,
                            MeasuredBatteryVoltage = payloadInfo.Info?.BatteryVoltage,
                            DeviceIdAndClass = payloadInfo.Info?.DeviceClassGroupText,
                            DeviceLocalDateTime = payloadInfo.Info?.DeviceLocalDateTime,
                            MeasuredHumidity = payloadInfo.Info?.HumidityPercentage
                        },
                        LoRaData = loraMessage.ShallowCopy()
                    };



                if (!payloadInfo.IsSupportedDevice)
                {
                    _logger.Error($"Unsupported Payload: {message.PayloadRaw} from {message.HardwareSerial}");
                    return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
                }

                businessObjectRoot.Measurements = new ChannelDataStorage();
                foreach (KeyValuePair<int, float> measurement in loraMessage.Measurements)
                {
                    businessObjectRoot.Measurements.StoreInChannel(measurement.Key, new DataPoint(loraMessage.Time, measurement.Value));
                }

                businessObjectRoot.MessageType = GetMessageTypeFromPortNumber(loraMessage);

                return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unsupported JSON format with Payload: {message.PayloadRaw} - {ex}");
                
                var businessObjectRoot =
                    new BusinessObjectRoot
                    {
                        DeviceInformation = new DeviceInformation
                        {
                            ProductLine = ProductLineName.ARC1_LoRa
                        }
                    };
                return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
            }
        }


        private ConversionResult ConvertLoraMessageTheThingsNetworkV3(LoraMessageTheThingsNetworkV3 message)
        {
            try
            {
                PayloadInformation payloadInfo;
                if (string.IsNullOrEmpty(message.UplinkMessage.FrmPayload))
                {
                    _logger.Error($"Unsupported Payload: 'null' from {message.EndDeviceIds.DevEui} / {message.EndDeviceIds.DeviceId}");
                    message.UplinkMessage.FrmPayload = "";
                    payloadInfo = new PayloadInformation();
                }
                else
                {
                    payloadInfo = PayloadConverter.ConvertFromTheThingNetwork(message.UplinkMessage.FrmPayload, message.UplinkMessage.FPort);
                }

                var strongestGateway = message.UplinkMessage.RxMetadata.OrderByDescending(i => i.Rssi).First();

                var loraMessage = new LoRaMessage
                {
                    Payload = message.UplinkMessage.FrmPayload,
                    Time = message.UplinkMessage.ReceivedAt.UtcDateTime,
                    DeviceConnectionType = -1,
                    Measurements = new Dictionary<int, float>(),

                    EUI = message.EndDeviceIds.DevEui,
                    Port = message.UplinkMessage.FPort,
                    CounterUp = message.UplinkMessage.FCnt,
                    CounterDown = 0, //there is no down counter?

                    DataCodingRate = $"SF{message.UplinkMessage.Settings.DataRate.Lora.SpreadingFactor}BW{message.UplinkMessage.Settings.DataRate.Lora.Bandwidth/1000.0};{message.UplinkMessage.Settings.CodingRate};{message.UplinkMessage.Settings.Frequency/1000000.0}",

                    Latitude = (strongestGateway.Location==null) ? 0f :(float)strongestGateway.Location.Latitude,
                    Longitude = (strongestGateway.Location == null) ? 0f : (float)strongestGateway.Location.Longitude,
                    StrongestSourceId = strongestGateway.GatewayIds.GatewayId,
                    SourceIdCount = message.UplinkMessage.RxMetadata.Count,
                    RSSI = (float)strongestGateway.Rssi,
                    SNR = (float)strongestGateway.Snr,
                    AdditionalNetworkInfo = new AdditionalNetworkInfo
                    {
                        NetworkDeviceName = message.EndDeviceIds.DeviceId,
                        NetworkApplicationName = message.EndDeviceIds.ApplicationIds.ApplicationId,
                        //RegionHint = message.UplinkMessage.RxMetadata.FirstOrDefault(_ => _.PacketBroker != null)?.PacketBroker.HomeNetworkClusterId
                    }
                };
                
                if (payloadInfo.Measurements != null && payloadInfo.Measurements.Count > 0)
                {
                    foreach (Measurement measurement in payloadInfo.Measurements)
                    {
                        if (!loraMessage.Measurements.ContainsKey(measurement.ChannelNumber))
                        {
                            loraMessage.Measurements.Add(measurement.ChannelNumber, measurement.Value);
                        }
                        else
                        {
                            loraMessage.Measurements[measurement.ChannelNumber] = measurement.Value;
                        }
                    }
                    loraMessage.DeviceConnectionType = payloadInfo.Measurements[0].ConnectionDeviceType;
                }

                var businessObjectRoot =
                    new BusinessObjectRoot
                    {
                        DeviceInformation = new DeviceInformation
                        {
                            ProductLine              = ProductLineName.ARC1_LoRa,
                            SignalQuality            = (int?)((loraMessage.RSSI + 113f) / 2f), //The SPA translates this back again to DBM

                            DeviceSerialNumber       = payloadInfo.Info?.SerialNumber,
                            BatteryCapacity          = payloadInfo.Info?.BatteryCapacity,
                            GsmModuleSoftwareVersion = payloadInfo.Info?.SwVersionText,
                            MeasuredBatteryVoltage   = payloadInfo.Info?.BatteryVoltage,
                            DeviceIdAndClass         = payloadInfo.Info?.DeviceClassGroupText,
                            DeviceLocalDateTime      = payloadInfo.Info?.DeviceLocalDateTime,
                            MeasuredHumidity         = payloadInfo.Info?.HumidityPercentage
                        },
                        LoRaData = loraMessage.ShallowCopy()
                    };



                if (!payloadInfo.IsSupportedDevice)
                {
                    _logger.Error($"Unsupported Payload: {message.UplinkMessage.FrmPayload} from {message.EndDeviceIds.DevEui} ({message.EndDeviceIds.DeviceId} | {message.EndDeviceIds.ApplicationIds.ApplicationId})");
                    return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
                }

                businessObjectRoot.Measurements = new ChannelDataStorage();
                foreach (KeyValuePair<int, float> measurement in loraMessage.Measurements)
                {
                    businessObjectRoot.Measurements.StoreInChannel(measurement.Key, new DataPoint(loraMessage.Time, measurement.Value));
                }

                businessObjectRoot.MessageType = GetMessageTypeFromPortNumber(loraMessage);

                return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unsupported JSON format with Payload: {message.UplinkMessage.FrmPayload} - {ex}");

                var businessObjectRoot =
                    new BusinessObjectRoot
                    {
                        DeviceInformation = new DeviceInformation
                        {
                            ProductLine = ProductLineName.ARC1_LoRa
                        }
                    };
                return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
            }
        }

        private ConversionResult ConvertLoraMessageLoriot(LoraMessageLoriot message)
        {
            try
            {
                int gatewayCount = 1;
                var strongestGateway = new LoriotGateway();
                bool hasGateways = message.Gws != null;
                if (hasGateways)
                {
                    gatewayCount = message.Gws.Count;
                    strongestGateway = message.Gws.OrderByDescending(_ => _.Rssi).First();
                }

                var loraMessage = new LoRaMessage
                {
                    Payload = message.Data,
                    Time = ConvertUnixMillisecondsTimeStamp(message.Ts),

                    EUI       = message.Eui,
                    Port      = (int) message.Port,
                    CounterUp = (int) message.Fcnt,
                    CounterDown = 0,

                    DataCodingRate = $"{message.Dr};{message.Snr};{message.Freq};{message.Toa}",
                    StrongestSourceId = "unknown",
                    SourceIdCount = gatewayCount,
                    RSSI = (float?)message.Rssi,
                    SNR = (float?) message.Snr,
                    Latitude = 0f,
                    Longitude = 0f,
                    AdditionalNetworkInfo = null //no info available
                };


                if (message.Cmd == "rx")
                {
                    PayloadInformation payloadInfo = PayloadConverter.ConvertFromLoriot(message.Data, (int)message.Port);
                    loraMessage.Measurements = new Dictionary<int, float>();
                    loraMessage.DeviceConnectionType = -1;

                    if (payloadInfo.Measurements != null && payloadInfo.Measurements.Count>0)
                    {
                        foreach (Measurement measurement in payloadInfo.Measurements)
                        {
                            if (!loraMessage.Measurements.ContainsKey(measurement.ChannelNumber))
                            {
                                loraMessage.Measurements.Add(measurement.ChannelNumber, measurement.Value);
                            }
                            else
                            {
                                loraMessage.Measurements[measurement.ChannelNumber] = measurement.Value;
                            }
                        }

                        loraMessage.DeviceConnectionType = payloadInfo.Measurements[0].ConnectionDeviceType;
                    }
                }
                else
                {
                    loraMessage.Measurements = new Dictionary<int, float>(0);
                    loraMessage.DeviceConnectionType = -1;
                }

                if (hasGateways)
                {
                    loraMessage.StrongestSourceId = strongestGateway.Gweui;
                }

                if (hasGateways)
                {
                    if (loraMessage.RSSI == null) { loraMessage.RSSI = (float?)strongestGateway.Rssi; }
                    if (loraMessage.SNR == null) { loraMessage.SNR = (float?)strongestGateway.Snr; }
                }

                //there seems to be no location data from Loriot
                if (hasGateways)
                {
                    if (strongestGateway.Lat != null) { loraMessage.Latitude = (float)strongestGateway.Lat; }
                    if (strongestGateway.Lon != null) { loraMessage.Longitude = (float)strongestGateway.Lon; }
                    //_logger.Warn("Longitude and Latitude are NULL.");
                }

                byte? batteryCapacity = null;
                if (message.Bat!=null && 0 <= message.Bat && message.Bat < 255)
                {
                    batteryCapacity = (byte?) (message.Bat / 2.54);
                }
                else
                {
                    _logger.Warn("message.Bat is extraordinary (or null): " + message.Bat);
                }


                var businessObjectRoot =
                    new BusinessObjectRoot
                    {
                        DeviceInformation = new DeviceInformation
                        {
                            ProductLine = ProductLineName.ARC1_LoRa,
                            SignalQuality = (int?)((loraMessage.RSSI + 113f) / 2f), //The SPA translates this back again to DBM
                            BatteryCapacity = batteryCapacity
                            //the rest is null initialized
                        },
                        LoRaData = loraMessage.ShallowCopy()
                    };

                if (message.Cmd == "rx") 
                {
                    PayloadInformation payloadInfo = PayloadConverter.ConvertFromLoriot(message.Data, (int)message.Port);
                    businessObjectRoot.DeviceInformation.DeviceSerialNumber       = payloadInfo.Info?.SerialNumber;
                    businessObjectRoot.DeviceInformation.GsmModuleSoftwareVersion = payloadInfo.Info?.SwVersionText;
                    businessObjectRoot.DeviceInformation.MeasuredBatteryVoltage   = payloadInfo.Info?.BatteryVoltage;
                    businessObjectRoot.DeviceInformation.DeviceIdAndClass         = payloadInfo.Info?.DeviceClassGroupText;
                    businessObjectRoot.DeviceInformation.DeviceLocalDateTime      = payloadInfo.Info?.DeviceLocalDateTime;
                    businessObjectRoot.DeviceInformation.MeasuredHumidity         = payloadInfo.Info?.HumidityPercentage;
                    businessObjectRoot.LoRaData = loraMessage.ShallowCopy();
                    
                    if (!payloadInfo.IsSupportedDevice)
                    {
                        _logger.Error($"Unsupported Payload: {message.Data} from {message.Eui}");
                        return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
                    }
                }



                businessObjectRoot.Measurements = new ChannelDataStorage();
                foreach (KeyValuePair<int, float> measurement in loraMessage.Measurements)
                {
                    businessObjectRoot.Measurements.StoreInChannel(measurement.Key,
                        new DataPoint(loraMessage.Time, measurement.Value));
                }

                if (message.Cmd == "gw")
                {
                    businessObjectRoot.MessageType = MessageType.InformationMessage;
                }
                else
                {
                    businessObjectRoot.MessageType = GetMessageTypeFromPortNumber(loraMessage);
                }
                    

                return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unsupported JSON format with Payload: {message.Data} from {message.Eui} - {ex}");

                var businessObjectRoot =
                    new BusinessObjectRoot
                    {
                        DeviceInformation = new DeviceInformation
                        {
                            ProductLine = ProductLineName.ARC1_LoRa
                        }
                    };
                return new ConversionResult(businessObjectRoot, (_logger as InternalLogger)?.ConversionMessages);
            }
        }

        private static MessageType GetMessageTypeFromPortNumber(LoRaMessage loraMessage)
        {
            switch (loraMessage.Port)
            {
                case 1 : return MessageType.RecordDataMessage; //This is the normal Message type for measurements
                case 2 : return MessageType.AlarmMessage;
                case 3 : return MessageType.ConfigurationMessageWithoutAck; //there is no ack for LoRa implemented from KELLER
                case 4 : return MessageType.InformationMessage;
                case 5 : return MessageType.AnswerFromRequestMessage;
                default: return MessageType.UnknownMessage;
            }
        }

        private static DateTime ConvertUnixMillisecondsTimeStamp(double unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTimeStamp);
        }

        /// <inheritdoc />
        public ConversionResult Convert(string jsonString)
        {
            //todo store message to ?

            if (IsJsonFromTheThingsNetworkV3(jsonString))
            {
                var message = LoraMessageTheThingsNetworkV3.FromJson(jsonString);
                if (message.UplinkMessage?.FrmPayload == null)
                {
                    _logger.Warn("Can not use this message. Payload is null: " + jsonString);
                    return new ConversionResult(new BusinessObjectRoot { LoRaData = new LoRaMessage { Payload = null } }, (_logger as InternalLogger)?.ConversionMessages);
                }
                return ConvertLoraMessageTheThingsNetworkV3(message);
            }

            if (IsJsonFromTheThingsNetwork(jsonString))
            {
                LoraMessageTheThingsNetwork message = LoraMessageTheThingsNetwork.FromJson(jsonString);
                if (message.PayloadRaw == null)
                {
                    _logger.Warn("Can not use this message. Payload is null: " + jsonString);
                    return new ConversionResult(new BusinessObjectRoot { LoRaData = new LoRaMessage { Payload = null } }, (_logger as InternalLogger)?.ConversionMessages);
                }
                return ConvertLoraMessageTheThingsNetwork(message);
            }

            if (IsJsonFromActility(jsonString))
            {
                LoraMessageActility message = LoraMessageActility.FromJson(jsonString);
                if (message.DevEUIUplink.PayloadHex == null)
                {
                    _logger.Warn("Can not use this message. Payload is null: " + jsonString);
                    return new ConversionResult(new BusinessObjectRoot { LoRaData = new LoRaMessage { Payload = null } }, (_logger as InternalLogger)?.ConversionMessages);
                }
                return ConvertLoraMessageActility(message);
            }

            if (IsJsonFromNetmore(jsonString))
            {
                LoraMessageNetmore message = LoraMessageNetmore.FromJson(jsonString);
                if (message.Payload == null)
                {
                    _logger.Warn("Can not use this message. Payload is null: " + jsonString);
                    return new ConversionResult(new BusinessObjectRoot { LoRaData = new LoRaMessage { Payload = null } }, (_logger as InternalLogger)?.ConversionMessages);
                }
                return ConvertLoraMessageNetmore(message);
            }

            //must be Loriot
            {
                LoraMessageLoriot message = LoraMessageLoriot.FromJson(jsonString);
                if (message.Data == null)
                {
                    _logger.Warn("This is most probably a Gateway message. Payload is null: " + jsonString);
                   // return new ConversionResult(new BusinessObjectRoot{LoRaData = new LoRaMessage{Payload = null}}, (_logger as InternalLogger)?.ConversionMessages);
                }
                return ConvertLoraMessageLoriot(message);
            }

        }

        private static bool IsJsonFromTheThingsNetwork(string jsonString)
        {
            return JObject.Parse(jsonString).Properties().ToList().Exists(i => i.Name == "app_id");
        }

        private static bool IsJsonFromTheThingsNetworkV3(string jsonString)
        {
            return JObject.Parse(jsonString).Properties().ToList().Exists(i => i.Name == "end_device_ids");
        }

        private static bool IsJsonFromActility(string jsonString)
        {
            return JObject.Parse(jsonString).Properties().ToList().Exists(i => i.Name.ToLower() == "DevEUI_uplink".ToLower());
        }

        private static bool IsJsonFromNetmore(string jsonString)
        {
            return JObject.Parse(jsonString).Properties().ToList().Exists(i => i.Name.ToLower() == "gatewayIdentifier".ToLower());
        }
    }
}