using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides extension methods for enumerable and dictionary types to project enums and collections into result
    /// models, build query strings, and execute actions for each element.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Generate a sequence of Tout items for every member of the enum TEnum by applying the provided selector to
        /// each member's name and numeric value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type whose members are enumerated.</typeparam>
        /// <typeparam name="Tout">The result type produced by the selector.</typeparam>
        /// <param name="type">An instance of TEnum that identifies the enum type to enumerate.</param>
        /// <param name="resultSelect">A function that receives an enum member's name and its numeric value (as a string) and returns a Tout
        /// instance.</param>
        /// <returns>An <![CDATA[IEnumerable<Tout>]]> containing the results of invoking resultSelect for each enum member.</returns>
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

        /// <summary>
        /// Projects each element of the source sequence into a result object that represents a list item using the
        /// specified value and text selectors.
        /// </summary>
        /// <typeparam name="Tin">The type of elements in the source sequence.</typeparam>
        /// <typeparam name="Tout">The type of elements in the returned sequence.</typeparam>
        /// <param name="collection">The source sequence of elements to project.</param>
        /// <param name="resultSelect">An expression that creates a result object from a value string and a text string.</param>
        /// <param name="valueExpression">An expression that selects the value member from a source element.</param>
        /// <param name="textExpression">An expression that selects the text member from a source element.</param>
        /// <returns>An <![CDATA[IEnumerable<Tout>]]> containing the projected list items.</returns>
        public static IEnumerable<Tout> GetListItems<Tin, Tout>(this IEnumerable<Tin> collection,
            Expression<Func<string, string, Tout>> resultSelect,
            Expression<Func<Tin, object>> valueExpression, Expression<Func<Tin, object>> textExpression)
        {
            return collection.GetListItems<Tin, Tout>(resultSelect, valueExpression, "{0}", textExpression);
        }

        /// <summary>
        /// Project a sequence of source elements into a sequence of Tout by formatting one or more member values into a
        /// text string and combining that text with a value string via the provided selector.
        /// </summary>
        /// <typeparam name="Tin">Type of elements in the source collection.</typeparam>
        /// <typeparam name="Tout">Type of elements returned by the projection.</typeparam>
        /// <param name="collection">Source sequence of elements to project.</param>
        /// <param name="resultSelect">Expression that produces a Tout from a formatted text string and a value string.</param>
        /// <param name="valueExpression">Expression that selects the value object from a source element; its string representation is used as the
        /// value.</param>
        /// <param name="formatText">Composite format string used with string.Format to produce the display text.</param>
        /// <param name="textExpressions">Expressions that select objects from a source element to be formatted into formatText; their order
        /// corresponds to format placeholders.</param>
        /// <returns>An <![CDATA[IEnumerable<Tout>]]> containing one projected element per source element, created by invoking resultSelect
        /// with the formatted text and value string.</returns>
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

        /// <summary>
        /// Converts a dictionary of string key/value pairs to a URL query string.
        /// </summary>
        /// <param name="dictionary">Dictionary of query parameters; keys are converted to lowercase and values are used as-is.</param>
        /// <param name="addDelimiter">True to prepend a leading '?' when the dictionary contains entries; otherwise false.</param>
        /// <returns>A query string of key=value pairs joined by '<![CDATA[&]]>'. If addDelimiter is true and the dictionary contains entries
        /// the string is prefixed with '?'. Returns an empty string when the dictionary is empty.</returns>
        public static string ToQueryString(this IDictionary<string, string> dictionary, bool addDelimiter = true)
        {
            var result = string.Join("&", dictionary.Select(kv => $"{kv.Key.ToLower()}={kv.Value}"));

            if (addDelimiter && dictionary.Count > 0)
            {
                result = $"?{result}";
            }

            return result;
        }

        /// <summary>
        /// Performs the specified action on each element of the sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="collection">The sequence whose elements the action is applied to.</param>
        /// <param name="action">The action to apply to each element.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
    }
}
