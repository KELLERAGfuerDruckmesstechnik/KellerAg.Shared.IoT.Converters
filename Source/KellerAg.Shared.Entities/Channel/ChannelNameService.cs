using KellerAg.Shared.Entities.Device;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace KellerAg.Shared.Entities.Channel
{
    public class ChannelNameService : INotifyPropertyChanged, IChannelNameService
    {
        private readonly ReaderWriterLockSlim _readWriterLock = new ReaderWriterLockSlim();
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

        public ChannelNameService()
        {
            _deviceSpecificInfoCache = new Dictionary<string, ChannelInfo[]>();
            _typeSpecificInfoCache = new Dictionary<DeviceType, ChannelInfo[]>();
            DeviceSpecificNames = new Dictionary<string, Dictionary<ChannelType, string>>();
            DeviceTypeSpecificNames = new Dictionary<DeviceType, Dictionary<ChannelType, string>>();
            GlobalNames = new Dictionary<ChannelType, string>();
        }

        public Dictionary<string, Dictionary<ChannelType, string>> DeviceSpecificNames
        {
            get
            {
                _readWriterLock.EnterReadLock();
                try
                {
                    return _deviceSpecificNames;
                }
                finally
                {
                    _readWriterLock.ExitReadLock();
                }
            }
            set
            {
                _readWriterLock.EnterWriteLock();
                try
                {
                    _deviceSpecificNames = value;
                    _deviceSpecificInfoCache.Clear();
                }
                finally
                {
                    _readWriterLock.ExitWriteLock();
                    OnPropertyChanged(nameof(DeviceSpecificNames));
                }
            }
        }

        public Dictionary<DeviceType, Dictionary<ChannelType, string>> DeviceTypeSpecificNames
        {
            get
            {
                _readWriterLock.EnterReadLock();
                try
                {
                    return _deviceTypeSpecificNames;
                }
                finally
                {
                    _readWriterLock.ExitReadLock();
                }
            }
            set
            {
                _readWriterLock.EnterWriteLock();
                try
                {
                    _deviceTypeSpecificNames = value;
                    _deviceSpecificInfoCache.Clear();
                    _typeSpecificInfoCache.Clear();
                }
                finally
                {
                    _readWriterLock.ExitWriteLock();
                    OnPropertyChanged(nameof(DeviceTypeSpecificNames));
                }
            }
        }

        public Dictionary<ChannelType, string> GlobalNames
        {
            get
            {
                _readWriterLock.EnterReadLock();
                try
                {
                    return _globalNames;
                }
                finally
                {
                    _readWriterLock.ExitReadLock();
                }
            }
            set
            {
                _readWriterLock.EnterWriteLock();
                try
                {
                    _globalNames = value;
                    _deviceSpecificInfoCache.Clear();
                    _typeSpecificInfoCache.Clear();
                }
                finally
                {
                    _readWriterLock.ExitWriteLock();
                    OnPropertyChanged(nameof(GlobalNames));
                }

            }
        }

        private readonly Dictionary<string, ChannelInfo[]> _deviceSpecificInfoCache;
        private readonly Dictionary<DeviceType, ChannelInfo[]> _typeSpecificInfoCache;
        private Dictionary<string, Dictionary<ChannelType, string>> _deviceSpecificNames;
        private Dictionary<DeviceType, Dictionary<ChannelType, string>> _deviceTypeSpecificNames;
        private Dictionary<ChannelType, string> _globalNames;

        public Dictionary<ChannelType, string> GetChannelNames(IDevice device)
        {
            var dict = new Dictionary<ChannelType, string>();
            foreach (var info in GetChannelsWithNames(device))
            {
                dict.Add(info.ChannelType, info.Name);
            }

            return dict;
        }

        public ChannelInfo GetChannelWithName(IDevice device, ChannelType type)
        {
            var channels = GetChannelsWithNames(device);

            return channels.FirstOrDefault(x => x.ChannelType == type);
        }

        public ChannelInfo GetChannelWithName(string deviceId, DeviceType deviceType, ChannelType channelType)
        {
            var channels = GetChannelsWithNames(deviceId, deviceType);

            return channels.FirstOrDefault(x => x.ChannelType == channelType);
        }

        public ChannelInfo[] GetChannelsWithNames(IDevice device)
        {
            return GetChannelsWithNames(device.Id, device.DeviceInfo.DeviceType);
        }

        public ChannelInfo[] GetChannelsWithNames(string deviceId, DeviceType deviceType)
        {
            _cacheLock.EnterReadLock();
            try
            {
                if (_deviceSpecificInfoCache.ContainsKey(deviceId))
                {
                    return _deviceSpecificInfoCache[deviceId];
                }
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }

            var channels = GetChannelsWithNames(deviceType);

            var dict = DeviceSpecificNames.SingleOrDefault(x => x.Key == deviceId).Value;
            if (dict != null)
            {
                foreach (var entry in dict)
                {
                    var channel = channels.FirstOrDefault(x => x.ChannelType == entry.Key);
                    if (channel != null)
                    {
                        channel.Name = entry.Value;
                    }
                }
            }

            _cacheLock.EnterWriteLock();
            try
            {
                _deviceSpecificInfoCache.Add(deviceId, channels);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }

            return channels;
        }

        public ChannelInfo[] GetChannelsWithNames(DeviceType deviceType)
        {
            _cacheLock.EnterReadLock();
            try
            {
                if (_typeSpecificInfoCache.ContainsKey(deviceType))
                {
                    return _typeSpecificInfoCache[deviceType];
                }
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }

            var channels = GetChannelsWithGlobalNames();

            var dict = DeviceTypeSpecificNames.SingleOrDefault(x => x.Key == deviceType).Value;
            if (dict != null)
            {
                foreach (var entry in dict)
                {
                    var channel = channels.FirstOrDefault(x => x.ChannelType == entry.Key);
                    if (channel != null)
                    {
                        channel.Name = entry.Value;
                    }
                }
            }

            _cacheLock.EnterWriteLock();
            try
            {
                _typeSpecificInfoCache.Add(deviceType, channels);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }

            return channels;
        }

        private ChannelInfo[] GetChannelsWithGlobalNames()
        {
            ChannelInfo[] channels = ChannelInfo.GetNewChannelsInstance();

            foreach (var entry in GlobalNames)
            {
                var channel = channels.FirstOrDefault(x => x.ChannelType == entry.Key);
                if (channel != null)
                {
                    channel.Name = entry.Value;
                }
            }

            return channels;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
