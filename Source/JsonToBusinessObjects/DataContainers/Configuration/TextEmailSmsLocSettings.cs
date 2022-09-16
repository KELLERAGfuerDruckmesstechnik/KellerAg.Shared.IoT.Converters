namespace JsonToBusinessObjects.DataContainers.Configuration
{
    public class TextEmailSmsLocSettings
    {
        /// <summary>
        /// #b /a
        /// </summary>
        public string EmailAddress1 { get; set; }
        public string EmailAddress2 { get; set; }
        public string EmailAddress3 { get; set; }
        public string PasswordForQuerySms { get; set; }
        public string SimPin { get; set; }
        public string RecallForDataConnection { get; set; }
        public string SmsNumber1Measure { get; set; }
        public string SmsNumber2Alarm { get; set; }
        public string SmsNumber3Info { get; set; }
        public string SmsServiceCenterNr { get; set; }
        public string NetworkName { get; set; }
        public string OwnTelNumber { get; set; }
        public string LocationName { get; set; }
        public string SmsText1Measure { get; set; }
        public string SmsText2Alarm { get; set; }
        public string SmsText3AnswerCheck { get; set; }
        public string LongitudeText { get; set; }
        public string LatitudeText { get; set; }
        public string AltitudeText { get; set; }
        public string CellLocateLongitude { get; set; }
        public string CellLocateLatitude { get; set; }
        public string CellLocateAltitude { get; set; }
    }
}