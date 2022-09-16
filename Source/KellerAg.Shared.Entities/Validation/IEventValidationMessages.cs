namespace KellerAg.Shared.Entities.Validation
{
    public interface IEventValidationMessages
    {
        string DetectionIntervalMustBeBiggerThanZero { get; }
        string DetectionIntervalMustBeShorterThan { get; }
        string MeasureIntervalMustBeBiggerThanZero { get; }
        string MeasureIntervalMustBeShorterThan { get; }
        string MeasurementsForAverageBetween1And255 { get; }
        string MeasurementsForAverageMustBeSmallerThanInterval { get; }
    }
}
