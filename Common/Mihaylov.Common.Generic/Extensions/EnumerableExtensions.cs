using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mihaylov.Common.Generic.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<Tout> GetEnumListItems<TEnum, Tout>(this TEnum type, Func<string, string, Tout> resultSelect) where TEnum : struct, Enum
        {
            var result = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(v =>
                {
                    string text = v.ToString();
                    string value = ((int)(object)v).ToString();
                    Tout model = resultSelect(text, value);

                    return model;
                })
                .ToList();

            return result;
        }

        public static IEnumerable<Tout> GetListItems<Tin, Tout>(this IEnumerable<Tin> collection,
            Expression<Func<string, string, Tout>> resultSelect,
            Expression<Func<Tin, object>> valueExpression, Expression<Func<Tin, object>> textExpression)
        {
            return collection.GetListItems<Tin, Tout>(resultSelect, valueExpression, "{0}", textExpression);
        }

        public static IEnumerable<Tout> GetListItems<Tin, Tout>(this IEnumerable<Tin> collection,
            Expression<Func<string, string, Tout>> resultSelect, Expression<Func<Tin, object>> valueExpression, 
            string formatText, params Expression<Func<Tin, object>>[] textExpressions)
        {
            var list = new List<Tout>();

            Func<string, string, Tout> resultMethod = resultSelect.Compile();
            Func<Tin, object> valueMethod = valueExpression.Compile();

            foreach (Tin item in collection)
            {
                var textParams = new List<object>();
                foreach (var textExpression in textExpressions)
                {
                    Func<Tin, object> textMetod = textExpression.Compile();
                    textParams.Add(textMetod(item));
                }

                var text = string.Format(formatText, textParams.ToArray());
                var value = valueMethod(item).ToString();
                
                Tout result = resultMethod(text, value);

                list.Add(result);
            }

            return list;
        }

        public static string ToQueryString(this IDictionary<string, string> dictionary, bool addDelimiter = true)
        {
            var result = string.Join("&", dictionary.Select(kv => $"{kv.Key.ToLower()}={kv.Value}"));

            if (addDelimiter && dictionary.Count > 0)
            {
                result = $"?{result}";
            }

            return result;
        }
    }
}
