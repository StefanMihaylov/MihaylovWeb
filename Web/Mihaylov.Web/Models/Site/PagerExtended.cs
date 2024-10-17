using Mihaylov.Api.Site.Client;
using Mihaylov.Web.Models.Configs;

namespace Mihaylov.Web.Models.Site
{
    public class PagerExtended : Pager
    {
        public PagerExtended(Pager pager)
        {
            Page = pager.Page;
            PageSize = pager.PageSize;
            PageMax = pager.PageMax;
            Count = pager.Count;
        }

        public IGridRequest Request {  get; set; }

        public string Path { get; set; }

        public string FullPath(int? page)
        {
            Request.Page = page;
            return $"{Path}{Request.ToQueryString()}";
        }
    }
}
