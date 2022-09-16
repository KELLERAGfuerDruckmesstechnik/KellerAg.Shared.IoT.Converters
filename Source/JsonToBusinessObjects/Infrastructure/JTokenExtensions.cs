namespace JsonToBusinessObjects.Infrastructure
{
    using System;
    using Logging;
    using Newtonsoft.Json.Linq;

    internal static class JTokenExtensions
    {
        public static void ExecuteIfAvailable<T>(this JToken jToken, string key, Action<T> function, ILogger logger)
        {
            if (jToken is JValue jValue)
            {
                throw new InvalidOperationException($"Can't get child from value token {jValue.Value}.");
            }

            T value;
            try
            {
                value = jToken.Value<T>(key);
            }
            catch (Exception e) when (e is InvalidCastException || e is FormatException)
            {
                logger.Error($"Exception occurred during conversion in Token with key '{key}'", e);
                return;
            }

            if (value != null)
            {
                function.Invoke(value);
            }
        }
    }
}