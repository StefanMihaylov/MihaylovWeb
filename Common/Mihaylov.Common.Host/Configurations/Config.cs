using System;

namespace Mihaylov.Common;

/// <summary>
/// Provides static helpers for reading configuration from environment variables with typed parsing and optional
/// defaults.
/// </summary>
public class Config
{
    /// <summary>
    /// Attempts to parse the specified string into a value of type T.
    /// </summary>
    /// <typeparam name="T">The type of value to parse.</typeparam>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">When the delegate returns, contains the parsed value if parsing succeeded; otherwise, the default value of T.</param>
    /// <returns>true if parsing succeeded; otherwise, false.</returns>
    public delegate bool TryParseHandler<T>(string value, out T result);

    /// <summary>
    /// Retrieves the environment variable with the specified key, parses it using the provided try-parse handler, and
    /// returns the parsed value or the supplied default.
    /// </summary>
    /// <typeparam name="T">The type of the value to return.</typeparam>
    /// <param name="key">The environment variable name.</param>
    /// <param name="tryParseHandler">A delegate that attempts to parse the environment variable string into T; returns true and outputs the parsed
    /// value on success.</param>
    /// <param name="defaultValue">Fallback value returned when the environment variable is missing, consists only of whitespace, or parsing fails.</param>
    /// <returns>The parsed value when the environment variable is present and parsing succeeds; otherwise the supplied
    /// defaultValue.</returns>
    public static T GetEnvironmentVariable<T>(string key, TryParseHandler<T> tryParseHandler, T defaultValue)
    {
        string configValue = Environment.GetEnvironmentVariable(key);

        if (!string.IsNullOrWhiteSpace(configValue) && tryParseHandler(configValue, out T result))
        {
            return result;
        }

        return defaultValue;
    }

    /// <summary>
    /// Gets the environment variable with the specified key and returns its string value or the provided default.
    /// </summary>
    /// <param name="key">The name of the environment variable to retrieve.</param>
    /// <param name="defaultValue">The value to return if the environment variable is not found or cannot be parsed.</param>
    /// <returns>The environment variable value if available and parsed successfully; otherwise, defaultValue.</returns>
    public static string GetEnvironmentVariable(string key, string defaultValue)
    {
        return GetEnvironmentVariable(key, TryParseString, defaultValue);
    }

    /// <summary>
    /// Gets the value of the specified environment variable.
    /// </summary>
    /// <param name="key">The name of the environment variable to retrieve.</param>
    /// <returns>The value of the environment variable.</returns>
    /// <exception cref="ArgumentException">Thrown if the environment variable is not set or contains only whitespace.</exception>
    public static string GetEnvironmentVariable(string key)
    {
        string configValue = Environment.GetEnvironmentVariable(key);

        if (string.IsNullOrWhiteSpace(configValue))
        {
            throw new ArgumentException($"'{key}' missing");
        }

        return configValue;
    }
   
    private static bool TryParseString(string input, out string output)
    {
        output = input;
        return true;
    }
}
