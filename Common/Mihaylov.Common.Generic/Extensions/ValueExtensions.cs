using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Mihaylov.Common
{
    /// <summary>
    /// Extension methods for common value operations: DateTime age and day calculations, birth-date and creation-date
    /// derivation, percentage computation, byte-size formatting/parsing, and string-to-enum conversion.
    /// </summary>
    public static class ValueExtensions
    {
        /// <summary>
        /// Gets the number of days represented by the specified nullable DateTime, or null if the value is null.
        /// </summary>
        /// <param name="date">The nullable DateTime whose days are computed; null yields null.</param>
        /// <returns>The number of days computed from date, or null if date has no value.</returns>
        public static int? GetDays(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            var age = date.Value.GetDays();

            return age;
        }

        /// <summary>
        /// Gets the number of whole days elapsed from the specified date to the current UTC time.
        /// </summary>
        /// <param name="date">The date from which to calculate elapsed days (compared to DateTime.UtcNow).</param>
        /// <returns>The number of whole days between date and the current UTC time; fractional days are discarded. Returns a
        /// negative value if date is in the future.</returns>
        public static int GetDays(this DateTime date)
        {
            var age = (int)DateTime.UtcNow.Subtract(date).TotalDays;

            return age;
        }

        /// <summary>
        /// Calculates the age in years from a nullable date of birth; returns null if no date is provided.
        /// </summary>
        /// <param name="dateOfBirth">Nullable date of birth to compute the age from.</param>
        /// <returns>The age in years, or null if dateOfBirth is null.</returns>
        public static int? GetAge(this DateTime? dateOfBirth)
        {
            if (!dateOfBirth.HasValue)
            {
                return null;
            }

            var age = dateOfBirth.Value.GetAge();

            return age;
        }

        /// <summary>
        /// Calculates the age in whole years based on the specified date of birth and the current UTC date.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth used to compute the age.</param>
        /// <returns>The age in whole years. Returns a negative value if the specified date is in the future.</returns>
        public static int GetAge(this DateTime dateOfBirth)
        {
            var dateNow = DateTime.UtcNow;

            int age = dateNow.Year - dateOfBirth.Year;
            if (dateOfBirth > dateNow.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// Gets the birth date from an explicit date or, if absent and permitted, from an age-based calculation.
        /// </summary>
        /// <param name="date">Explicit birth date, or null if not provided.</param>
        /// <param name="age">Age in years used to calculate the birth date when calculation is requested; may be null.</param>
        /// <param name="typeHasValue">True when the birth-date type is present; false causes no birth date to be returned.</param>
        /// <param name="isCalculated">True to calculate the birth date from age when no explicit date is provided.</param>
        /// <param name="currentDate">Reference date used when calculating the birth date from age.</param>
        /// <returns>The explicit date if present; otherwise the birth date calculated from age when allowed; otherwise null.</returns>
        public static DateTime? GetBirthDate(this DateTime? date, int? age, bool typeHasValue, bool isCalculated, DateTime currentDate)
        {
            if (!typeHasValue)
            {
                return null;
            }

            if (date.HasValue)
            {
                return date;
            }

            if (isCalculated)
            {
                return age.GetBirthDate(currentDate);
            }

            return null;
        }

        /// <summary>
        /// Returns true when a birth date is provided or when an age is provided and marked as calculated.
        /// </summary>
        /// <param name="date">The birth date value, or null if not provided.</param>
        /// <param name="age">The age in years, or null if not provided.</param>
        /// <param name="isCalculated">True when the age value is calculated rather than directly supplied.</param>
        /// <returns>True if a birth date is present or an age is present and isCalculated is true; otherwise false.</returns>
        public static bool IsBirthDateTypeValid(this DateTime? date, int? age, bool isCalculated)
        {
            var result = !date.HasValue && (!age.HasValue || !isCalculated);

            return !result;
        }

        /// <summary>
        /// Calculates the birth date from a nullable age and a reference date.
        /// </summary>
        /// <param name="age">Nullable age in years; null results in a null return.</param>
        /// <param name="date">Reference date used to compute the birth date.</param>
        /// <returns>The calculated birth date, or null if age is null.</returns>
        public static DateTime? GetBirthDate(this int? age, DateTime date)
        {
            if (!age.HasValue)
            {
                return null;
            }

            var birthDate = age.Value.GetBirthDate(date);
            return birthDate;
        }

        /// <summary>
        /// Calculates a birth date by subtracting the specified age in years and six months from the provided date.
        /// </summary>
        /// <param name="age">Age in years to subtract from the reference date.</param>
        /// <param name="date">Reference date from which the birth date is calculated; the time component is normalized to midnight.</param>
        /// <returns>A DateTime representing the calculated birth date obtained by subtracting age years and six months from the
        /// provided date; time component is midnight.</returns>
        public static DateTime GetBirthDate(this int age, DateTime date)
        {
            return date.Date.AddYears(-age).AddMonths(-6);
        }

        /// <summary>
        /// Gets the creation date for the specified nullable age.
        /// </summary>
        /// <param name="days">The nullable age in years used to calculate the creation date.</param>
        /// <returns>The calculated creation date, or null if age is null or negative.</returns>
        public static DateTime? GetCreateDate(this int? days)
        {
            if(!days.HasValue || days < 0)
            {
                return null;
            }

            return days.Value.GetCreateDate();
        }

        /// <summary>
        /// Gets the UTC date that is the specified number of days before the current UTC date.
        /// </summary>
        /// <param name="days">Number of days to subtract from the current UTC date; positive values produce past dates and negative values
        /// produce future dates.</param>
        /// <returns>A DateTime with Kind = Utc representing the computed date; the time component is 00:00:00 (midnight).</returns>
        public static DateTime GetCreateDate(this int days)
        {
            return DateTime.UtcNow.Date.AddDays(-days);
        }

        /// <summary>
        /// Calculates the integer percentage of processed items relative to total.
        /// </summary>
        /// <param name="processed">Number of processed items.</param>
        /// <param name="total">Total number of items.</param>
        /// <returns>Integer percentage obtained by truncating the fractional part of (processed / total * 100); values may be
        /// greater than 100 or negative depending on inputs.</returns>
        public static int GetPersentage(this long processed, long total)
        {
            return (int)((decimal)processed / total * 100);
        }

        /// <summary>
        /// Calculates the percentage of processed items relative to the total.
        /// </summary>
        /// <param name="processed">Processed count.</param>
        /// <param name="total">Total count.</param>
        /// <returns>Percentage of processed relative to total as an integer (0–100).</returns>
        public static int GetPersentage(this int processed, int total)
        {
            return ((long)processed).GetPersentage(total);
        }

        /// <summary>
        /// Converts the specified string to an enum value of type T by matching the string to an enum member name or to
        /// the Value of its EnumMemberAttribute.
        /// </summary>
        /// <typeparam name="T">The enum type to convert the string to; T is expected to be an enum.</typeparam>
        /// <param name="str">The string to convert; compared case-sensitively to enum member names and to any EnumMemberAttribute.Value.</param>
        /// <returns>The corresponding enum value of type T if a match is found; otherwise default(T).</returns>
        public static T ToEnum<T>(this string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                FieldInfo fieldInfo = enumType.GetField(name);
                EnumMemberAttribute[] attributes = (EnumMemberAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), true);

                string value = name;
                if (attributes.Length > 0)
                {
                    var enumMemberAttribute = attributes.Single();
                    value = enumMemberAttribute.Value;
                }

                if (value == str)
                {
                    return (T)Enum.Parse(enumType, name);
                }
            }

            //throw exception or whatever handling you want or
            return default(T);
        }

        /// <summary>
        /// Formats a byte count into a human-readable string using binary (IEC) prefixes with up to two decimal places.
        /// </summary>
        /// <param name="bytes">Number of bytes to format.</param>
        /// <returns>A string containing the value formatted with up to two decimal places and a binary unit suffix (B, KiB, MiB,
        /// GiB, TiB).</returns>
        public static string FormatBytes(this long bytes)
        {
            string[] Suffix = { "B", "KiB", "MiB", "GiB", "TiB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return string.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        /// <summary>
        /// Converts a size string containing a numeric value and a binary unit into a byte count.
        /// </summary>
        /// <param name="size">Size string in the format <![CDATA[<numeric> <unit>]]>, for example "1.5 GiB". Accepted units: B, KiB, MiB, GiB, TiB.</param>
        /// <returns>The total number of bytes represented by the input, or null if the input is null, empty, not in the expected
        /// two-part format, or uses an unsupported unit.</returns>
        public static long? GetBytes(this string size)
        {
            const long KiB = 1024;
            const long MiB = KiB * KiB;
            const long GiB = MiB * KiB;
            const long TiB = GiB * KiB;

            if (string.IsNullOrEmpty(size))
            {
                return null;
            }

            var sizeParts = size.Split(' ');
            if (sizeParts.Length != 2)
            {
                return null;
            }

            var value = decimal.Parse(sizeParts[0]);
            var unit = sizeParts[1];

            long multiplier;
            switch (unit)
            {
                case "B":
                    multiplier = 1;
                    break;
                case "KiB":
                    multiplier = KiB;
                    break;
                case "MiB":
                    multiplier = MiB;
                    break;
                case "GiB":
                    multiplier = GiB;
                    break;
                case "TiB":
                    multiplier = TiB;
                    break;
                default:
                    return null;
            }

            return (long)(value * multiplier);
        }
    }
}
