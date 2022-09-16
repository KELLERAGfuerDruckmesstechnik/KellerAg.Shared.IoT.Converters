using System;
using System.Collections.Generic;

namespace JsonToBusinessObjects.Conversion.LoRa
{
    /// <summary>
    /// Data container for the most important information of TTN messages, Actility messages and Loriot.io messages
    /// </summary>
    public class LoRaMessage
    {
        /// <summary>
        /// Actility: DevEUI_uplink.Time
        /// TTN: metadata.time
        /// Loriot: Ts
        /// Should be in UTC
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.DevEUI
        /// TTN: hardware_serial
        /// Loriot: EUI
        /// </summary>
        public string EUI { get; set; }

        /// <summary>
        /// Actility: List of all found measurements in the Payload string
        /// TTN: List of all found measurements in the Payload string
        /// Loriot: List of all found measurements in the Payload string
        /// 
        /// key (int) represents the channel number (not the measurementDefinitionID)
        /// </summary>
        public Dictionary<int, float> Measurements { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.FPort
        /// TTN: port
        /// Loriot: port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.FCntUp
        /// TTN: counter
        /// Loriot: fcnt
        /// </summary>
        public int CounterUp { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.FCntDown
        /// TTN: -- does not exist here
        /// Loriot: -- does not exist here
        /// </summary>
        public int? CounterDown { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.payload_hex
        /// TTN: payload_raw
        /// Loriot: data
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// Represents the Device Type (or “Connection Type”).
        /// Please see https://docs.kolibricloud.ch/sending-technology/lora-technology/keller-lora-payload/.
        /// </summary>
        public int DeviceConnectionType { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.SpFact;DevEUI_uplink.SubBand;DevEUI_uplink.Channel
        /// TTN: combination of metadata.data_rate;metadata.coding_rate
        /// Loriot: combination of message.Dr;message.Snr;message.Freq;message.Toa
        /// </summary>
        public string DataCodingRate { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.CustomerData.loc.lat
        /// TTN: metadata.gateways.latitude of strongest gateway
        /// Loriot: lat of strongest gateway
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.CustomerData.loc.lon
        /// TTN: metadata.gateways.longitude of strongest gateway
        /// Loriot: lon of strongest gateway
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.Lrrid
        /// TTN: metadata.gateways.gtw_id of strongest gateway
        /// Loriot: gweui of strongest gateway
        /// </summary>
        public string StrongestSourceId { get; set; }

        /// <summary>
        /// Actility: The number of found DevEUI_uplink.Lrrs.Lrr
        /// TTN: The number of found metadata.gateways
        /// Loriot: The Gws.Count of found gateways
        /// </summary>
        public int SourceIdCount { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.LrrRSSI
        /// TTN: metadata.gateways.rssi  of strongest gateway
        /// Loriot: message.Rssi
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public float? RSSI { get; set; }

        /// <summary>
        /// Actility: DevEUI_uplink.LrSNR
        /// TTN: metadata.gateways.snr  of strongest gateway
        /// Loriot: message.Snr
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public float? SNR { get; set; }


        public AdditionalNetworkInfo AdditionalNetworkInfo { get; set; }

        /// <summary>
        /// As long as we only use standard types in this flat hierarchy shallow copy should be good enough
        /// </summary>
        /// <returns>A shallow copy</returns>
        public LoRaMessage ShallowCopy()
        {
            return (LoRaMessage)MemberwiseClone();
        }
    }

    public class AdditionalNetworkInfo
    {
        public string NetworkDeviceName { get; set; }

        public string NetworkApplicationName { get; set; }

        public string RegionHint { get; set; }

    }
}
