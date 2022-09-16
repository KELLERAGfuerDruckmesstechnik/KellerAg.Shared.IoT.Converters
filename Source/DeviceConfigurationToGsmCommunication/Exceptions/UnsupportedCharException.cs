using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceConfigurationToGsmCommunication.Exceptions
{
    using System;

    public class UnsupportedCharException : Exception
    {
        public UnsupportedCharException() : base()
        {
        }

        public UnsupportedCharException(string message) : base(message)
        {
        }
    }
}
