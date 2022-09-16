namespace JsonToBusinessObjects.Conversion
{
    using System;
    using System.Collections.Generic;
    public class ChannelDataStorage
    {
        private readonly Dictionary<int, List<DataPoint>> _store = new Dictionary<int, List<DataPoint>>();

        public Dictionary<int, List<DataPoint>> DataPointsByChannel => new Dictionary<int, List<DataPoint>>(_store);

        public void StoreInChannel(int channelNumber, DataPoint dataPoint)
        {
            RetrieveListForChannel(channelNumber).Add(dataPoint);
        }

        public List<DataPoint> GetValuesForChannel(int channelNumber)
        {
            _store.TryGetValue(channelNumber, out List<DataPoint> list);
            return list ?? new List<DataPoint>();
        }

        private List<DataPoint> RetrieveListForChannel(int channelNumber)
        {
            if (_store.TryGetValue(channelNumber, out List<DataPoint> list))
            {
                return list;
            }

            // if not present, add new empty list and return it
            return _store[channelNumber] = new List<DataPoint>();
        }
    }

    public class DataPoint
    {
        public DataPoint(DateTime time, float value)
        {
            Time = time;
            Value = value;
        }

        public DateTime Time { get; }
        public float Value { get; }
    }
}