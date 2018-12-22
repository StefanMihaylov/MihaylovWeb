using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Mihaylov.Common.Extensions
{
    public static class EnumExtentions
    {
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
