using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Users.Client;
using Mihaylov.Common.Host.Abstract.Authorization;

namespace Mihaylov.Web.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        public const string COOKIE_NAME = "X-Access-Token";

        // private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IUsersApiClient _usersApiClient;

        public LoginModel(ILoggerFactory loggerFactory, IUsersApiClient usersApiClient)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _usersApiClient = usersApiClient;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IEnumerable<SchemeModel> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            public string UserName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            // [Display(Name = "Remember me?")]
            // public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = await _usersApiClient.ExternalGetExternalSchemesAsync().ConfigureAwait(false);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = await _usersApiClient.ExternalGetExternalSchemesAsync().ConfigureAwait(false);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var request = new LoginRequestModel()
                {
                    UserName = this.Input.UserName,
                    Password = this.Input.Password,
                    ClaimTypesEnum = ClaimType.Username,
                };

                var response = await _usersApiClient.LoginLoginAsync(request).ConfigureAwait(false);
                if (response?.Succeeded != true)
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return Page();
                }

                var token = response.Token;

                var cookieOptions = new CookieOptions() { HttpOnly = false, SameSite = SameSiteMode.Strict };
                Response.Cookies.Append(COOKIE_NAME, response.Token, cookieOptions);

                _logger.LogInformation($"User '{this.Input.UserName}' logged in.");
                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"User logged in failed. ErrorMessage: {ex.Message}");

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
			
			/*
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
				
            }

            // If we got this far, something failed, redisplay form
            return Page();  */
        }
    }
}
