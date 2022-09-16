﻿using KellerAg.Shared.Entities.Communication;

namespace KellerAg.Shared.Entities.Device
{
    public class Device : IDevice
    {
        public Device(string id, DeviceInfo deviceInfo, ICommunication communication, bool isSupported = true)
        {
            Id = id;
            DeviceInfo = deviceInfo;
            Communication = communication;
            IsSupported = isSupported;
        }

        public Device()
        {
        }

        private byte _address;

        public byte Address
        {
            get => _address;
            set => _address = value == 0 ? (byte) 250 : value;
        }

        public ICommunication Communication { get; set; }

        public DeviceInfo DeviceInfo { get; set; }

        public string Id { get; }

        public bool IsSupported { get; }

    }
}