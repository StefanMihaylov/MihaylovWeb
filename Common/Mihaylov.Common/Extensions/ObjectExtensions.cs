namespace Mihaylov.Common.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ObjectExtensions
    {
        public static T CastTo<T>(this object obj)
        {
            var result = Activator.CreateInstance(typeof(T));

            foreach (var property in obj.GetType().GetProperties())
            {
                try
                {
                    result.GetType().GetProperty(property.Name).SetValue(result, property.GetValue(obj));
                }
                catch
                {
                    continue;
                }
            }

            return (T)result;
        }

        public static T CastTo<T>(this T collection, object obj)
        {
            return (T)obj;
        }
    }
}
