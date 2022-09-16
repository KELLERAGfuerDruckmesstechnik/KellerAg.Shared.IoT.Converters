namespace JsonToBusinessObjects.Conversion.Messages
{
    using System;

    public class ConversionMessage
    {
        public string Message { get; }
        public Exception Exception { get; }

        public ConversionMessage(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }
    }
}