using KellerAg.Shared.Entities.Device;
using System.Collections.Generic;
using System.ComponentModel;

namespace KellerAg.Shared.Entities.Channel
{
    public interface IChannelNameService
    {
        Dictionary<string, Dictionary<ChannelType, string>> DeviceSpecificNames { get; set; }
        Dictionary<DeviceType, Dictionary<ChannelType, string>> DeviceTypeSpecificNames { get; set; }
        Dictionary<ChannelType, string> GlobalNames { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        Dictionary<ChannelType, string> GetChannelNames(IDevice device);
        ChannelInfo[] GetChannelsWithNames(DeviceType deviceType);
        ChannelInfo[] GetChannelsWithNames(IDevice device);
        ChannelInfo[] GetChannelsWithNames(string deviceId, DeviceType deviceType);
        ChannelInfo GetChannelWithName(IDevice device, ChannelType type);
        ChannelInfo GetChannelWithName(string deviceId, DeviceType deviceType, ChannelType channelType);
    }
}