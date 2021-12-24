using System;

namespace Mihaylov.Common
{
    public class Config
    {
        public static T GetEnvironmentVariable<T>(string key, TryParseHandler<T> tryParseHandler, T defaultValue)
        {
            var configValue = Environment.GetEnvironmentVariable(key);

            if (!string.IsNullOrWhiteSpace(configValue) && tryParseHandler(configValue, out T result))
            {
                return result;
            }

            return defaultValue;
        }

        public delegate bool TryParseHandler<T>(string value, out T result);
    }
}
