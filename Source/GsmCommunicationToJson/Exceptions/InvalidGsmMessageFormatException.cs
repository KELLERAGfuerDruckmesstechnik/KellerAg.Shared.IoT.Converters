namespace GsmCommunicationToJson.Exceptions
{
    using System;

    public class InvalidGsmMessageFormatException : Exception
    {
        public string MessageWithInvalidFormat { get; }

        public InvalidGsmMessageFormatException(string message, string messageWithInvalidFormat) : base(message)
        {
            this.MessageWithInvalidFormat = messageWithInvalidFormat;
        }   
    }
}