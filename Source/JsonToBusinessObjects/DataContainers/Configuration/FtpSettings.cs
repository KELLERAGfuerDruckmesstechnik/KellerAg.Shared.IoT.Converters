namespace JsonToBusinessObjects.DataContainers.Configuration
{
    // ReSharper disable InconsistentNaming
    public class FtpSettings
    {
        public string FtpServerName { get; set; }
        public string FtpLoginUserName { get; set; }
        public string FtpPassword { get; set; }
        public string FtpAccount { get; set; }
        public string FtpSourceControlPort { get; set; }
        public string FtpDestinationControlPort { get; set; }
        public string FtpSourceDataPort { get; set; }
        public string FtpFilePath { get; set; }
    }
}