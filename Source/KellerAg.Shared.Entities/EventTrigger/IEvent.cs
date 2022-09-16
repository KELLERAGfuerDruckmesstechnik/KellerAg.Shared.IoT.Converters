using KellerAg.Shared.Entities.Channel;
using System;

namespace KellerAg.Shared.Entities.EventTrigger
{
    public interface IEvent
    {
        TimeSpan DetectionInterval { get; set; }
        RecordingEvent EventType { get; set; }
        TimeSpan MeasureInterval { get; set; }
        int MeasurementsForAverage { get; set; }
        double PrimaryValue { get; set; }
        double SecondaryValue { get; set; }
        ChannelInfo TriggerChannel { get; set; }
    }
}