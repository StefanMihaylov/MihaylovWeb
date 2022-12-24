namespace Mihaylov.Common.Extensions
{
    using System;

    public static class NullableExtensions
    {
        public const string IncorectParsingMessage = "Incorrect format";
        public const string DefaultDateTimeFormat = "dd.MM.yyyy HH:mm:ss";
        public const string DefaultIntFormat = "G"; // standard numeric format specifier https://msdn.microsoft.com/en-us/library/8wch342y%28v=vs.110%29.aspx
        public const string DefaultDecimalFormat = "0.00";

        public static string ConvertToString(this DateTime? datetime, string formatString = null)
        {
            if (datetime == null)
            {
                return string.Empty;
            }
            else
            {
                return ConvertToString(datetime.Value, formatString);
            }
        }

        public static string ConvertToString(this DateTime datetime, string formatString = null)
        {
            if (datetime == DateTime.MinValue)
            {
                return IncorectParsingMessage;
            }
            else
            {
                formatString = formatString ?? DefaultDateTimeFormat;
                string result = datetime.ToString(formatString);
                return result;
            }
        }

        public static string ConvertToString(this int? number, string formatString = null)
        {
            if (number == null)
            {
                return string.Empty;
            }
            else
            {
                if (number.Value == int.MinValue)
                {
                    return IncorectParsingMessage;
                }
                else
                {
                    formatString = formatString ?? DefaultIntFormat;
                    return number.Value.ToString(formatString);
                }
            }
        }

        public static string ConvertToString(this decimal? number, string formatString = null, string unit = null)
        {
            if (number == null)
            {
                return string.Empty;
            }
            else
            {
                if (number.Value == decimal.MinValue)
                {
                    return IncorectParsingMessage;
                }
                else
                {
                    formatString = formatString ?? DefaultDecimalFormat;
                    string formatPattern = string.Format("{{0:{0}}} {{1}}", formatString);
                    string result = string.Format(formatPattern, number.Value, unit).Trim();
                    return result;
                }
            }
        }
    }
}
