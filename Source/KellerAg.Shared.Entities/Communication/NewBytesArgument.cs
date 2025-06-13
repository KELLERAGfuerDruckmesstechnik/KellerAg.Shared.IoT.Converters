using System;

namespace KellerAg.Shared.Entities.Communication
{
    public class NewBytesArgument : EventArgs
    {
        public byte[] NewBytes { get; }

        public NewBytesArgument(byte[] newBytes)
        {
            NewBytes = newBytes;
        }
    }
}
