namespace KellerAg.Shared.Entities.Calculations
{
    public enum CalculationParameter
    {
        Offset,
        Density,
        Gravity,

        /// <summary>
        /// Especially, when using multisensor measurementDefinitionId like 55-59 the target calculation might be another like 34-36. This information must be stored
        /// The calculation is a default calculation, that has a calculation parameters for eg 34 but this property shows that it corresponds to one of the multisensor channels.
        /// </summary>
        CorrespondingMeasurementDefinitionId,

        ChannelId,

        /// <summary>
        /// Former "P1" in (Pd=P1-P2)
        /// </summary>
        HydrostaticPressureMeasurementDefinitionId,

        /// <summary>
        /// Former "P2" in (Pd=P1-P2)
        /// </summary>
        BarometricPressureMeasurementDefinitionId,
        UseBarometricPressureToCompensate,

        /// <summary>
        /// Former B
        /// </summary>
        InstallationLength,
        /// <summary>
        /// Former A
        /// </summary>
        HeightOfWellheadAboveSea,

        WallHeight,
        FormFactor,
        FormWidth,
        FormAngle,
        Area,
        Width,
        Height,
        Length,
        TankType,
        From,
        To,
    }
}