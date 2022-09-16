namespace JsonToBusinessObjects.DataContainers.Configuration
{
    // ReSharper disable InconsistentNaming
    public class GprsSettings
    {
        public string GprsAPN { get; set; }
        public string GprsID { get; set; }
        public string GprsPassword { get; set; }
        public string GprsDNS { get; set; }
        public string SmtpShowedName { get; set; }
        public string PopUsername { get; set; }
        public string PopPassword { get; set; }
        public string OptSmtpUsername { get; set; }
        public string OptSmtpPassword { get; set; }
        public string Pop3Server { get; set; }
        public string Pop3Port { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public string ReturnAddress { get; set; }

        //Since 18.37

        /// <summary>
        /// #a/o
        /// </summary>
        public string CellularModuleId { get; set; }
        /// <summary>
        /// #a/p
        /// </summary>
        public string CellularModuleRevisionId { get; set; }
        /// <summary>
        /// #a/q
        /// </summary>
        public string CellularModuleSerialNumberIMEI { get; set; }
        /// <summary>
        /// #a/r
        /// </summary>
        public string CellularSIMCardId { get; set; }
        /// <summary>
        /// #a/s
        /// </summary>
        public string CellularSIMCardSubscriberId { get; set; }
    }
}