using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mihaylov.Web.Common.Extensions
{
    public static class IEnumerableExtensions
    {
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
