using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Mihaylov.Web.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        // private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger _logger;

        public LogoutModel(ILoggerFactory loggerFactory)
        {
			_logger = loggerFactory.CreateLogger(this.GetType().Name);
        }

        public IActionResult OnPost(string returnUrl = null)
        {
			Response.Cookies.Delete(LoginModel.COOKIE_NAME);

          //  await _signInManager.SignOutAsync();
           _logger.LogInformation($"User '{User?.Identity?.Name}' logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
