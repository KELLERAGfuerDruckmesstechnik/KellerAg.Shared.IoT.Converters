namespace KellerAg.Shared.Entities.EventTrigger
{
    public enum RecordingEvent
    {
        None = -1,
        //Fixed interval is handled separately
        //FixInterval = 0,
        Interval = 1,
        OnAndOff = 2,
        Delta = 3,
        ChBiggerThan = 4,
        ChSmallerThan = 5

    }
}