using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Mihaylov.Common.Generic.Extensions
{
    public static class ValueExtensions
    {
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


        public static DateTime? GetBirthDate(this int? age)
        {
            if (!age.HasValue)
            {
                return null;
            }

            var birthDate = age.Value.GetBirthDate();
            return birthDate;
        }

        public static DateTime GetBirthDate(this int age)
        {
            return DateTime.UtcNow.Date.AddYears(-age).AddMonths(-6);
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
    }
}
