﻿using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Mihaylov.Common
{
    public static class ValueExtensions
    {
        public static int? GetDays(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            var age = date.Value.GetDays();

            return age;
        }

        public static int GetDays(this DateTime date)
        {
            var age = (int)DateTime.UtcNow.Subtract(date).TotalDays;

            return age;
        }


        public static int? GetAge(this DateTime? dateOfBirth)
        {
            if (!dateOfBirth.HasValue)
            {
                return null;
            }

            var age = dateOfBirth.Value.GetAge();

            return age;
        }

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

        public static bool IsBirthDateTypeValid(this DateTime? date, int? age, bool isCalculated)
        {
            var result = !date.HasValue && (!age.HasValue || !isCalculated);

            return !result;
        }

        public static DateTime? GetBirthDate(this int? age, DateTime date)
        {
            if (!age.HasValue)
            {
                return null;
            }

            var birthDate = age.Value.GetBirthDate(date);
            return birthDate;
        }

        public static DateTime GetBirthDate(this int age, DateTime date)
        {
            return date.Date.AddYears(-age).AddMonths(-6);
        }

        public static DateTime? GetCreateDate(this int? age)
        {
            if(!age.HasValue || age < 0)
            {
                return null;
            }

            return age.Value.GetCreateDate();
        }

        public static DateTime GetCreateDate(this int age)
        {
            return DateTime.UtcNow.Date.AddDays(-age);
        }


        public static int GetPersentage(this long processed, long total)
        {
            return (int)((decimal)processed / total * 100);
        }

        public static int GetPersentage(this int processed, int total)
        {
            return ((long)processed).GetPersentage(total);
        }

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
