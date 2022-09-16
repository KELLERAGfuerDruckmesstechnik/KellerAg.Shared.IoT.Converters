using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceConfigurationToGsmCommunication.Exceptions
{
    using System;

    public class EmptyGsmMessageException : Exception
    {
        public EmptyGsmMessageException() : base()
        {
        }

        public EmptyGsmMessageException(string message) : base(message)
        {
        }
    }
}
