using System;

namespace Mihaylov.Common
{
    public class Config
    {
        public delegate bool TryParseHandler<T>(string value, out T result);

        public static T GetEnvironmentVariable<T>(string key, TryParseHandler<T> tryParseHandler, T defaultValue)
        {
            string configValue = GetConfigValue(key);

            if (!string.IsNullOrWhiteSpace(configValue) && tryParseHandler(configValue, out T result))
            {
                return result;
            }

            return defaultValue;
        }

        public static string GetEnvironmentVariable(string key, string defaultValue)
        {
            return GetEnvironmentVariable(key, TryParseString, defaultValue);
        }

        public static string GetEnvironmentVariable(string key)
        {
            string configValue = GetConfigValue(key);

            if (string.IsNullOrWhiteSpace(configValue))
            {
                throw new ArgumentException($"'{key}' missing");
            }

            return configValue;
        }

        private static string GetConfigValue(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
        
        private static bool TryParseString(string input, out string output)
        {
            output = input;
            return true;
        }
    }
}
