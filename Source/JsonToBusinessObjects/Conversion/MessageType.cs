namespace JsonToBusinessObjects.Conversion
{
    public enum MessageType
    {
        UnknownMessage = 0,
        MeasurementMessage = 1,
        AlarmMessage = 2,
        ConfigurationMessageWithoutAck =3,
        ConfigurationMessageWithAck = 4,
        RecordDataMessage,
        RequestedRecordDataMessage,
        ErrorReportMessage,
        InformationMessage,
        AnswerFromRequestMessage
    }
}