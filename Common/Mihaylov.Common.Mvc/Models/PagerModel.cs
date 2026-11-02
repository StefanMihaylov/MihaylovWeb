using System;
using System.Collections.Generic;

namespace Mihaylov.Common;

/// <summary>
/// Everything the shared <c>_Pagination</c> view needs to draw a pager: the paging numbers, the links and the label of the items.
/// Create it from an <see cref="IPager"/> and a link builder, for example in a derived record that knows the current request.
/// </summary>
/// <param name="Page">The current page, starting at 1.</param>
/// <param name="Count">The total number of items across all pages.</param>
/// <param name="PageSize">
/// The number of items per page. When it is <c>null</c>, the view draws nothing,
/// and <see cref="LastPage"/> is 1.
/// </param>
/// <param name="PageUrl">Builds the link to the given page number. The view calls it only for pages it shows as links.</param>
/// <param name="ItemLabel">The plural name of the items, shown after the count in the summary (for example "persons").</param>
public record PagerModel(int Page, int Count, int? PageSize, Func<int, string> PageUrl, string ItemLabel = "items")
{
    /// <summary>
    /// Gets the number of the last page, at least 1.
    /// It is 1 when <see cref="PageSize"/> is <c>null</c> or not positive, and when there are no items.
    /// </summary>
    public int LastPage => PageSize > 0 ? Math.Max(1, (Count + PageSize.Value - 1) / PageSize.Value) : 1;

    /// <summary>
    /// Gets a value indicating whether there is a page before <see cref="Page"/>.
    /// </summary>
    public bool HasPrevious => Page > 1;

    /// <summary>
    /// Gets a value indicating whether there is a page after <see cref="Page"/>.
    /// </summary>
    public bool HasNext => Page < LastPage;

    /// <summary>
    /// Gets the page numbers the view shows: the first page, the last page and the pages within two of <see cref="Page"/>.
    /// </summary>
    /// <returns>
    /// The numbers in ascending order. A <c>null</c> entry stands for a gap ("…") where one or more pages are left out.
    /// </returns>
    public IEnumerable<int?> Numbers()
    {
        const int WINDOW = 2;

        var result = new List<int?>();

        var previous = 0;

        for (var number = 1; number <= LastPage; number++)
        {
            if (number != 1 && number != LastPage && Math.Abs(number - Page) > WINDOW)
            {
                continue;
            }

            if (number - previous > 1)
            {
                result.Add(null);
            }

            result.Add(number);

            previous = number;
        }

        return result;
    }
}
