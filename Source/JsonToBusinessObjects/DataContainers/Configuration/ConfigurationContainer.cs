namespace JsonToBusinessObjects.DataContainers.Configuration
{
    public class ConfigurationContainer
    {
        public GprsSettings GprsSettings { get; set; }
        public TextEmailSmsLocSettings TextEmailSmsLocSettings { get; set; }
        public MeasurementSettings MeasurementSettings { get; set; }
        public FloatingPointMeasurementSettings FloatingPointMeasurementSettings { get; set; }
        public FtpSettings FtpSettings { get; set; }
        public MeasurementSettings2 MeasurementSettings2 { get; set; }
    }
}