using Microsoft.AspNetCore.Http;
using Mihaylov.Common;

namespace Mihaylov.UI.Mvc.Users;

public class CookieTokenProvider(IHttpContextAccessor accessor) : IAccessTokenProvider
{
    public string GetToken()
    {
        var token = accessor.HttpContext?.Request.Cookies[IAccessTokenProvider.COOKIE_NAME];
        return token;
    }
}
