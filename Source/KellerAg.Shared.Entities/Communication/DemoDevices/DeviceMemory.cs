using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KellerAg.Shared.Entities.Communication.DemoDevices
{
    public class DeviceMemory
    {
        public DeviceMemory(List<byte[]> fileRows)
        {
            _fileBytes = fileRows;
            //InitializeFile(fileName);
        }

        public DeviceMemory(string file)
        {
            InitializeFile(file);
        }
        private List<byte[]> _fileBytes;

        public int Rows => _fileBytes.Count;

        public byte[] GetRow(int row)
        {
            if (_fileBytes.Count > row)
            {
                return _fileBytes[row];
            }

            return new byte[] { };
        }

        public byte[] GetRow(int pageH, int pageL)
        {
            return GetRow(pageH * 256 + pageL);
        }

        private void InitializeFile(string file)
        {
            _fileBytes = new List<byte[]>();
            var fileContent = File.ReadAllLines(file);
            var configurationFinished = false;
            foreach (var content in fileContent)
            {
                if (!configurationFinished && content.StartsWith("//"))
                {
                    configurationFinished = true;
                    continue;
                }

                if (configurationFinished)
                {
                    var fileRow = content.Split(' ');
                    var rowArray = fileRow.Select(byte.Parse).ToArray();
                    _fileBytes.Add(rowArray);
                }
            }
        }
    }
}
