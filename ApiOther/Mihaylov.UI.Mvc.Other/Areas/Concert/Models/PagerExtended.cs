using Mihaylov.Common;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

/// <summary>
/// The pager of a Concert grid: the paging numbers of the grid result and page links that keep the current filter and the active tab of the request.
/// </summary>
/// <param name="Pager">The paging information returned with the grid.</param>
/// <param name="request">The request that produced the grid; its parameters, including the active tab, are repeated in every page link.</param>
/// <param name="baseUrl">The path the page links point to, without a query string.</param>
/// <param name="ItemLabel">The plural name of the items, shown in the summary.</param>
public record PagerExtended(IPager Pager, ConcertQueryRequest request, string baseUrl, string ItemLabel)
    : PagerModel(Pager.Page, Pager.Count, Pager.PageSize,
                 page => request.FullPath(baseUrl, nameof(ConcertQueryRequest.Page), page), ItemLabel)
{
}