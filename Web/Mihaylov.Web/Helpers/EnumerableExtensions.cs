using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mihaylov.Web.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<SelectListItem> GetEnumDropdownListItems<TEnum>(this TEnum type) where TEnum : struct, Enum
        {
            var result = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(v => new SelectListItem(v.ToString(), ((int)(object)v).ToString()))
                .ToList();

            return result;
        }

        public static IEnumerable<SelectListItem> GetDropdownListItems<T>(this IEnumerable<T> collection, Expression<Func<T, object>> valueExpression, Expression<Func<T, object>> textExpression)
        {
            return collection.GetDropdownListItems<T>(valueExpression, "{0}", textExpression);
        }

        public static IEnumerable<SelectListItem> GetDropdownListItems<T>(this IEnumerable<T> collection, Expression<Func<T, object>> valueExpression, string formatText, params Expression<Func<T, object>>[] textExpressions)
        {
            var list = new List<SelectListItem>();

            foreach (T item in collection)
            {
                Func<T, object> valueMethod = valueExpression.Compile();
                var textParams = new List<object>();
                foreach (var textExpression in textExpressions)
                {
                    Func<T, object> textMetod = textExpression.Compile();
                    textParams.Add(textMetod(item));
                }

                var newListItem = new SelectListItem()
                {
                    Value = valueMethod(item).ToString(),
                    Text = string.Format(formatText, textParams.ToArray())
                };

                list.Add(newListItem);
            }

            return list;
        }
    }
}
