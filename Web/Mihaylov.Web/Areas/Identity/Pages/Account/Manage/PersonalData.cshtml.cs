// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Users.Client;

namespace Mihaylov.Web.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        // private readonly UserManager<IdentityUser> _userManager;
        private readonly IUsersApiClient _usersApiClient;
        private readonly ILogger _logger;

        public PersonalDataModel(IUsersApiClient usersApiClient, ILoggerFactory loggerFactory)
        {
            _usersApiClient = usersApiClient;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
