using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KellerAg.Shared.Entities.Communication.DemoDevices
{
    public class DemoDevice : ICommunication
    {
        public DemoDevice(string deviceName, string file)
        {
            Name = deviceName;
            Memory = new DeviceMemory(file);
            Configuration = new DeviceConfiguration(file);
            var rand = new Random();
            _current0 = rand.Next(20);
            _current1 = rand.Next(20);
            _current2 = rand.Next(20);
            _current3 = rand.Next(20);
            _current4 = rand.Next(20);
            _current5 = rand.Next(20);
            _currentDefault = rand.Next(20);
        }

        public string Name { get; }

        public bool EchoOn { get; set; }
        public bool AutoEcho { get; set; }
        public bool IsOpen => true;
        public bool IsBluetooth { get; set; }

        public int Speed => 9600;
        public object Interface { get; set; }
        public event EventHandler<NewBytesArgument> ReadContinuousOnByte;

        internal DeviceMemory Memory;

        internal DeviceConfiguration Configuration;

        private double _current0;
        private double _current1;
        private double _current2;
        private double _current3;
        private double _current4;
        private double _current5;
        private double _currentDefault;

        public void send(byte[] command, out byte[] rcfBuffer, int readByteCount)
        {
            rcfBuffer = new byte[0];
            byte[] buffer;
            if (command.Length < 4)
            {
                return;
            }
            var response = new[] { command[0], command[1] };

            switch (command[1])
            {
                case 68: // Read ROM
                    buffer = Memory.GetRow(command[2], command[3]);
                    if (command[4] == 0)
                    {
                        buffer = buffer.Take(8).ToArray();
                    }
                    break;
                case 73: // Read current value of channel
                    float chValue;
                    switch (command[2])
                    {
                        case 0: // probably Pd
                            chValue = Convert.ToSingle(Math.Sin(_current0));
                            _current0 += 0.1;
                            break;
                        case 1: // probably P1
                            chValue = Convert.ToSingle(Math.Sin(_current1));
                            _current1 += 0.1;
                            break;
                        case 2: // probably P2
                            chValue = Convert.ToSingle(Math.Sin(_current2));
                            _current2 += 0.1;
                            break;
                        case 3: // probably T
                            chValue = Convert.ToSingle(Math.Sin(_current3));
                            _current3 += 0.1;
                            break;
                        case 4: // probably TOB1
                            chValue = Convert.ToSingle(Math.Sin(_current4));
                            _current4 += 0.1;
                            break;
                        case 5: // probably TOB2
                            chValue = Convert.ToSingle(Math.Sin(_current5));
                            _current5 += 0.1;
                            break;
                        default:
                            chValue = Convert.ToSingle(Math.Sin(_currentDefault));
                            _currentDefault += 0.1;
                            break;
                    }

                    var values = BitConverter.GetBytes(chValue);
                    buffer = new[] { values[3], values[2], values[1], values[0], (byte)0 };
                    break;
                case 65: // special case because index is larger than byte
                    var indexInt = (command[2] << 8) + command[3];
                    buffer = Configuration.Get(command[1], indexInt, command[4]) ?? new byte[] { 0 };
                    break;
                case 96: //IOT commands
                    buffer = new byte[] { 0 };
                    break;
                default:
                    buffer = Configuration.Get(command[1], command[2]) ?? new byte[] { 0 };
                    break;
            }
            // Add address and function to response
            buffer = response.Concat(buffer).ToArray();
            rcfBuffer = Crc16ify(buffer, 0, buffer.Length);

        }


        public void ReadContinuous(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public byte[] Send(byte[] dataSend, int readByteCount)
        {
            throw new NotImplementedException();
        }

        public byte[] Send(byte[] dataSend, byte endSign)
        {
            throw new NotImplementedException();
        }

        public void send(byte[] command, out byte[] rcfBuffer, byte endSign)
        {
            throw new NotImplementedException();
        }

        public void open(object sender)
        {
        }

        public void close(object sender)
        {
        }

        public void setConfig(Dictionary<string, object> newConfig)
        {
            throw new NotImplementedException();
        }

        public bool setConfig(string key, object value)
        {
            throw new NotImplementedException();
        }

        public object getConfig(string key)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> getConfigCopy()
        {
            throw new NotImplementedException();
        }

        protected static byte[] Crc16ify(byte[] buffer, int offset, int bteCount)
        {
            const ushort polynom = 0xA001;

            ushort crc = 0xFFFF;

            for (int i = 0; i < bteCount; i++)
            {
                crc = (ushort)(crc ^ buffer[offset + i]);

                for (int n = 0; n < 8; n++)
                {
                    bool ex = crc % 2 == 1;
                    crc = (ushort)(crc / 2);
                    if (ex)
                        crc = (ushort)(crc ^ polynom);
                }
            }

            var calculatedCrc = new[] { (byte)(crc >> 8), (byte)(crc & 0x00ff) };
            return buffer.Concat(calculatedCrc).ToArray();
        }
    }
}
