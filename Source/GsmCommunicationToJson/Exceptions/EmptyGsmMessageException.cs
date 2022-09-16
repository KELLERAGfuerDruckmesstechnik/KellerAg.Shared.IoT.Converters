namespace GsmCommunicationToJson.Exceptions
{
    using System;

    public class EmptyGsmMessageException : Exception
    {
        public EmptyGsmMessageException(string message) : base(message)
        {
        }   
    }
}