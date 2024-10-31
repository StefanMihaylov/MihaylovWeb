// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Users.Client;
using Mihaylov.Common;
using Mihaylov.Common.Host.Authorization;

namespace Mihaylov.Web.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUsersApiClient _usersApiClient;
        private readonly ILogger _logger;

        public DeletePersonalDataModel(IUsersApiClient usersApiClient, ILoggerFactory loggerFactory)
        {
            _usersApiClient = usersApiClient;
            _logger = loggerFactory.CreateLogger(GetType().Name);
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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool RequirePassword { get; set; }

        public IActionResult OnGet()
        {
            RequirePassword = true; // await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            RequirePassword = true;
            if (RequirePassword)
            {
                var loginRequest = new LoginRequestModel()
                {
                    UserName = User.Identity.Name,
                    Password = Input.Password,
                    ClaimTypesEnum = ClaimType.Username,
                    LockoutOnFailure = false,
                };
                var response = await _usersApiClient.LoginLoginAsync(loginRequest).ConfigureAwait(false);
                if (!response.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            _usersApiClient.AddToken(Request.GetToken());
            var result = await _usersApiClient.UsersDeleteUserAsync(User.GetId()).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            LogoutModel.Logout(Response);

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", User.GetId());

            return Redirect("~/");
        }
    }
}
