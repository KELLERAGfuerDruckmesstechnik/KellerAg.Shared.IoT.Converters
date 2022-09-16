using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceConfigurationToGsmCommunication.Exceptions
{
    using System;

    public class CharValueTooLongException : Exception
    {
        public CharValueTooLongException() : base()
        {
        }

        public CharValueTooLongException(string message) : base(message)
        {
        }
    }
}
