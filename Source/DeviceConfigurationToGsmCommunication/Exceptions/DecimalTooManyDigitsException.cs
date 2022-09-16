using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceConfigurationToGsmCommunication.Exceptions
{
    using System;

    public class DecimalTooManyDigitsException : Exception
    {
        public DecimalTooManyDigitsException() : base()
        {
        }

        public DecimalTooManyDigitsException(string message) : base(message)
        {
        }
    }
}
