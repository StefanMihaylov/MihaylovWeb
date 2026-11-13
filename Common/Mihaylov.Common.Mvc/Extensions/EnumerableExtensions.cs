using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mihaylov.Common;

/// <summary>
/// Provides Enumerable extension methods
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Projects each element of the source sequence into a <see cref="SelectListItem"/> using the specified value and text selectors.
    /// </summary>
    /// <typeparam name="Tin">The type of elements in the source sequence.</typeparam>
    /// <param name="collection">The source sequence of elements to project.</param>
    /// <param name="valueFunc">A function that gets the value int from a source element.</param>
    /// <param name="textFunc">A function that gets the text string from a source element.</param>
    /// <returns>A list of <see cref="SelectListItem"/> with one item for each source element, in the same order.</returns>
    public static IEnumerable<SelectListItem> GetListItems<Tin>(this IEnumerable<Tin> collection, Func<Tin, int> valueFunc, Func<Tin, string> textFunc)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));

        return collection.GetListItems(a => valueFunc(a).ToString(), textFunc);
    }

    /// <summary>
    /// Projects each element of the source sequence into a <see cref="SelectListItem"/> using the specified value and text selectors.
    /// </summary>
    /// <typeparam name="Tin">The type of elements in the source sequence.</typeparam>
    /// <param name="collection">The source sequence of elements to project.</param>
    /// <param name="valueFunc">A function that gets the value string from a source element.</param>
    /// <param name="textFunc">A function that gets the text string from a source element.</param>
    /// <returns>A list of <see cref="SelectListItem"/> with one item for each source element, in the same order.</returns>
    public static IEnumerable<SelectListItem> GetListItems<Tin>(this IEnumerable<Tin> collection, Func<Tin, string> valueFunc, Func<Tin, string> textFunc)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(valueFunc, nameof(valueFunc));
        ArgumentNullException.ThrowIfNull(textFunc, nameof(textFunc));

        var list = collection.Select(item => new SelectListItem { Value = valueFunc(item), Text = textFunc(item) }).ToList();
        return list;
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
        ArgumentNullException.ThrowIfNull(collection);

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
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(resultSelect);
        ArgumentNullException.ThrowIfNull(valueExpression);
        ArgumentNullException.ThrowIfNull(formatText);
        ArgumentNullException.ThrowIfNull(textExpressions);

        var list = new List<Tout>();

        Func<string, string, Tout> resultMethod = resultSelect.Compile();
        Func<Tin, object> valueMethod = valueExpression.Compile();

        foreach (Tin item in collection)
        {
            var textParams = new List<object>();
            foreach (var textExpression in textExpressions)
            {
                Func<Tin, object> textMethod = textExpression.Compile();
                textParams.Add(textMethod(item));
            }

            var text = string.Format(formatText, textParams.ToArray());
            var value = valueMethod(item).ToString();

            Tout result = resultMethod(text, value);

            list.Add(result);
        }

        return list;
    }
}
