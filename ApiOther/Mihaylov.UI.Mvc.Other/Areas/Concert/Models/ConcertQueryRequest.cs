using Mihaylov.Common;

namespace Mihaylov.UI.Mvc.Other.Areas.Concert.Models;

public class ConcertQueryRequest : IQueryRequest
{
    public int? Page { get; set; }

    public string ActiveTab { get; set; }

    public ConcertQueryRequest()
    {
    }

    public ConcertQueryRequest(int? page, string activeTab)
    {
        Page = page;
        ActiveTab = activeTab;
    }
}
