namespace JsonToBusinessObjects.DataContainers.Configuration
{
    public class FloatingPointMeasurementSettings
    {
        public float AlarmOn { get; set; }
        public float AlarmOff { get; set; }
        public float AlarmDelta { get; set; }
        public float MultiplierTemperatureChannels { get; set; }
        public float MultiplierPressureChannels { get; set; }
        public float Val1OnEventLogging { get; set; }
        public float Val2OffEventLogging { get; set; }
        public float Val3DeltaEventLogging { get; set; }
        public float Val100WlcEnabled { get; set; }
        public float Val101WlcLength { get; set; }
        public float Val102WlcHeight { get; set; }
        public float Val103CalcOffset { get; set; }
        public float Val104WlcDensity { get; set; }
        public float Val105OflWidth { get; set; }
        public float Val106OflAngle { get; set; }
        public float Val107OflFormFactor { get; set; }
        public float Val108OflMinCalc { get; set; }
        /// <summary>
        /// #d/v
        /// Water Level Config. Reserve (index 19)
        /// </summary>
        public float Val109Fu3031Index19 { get; set; }
        /// <summary>
        /// #d/w
        /// Water Level Config. Reserve (index 20)
        /// </summary>
        public float Val110Fu3031Index20 { get; set; }
        public float LongitudeFu3031Index24 { get; set; }
        public float LatitudeFu3031Index45 { get; set; }
        public float AltitudeFu3031Index26 { get; set; }
    }
}