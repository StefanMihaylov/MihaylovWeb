using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.CsQuery
{
    public interface ICsQueryWrapper
    {
        Person GetInfo(string url, string username);
    }
}