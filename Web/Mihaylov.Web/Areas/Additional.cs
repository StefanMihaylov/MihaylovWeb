using Microsoft.AspNetCore.Http;
using Mihaylov.Web.Areas.Identity.Pages.Account;

namespace Mihaylov.Web.Areas
{
    public static class Additional
    {
        public static string GetToken(this HttpRequest request)
        {
            return request.Cookies[LoginModel.COOKIE_NAME];
        } 
    }
}
