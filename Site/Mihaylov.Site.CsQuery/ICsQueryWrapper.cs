using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.CsQuery
{
    public interface ICsQueryWrapper
    {
        Person GetInfo(string url, string username);
    }
}