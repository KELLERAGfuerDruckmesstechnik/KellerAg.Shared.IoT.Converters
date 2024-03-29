﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KellerAg.Shared.Entities.Communication.DemoDevices
{
    public class DeviceConfiguration
    {

        public DeviceConfiguration(string file)
        {
            _functionsWithoutIndex = new List<int> { 48, 69 };
            InitializeFile(file);
        }

        private byte[][][] _config;

        private readonly List<int> _functionsWithoutIndex;

        public byte[] Get(int function, int index)
        {
            index = _functionsWithoutIndex.Contains(function) ? 0 : index;
            if (function == 73)
            {
                //TODO generate random but clean data
                // Current value of channel
                float chValue = 0;
                switch (index)
                {
                    case 0: // probably Pd
                        chValue = (float)0.1;
                        break;
                    case 1: // probably P1
                        chValue = (float)1.1;
                        break;
                    case 2: // probably P2
                        chValue = 1;
                        break;
                    case 3: // probably T
                        chValue = (float)24.8;
                        break;
                    case 4: // probably TOB1
                        chValue = (float)25.2;
                        break;
                    case 5: // probably TOB2
                        chValue = (float)26.5;
                        break;
                }

                var values = BitConverter.GetBytes(chValue);
                return new[] { values[0], values[1], values[2], values[3], (byte)0 };
            }

            return _config[function][index];
        }

        private void ParseConfig(string[] fileContent)
        {
            _config = new byte[101][][];
            foreach (var content in fileContent)
            {

                if (content.StartsWith("//"))
                {
                    // Configuration finished
                    break;
                }

                var configParts = content.Split('|');
                var function = int.Parse(configParts[0].Trim());
                var index = int.Parse(configParts[1].Trim());
                var valuesString = configParts[2].Trim().Trim().Split(' ');
                var valuesArray = valuesString.Select(byte.Parse).ToArray();

                if (_config[function] == null)
                {
                    _config[function] = new byte[index + 1][];
                }
                else if (_config[function].Length <= index)
                {
                    Array.Resize(ref _config[function], index + 1);
                }
                _config[function][index] = valuesArray;

            }
        }

        private void InitializeFile(string file)
        {
            var fileContent = File.ReadAllLines(file);
            ParseConfig(fileContent);
        }
    }
}
