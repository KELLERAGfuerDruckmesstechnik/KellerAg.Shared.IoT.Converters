﻿using KellerAg.Shared.Entities.Communication;

namespace KellerAg.Shared.Entities.Device
{
    public interface IDevice
    {
        string Id { get; }

        DeviceInfo DeviceInfo { get; }

        ICommunication Communication { get; }

        byte Address { get; }

        int FirmwareYear { get; }

        int FirmwareWeek { get; }

        bool IsSupported { get; }

        bool IsModbusSupported { get; }

        bool IsKellerbusSupported { get; }
    }
}