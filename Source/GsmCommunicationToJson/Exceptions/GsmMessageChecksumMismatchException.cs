namespace GsmCommunicationToJson.Exceptions
{
    using System;

    public class GsmMessageChecksumMismatchException : Exception
    {
        public string MessageWithInvalidFormat { get; }

        public GsmMessageChecksumMismatchException(string message, string messageWithInvalidFormat) : base(message)
        {
            this.MessageWithInvalidFormat = messageWithInvalidFormat;
        }
    }
}