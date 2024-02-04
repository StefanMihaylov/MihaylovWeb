using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Mihaylov.WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        // private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger _logger;

        public LogoutModel(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string returnUrl = null)
        {
            Response.Cookies.Delete(LoginModel.COOKIE_NAME);

            _logger.LogInformation($"User '{User?.Identity?.Name}' logged out.");

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }

            // await _signInManager.SignOutAsync();
        }
    }
}
