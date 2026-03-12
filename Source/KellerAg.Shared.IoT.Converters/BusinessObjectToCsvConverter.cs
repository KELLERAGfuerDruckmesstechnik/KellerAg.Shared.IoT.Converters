namespace KellerAg.Shared.IoT.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using JsonToBusinessObjects.DataContainers;

    internal static class BusinessObjectToCsvConverter
    {
        /// <summary>
        /// Converts a BusinessObjectRoot's measurements to TXT1 format.
        /// Each line contains the date/time (MM/DD/YYYY HH:MM:SS) followed by space-separated
        /// measurement values for each channel (ordered by channel number).
        /// </summary>
        /// <param name="businessObjectRoot">The business object containing channel measurement data.</param>
        /// <returns>A string in TXT1 tabulated ASCII format, or an empty string if there are no measurements.</returns>
        public static string ConvertToTxt1(BusinessObjectRoot businessObjectRoot)
        {
            if (businessObjectRoot?.Measurements == null)
            {
                return string.Empty;
            }

            var channelData = businessObjectRoot.Measurements.DataPointsByChannel;
            if (channelData == null || channelData.Count == 0)
            {
                return string.Empty;
            }

            var sortedChannelKeys = channelData.Keys.OrderBy(k => k).ToList();

            var allTimestamps = channelData.Values
                .SelectMany(points => points.Select(p => p.Time))
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            var sb = new StringBuilder();
            foreach (var timestamp in allTimestamps)
            {
                sb.Append(timestamp.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture));

                foreach (var channelKey in sortedChannelKeys)
                {
                    var dataPoint = channelData[channelKey].FirstOrDefault(p => p.Time == timestamp);
                    if (dataPoint != null)
                    {
                        sb.Append(' ');
                        sb.Append(dataPoint.Value.ToString("G", CultureInfo.InvariantCulture));
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a BusinessObjectRoot's measurements to TXT2 format.
        /// Each line contains the timestamp (YYMMDDhhmmss), a variable identifier, and a value.
        /// Rows are ordered by timestamp, then by channel number.
        /// </summary>
        /// <param name="businessObjectRoot">The business object containing channel measurement data.</param>
        /// <param name="variableNames">
        /// Optional ordered list of variable names corresponding to each channel (by sorted channel index).
        /// When null or shorter than the number of channels, defaults to "CH{channelNumber}" for missing entries.
        /// </param>
        /// <returns>A string in TXT2 tabulated ASCII format, or an empty string if there are no measurements.</returns>
        public static string ConvertToTxt2(BusinessObjectRoot businessObjectRoot, IReadOnlyList<string> variableNames = null)
        {
            if (businessObjectRoot?.Measurements == null)
            {
                return string.Empty;
            }

            var channelData = businessObjectRoot.Measurements.DataPointsByChannel;
            if (channelData == null || channelData.Count == 0)
            {
                return string.Empty;
            }

            var sortedChannelKeys = channelData.Keys.OrderBy(k => k).ToList();

            var rows = new List<(DateTime Timestamp, int ChannelIndex, int ChannelKey, float Value)>();
            for (int i = 0; i < sortedChannelKeys.Count; i++)
            {
                int channelKey = sortedChannelKeys[i];
                foreach (var dataPoint in channelData[channelKey])
                {
                    rows.Add((dataPoint.Time, i, channelKey, dataPoint.Value));
                }
            }

            rows = rows.OrderBy(r => r.Timestamp).ThenBy(r => r.ChannelIndex).ToList();

            var sb = new StringBuilder();
            foreach (var row in rows)
            {
                string timestamp = row.Timestamp.ToString("yyMMddHHmmss", CultureInfo.InvariantCulture);

                string variableName = variableNames != null && row.ChannelIndex < variableNames.Count
                    ? variableNames[row.ChannelIndex]
                    : $"CH{row.ChannelKey}";

                string value = row.Value.ToString("G", CultureInfo.InvariantCulture);

                sb.AppendLine($"{timestamp} {variableName} {value}");
            }

            return sb.ToString();
        }
    }
}
