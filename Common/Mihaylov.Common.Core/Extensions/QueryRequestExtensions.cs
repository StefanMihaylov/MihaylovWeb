using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Mihaylov.Common
{
    /// <summary>
    /// Builds query strings and links from request models that implement <see cref="IQueryRequest"/>.
    /// The parameters are the simple-typed public properties of the request, see <see cref="GetQueryParameters{T}(T)"/>.
    /// </summary>
    public static class QueryRequestExtensions
    {
        /// <summary>
        /// Builds a link from <paramref name="basePath"/> and all the parameters of the request.
        /// </summary>
        /// <typeparam name="T">The request type; its properties become the parameters.</typeparam>
        /// <param name="request">The request whose properties are appended to <paramref name="basePath"/>.</param>
        /// <param name="basePath">The path to extend, with or without a query string, for example <c>/Site</c> or <c>/Site?tab=1</c>.</param>
        /// <returns>The link, see <see cref="FullPath{T}(T, string, string, int?)"/> for the separator rules.</returns>
        public static string FullPath<T>(this T request, string basePath) where T : IQueryRequest
        {
            return request.FullPath(basePath, null, null);
        }

        /// <summary>
        /// Builds a link from <paramref name="basePath"/> and the parameters of the request, with the page parameter replaced or removed.
        /// The request itself is not changed.
        /// </summary>
        /// <typeparam name="T">The request type; its properties become the parameters.</typeparam>
        /// <param name="request">The request whose properties are appended to <paramref name="basePath"/>.</param>
        /// <param name="basePath">
        /// The path to extend. The separator is <c>?</c> when it has no query string, <c>&amp;</c> when it already has one,
        /// and none when it ends with <c>?</c> or <c>&amp;</c>.
        /// </param>
        /// <param name="pageName">
        /// The name of the page parameter. It must match the property name including its case (for example
        /// <c>nameof(IGridRequest.Page)</c>), otherwise the original parameter stays and the link has the page twice.
        /// When it is null or white space, the page parameter is left as the request has it.
        /// </param>
        /// <param name="page">The page number to put in the link, or null to leave the page parameter out.</param>
        /// <returns>
        /// <paramref name="basePath"/> followed by the parameters, or <paramref name="basePath"/> unchanged when there are none.
        /// Names are lower case and values are URL-encoded, see <see cref="ToQueryString(IDictionary{string, string}, bool)"/>.
        /// </returns>
        public static string FullPath<T>(this T request, string basePath, string pageName, int? page) where T : IQueryRequest
        {
            var parameters = request.GetQueryParameters();

            if (!string.IsNullOrWhiteSpace(pageName))
            {
                if (page.HasValue)
                {
                    parameters[pageName] = page.Value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    parameters.Remove(pageName);
                }
            }

            var query = parameters.ToQueryString(false);
            if (query.Length == 0)
            {
                return basePath;
            }

            string separator = GetSeparator(basePath);
            var result = $"{basePath}{separator}{query}";

            return result;
        }

        /// <summary>
        /// Converts the parameters of the request to a query string.
        /// </summary>
        /// <typeparam name="T">The request type; its properties become the parameters.</typeparam>
        /// <param name="request">The request to convert.</param>
        /// <param name="addDelimiter">True to start the result with <c>?</c> when there is at least one parameter.</param>
        /// <returns>For example <c>?name=ab&amp;page=2</c>, or an empty string when the request has no parameters.</returns>
        public static string ToQueryString<T>(this T request, bool addDelimiter = true) where T : IQueryRequest
        {
            return request.GetQueryParameters().ToQueryString(addDelimiter);
        }

        /// <summary>
        /// Reads the parameters of the request through reflection.
        /// </summary>
        /// <remarks>
        /// The properties come from the compile-time type <typeparamref name="T"/> (for an interface, including the interfaces it inherits),
        /// not from the runtime type, so a variable typed as the interface gives only the interface's properties.
        /// Only public readable properties of simple types are used: numbers, <see cref="bool"/>, enums, <see cref="string"/>,
        /// <see cref="decimal"/>, <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="Guid"/>, <see cref="TimeSpan"/>
        /// and their nullable forms. Objects and collections are skipped. The property list is cached for each type.
        /// </remarks>
        /// <typeparam name="T">The request type.</typeparam>
        /// <param name="request">The request to read.</param>
        /// <returns>
        /// A new, changeable dictionary from property name (case as declared) to text. Null values and empty strings are left out.
        /// Values use <see cref="CultureInfo.InvariantCulture"/>, so a <see cref="DateTime"/> is not written as ISO 8601.
        /// </returns>
        public static IDictionary<string, string> GetQueryParameters<T>(this T request) where T : IQueryRequest
        {
            var parameters = new Dictionary<string, string>();

            foreach (var property in QueryProperties<T>.All)
            {
                var text = Convert.ToString(property.GetValue(request), CultureInfo.InvariantCulture);

                if (!string.IsNullOrEmpty(text))
                {
                    parameters.Add(property.Name, text);
                }
            }

            return parameters;
        }

        /// <summary>
        /// Converts a dictionary of string key/value pairs to a URL query string.
        /// </summary>
        /// <param name="dictionary">Dictionary of query parameters; keys are converted to lowercase and values are URL-encoded with <see cref="Uri.EscapeDataString(string)"/>.</param>
        /// <param name="addDelimiter">True to prepend a leading '?' when the dictionary contains entries; otherwise false.</param>
        /// <returns>A query string of key=value pairs joined by '<![CDATA[&]]>'. If addDelimiter is true and the dictionary contains entries
        /// the string is prefixed with '?'. Returns an empty string when the dictionary is empty.</returns>
        public static string ToQueryString(this IDictionary<string, string> dictionary, bool addDelimiter = true)
        {
            var result = string.Join("&", dictionary.Select(kv => $"{kv.Key.ToLower()}={Uri.EscapeDataString(kv.Value)}"));

            if (addDelimiter && dictionary.Count > 0)
            {
                result = $"?{result}";
            }

            return result;
        }

        private static string GetSeparator(string basePath)
        {
            if (!basePath.Contains('?'))
            {
                return "?";
            }

            if (basePath.EndsWith('?') || basePath.EndsWith('&'))
            {
                return string.Empty;
            }

            return "&";
        }

        private static class QueryProperties<T>
        {
            public static readonly PropertyInfo[] All = GetTypes()
                .SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0 && IsSimple(p.PropertyType))
                .GroupBy(p => p.Name)
                .Select(g => g.First())
                .OrderBy(p => p.Name, StringComparer.Ordinal)
                .ToArray();

            private static IEnumerable<Type> GetTypes()
            {
                // GetProperties on an interface type does not return the inherited interfaces' properties.
                return typeof(T).IsInterface ? typeof(T).GetInterfaces().Prepend(typeof(T)) : new[] { typeof(T) };
            }
        }

        private static bool IsSimple(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal)
                || type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(Guid)
                || type == typeof(TimeSpan);
        }
    }
}
