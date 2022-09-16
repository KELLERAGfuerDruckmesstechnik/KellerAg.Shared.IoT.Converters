using System;
using JsonToBusinessObjects.Infrastructure.Logging;
using Newtonsoft.Json;

namespace JsonToBusinessObjects.Conversion.LoRa
{
    public static class JsonConversionHelper
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };

        public static int ToInt(this string stringValue, ILogger logger)
        {
            if (int.TryParse(stringValue, out int intValue))
            {
                return intValue;
            }

            logger.Error($"Could not parse '{stringValue}' to a int value.");
            return int.MinValue;
        }

        public static float ToFloat(this string stringValue, ILogger logger)
        {
            if (float.TryParse(stringValue, out float floatValue))
            {
                return floatValue;
            }

            logger.Error($"Could not parse '{stringValue}' to a float value.");
            return float.MinValue;
        }

        public static DateTime ToDateTime(this string stringValue, ILogger logger)
        {
            if (DateTime.TryParse(stringValue, out DateTime dateTimeValue))
            {
                return dateTimeValue.ToUniversalTime();
            }

            logger.Error($"Could not parse '{stringValue}' to a DateTime value.");
            return DateTime.MinValue;
        }

    }
}