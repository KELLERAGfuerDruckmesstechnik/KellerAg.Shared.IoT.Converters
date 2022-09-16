using KellerAg.Shared.Entities.Channel;
using KellerAg.Shared.Entities.Units;
using System;

namespace KellerAg.Shared.Entities.EventTrigger
{
    public class Event : IEvent
    {
        public Event()
        {
            TriggerChannel = new ChannelInfo(ChannelType.Undefined, 0, "", "", "", UnitType.Unknown);
        }

        public RecordingEvent EventType { get; set; }

        public TimeSpan DetectionInterval { get; set; }

        public TimeSpan MeasureInterval { get; set; }

        public double PrimaryValue { get; set; }

        public double SecondaryValue { get; set; }

        public ChannelInfo TriggerChannel { get; set; }

        public int MeasurementsForAverage { get; set; }
    }
}
