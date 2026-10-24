using Microsoft.AspNetCore.Http;
using Mihaylov.Common;

namespace Mihaylov.Web.Common;

public class CookieTokenProvider(IHttpContextAccessor accessor) : IAccessTokenProvider
{
    public string GetToken()
    {
        var token = accessor.HttpContext?.Request.Cookies[IAccessTokenProvider.COOKIE_NAME];
        return token;
    }
}
