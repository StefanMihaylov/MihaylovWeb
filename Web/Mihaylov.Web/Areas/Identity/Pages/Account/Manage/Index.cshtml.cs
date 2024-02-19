using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mihaylov.Api.Users.Client;
using Mihaylov.Common.Host.Authorization;

namespace Mihaylov.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
		private readonly IUsersApiClient _usersApiClient;

		public IndexModel(IUsersApiClient usersApiClient)
        {
            _usersApiClient = usersApiClient;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }
        }
		
		public async Task<IActionResult> OnGetAsync()
        {
            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadAsync();
                return Page();
            }

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Unexpected error when trying to set phone number.";
            //        return RedirectToPage();
            //    }
            //}
            // await _signInManager.RefreshSignInAsync(user);

            var request = new UpdateUserModel()
            {
                UserId = User.GetId(),
                Email = this.Input.Email,
                FirstName = this.Input.FirstName,
                LastName = this.Input.LastName,
            };

            _usersApiClient.AddToken(Request.GetToken());
            var response = await _usersApiClient.UsersUpdateUserAsync(request).ConfigureAwait(false);
            if (!response.Succeeded)
            {
                StatusMessage = $"Error in user updating: {string.Join(";", response.Errors)}";
            }
            else
            {
                StatusMessage = "Your profile has been updated";
            }

            return RedirectToPage();
        }

        private async Task LoadAsync()
        {
            _usersApiClient.AddToken(Request.GetToken());
            var userData = await _usersApiClient.UsersGetUserByIdAsync(User.GetId()).ConfigureAwait(false);

            Username = userData.UserName;
            Input = new InputModel
            {
                Email = userData.Email,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
            };
        }
    }
}
