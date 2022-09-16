namespace KellerAg.Shared.Entities.Calculations
{
    public class WaterCalculationSettingsViewModel
    {
        /// <summary>
        /// If you do not do cloud stuff: You can ignore this field.
        /// </summary>
        public int DeviceId { get; set; }

        public double Density { get; set; }
        public int DensityUnitId { get; set; }

        public double Gravity { get; set; }
        public int GravityUnitId { get; set; }

        public DepthCalculationSettingsViewModel DepthCalculationSettings { get; set; }

        public HeightCalculationSettingsViewModel HeightCalculationSettings { get; set; }

        public HeightAboveSeeLevelCalculationSettingsViewModel HeightAboveSeeLevelCalculationSettings { get; set; }
    }

    public class DepthCalculationSettingsViewModel
    {
        public int? CalculationId { get; set; }
        public double? Offset { get; set; }
        public int? OffsetUnitId { get; set; }
        public double? P1 { get; set; }
        public int? P1UnitId { get; set; }
        public double? P2 { get; set; }
        public int? P2UnitId { get; set; }
        public double? B { get; set; }
        public int? BUnitId { get; set; }
    }

    public class HeightCalculationSettingsViewModel
    {
        public int? CalculationId { get; set; }
        public double? Offset { get; set; }
        public int? OffsetUnitId { get; set; }
        public double? P1 { get; set; }
        public int? P1UnitId { get; set; }
        public double? P2 { get; set; }
        public int? P2UnitId { get; set; }
    }

    public class HeightAboveSeeLevelCalculationSettingsViewModel
    {
        public int? CalculationId { get; set; }
        public double? Offset { get; set; }
        public int? OffsetUnitId { get; set; }
        public double? P1 { get; set; }
        public int? P1UnitId { get; set; }
        public double? P2 { get; set; }
        public int? P2UnitId { get; set; }
        public double? A { get; set; }
        public int? AUnitId { get; set; }
        public double? B { get; set; }
        public int? BUnitId { get; set; }
    }
}