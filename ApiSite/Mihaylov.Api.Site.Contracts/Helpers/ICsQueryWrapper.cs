using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Helpers
{
    public interface ICsQueryWrapper
    {
        Person GetInfo(string url, string username);
    }
}
