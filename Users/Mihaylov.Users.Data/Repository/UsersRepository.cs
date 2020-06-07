using Microsoft.AspNetCore.Identity;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Data.Repository.Helpers;
using Mihaylov.Users.Data.Repository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mihaylov.Users.Data.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenHelper _tokenHelper;

        public UsersRepository(UserManager<User> userManager, ITokenHelper tokenHelper)
        {
            this._userManager = userManager;
            this._tokenHelper = tokenHelper;
        }

        public async Task<RegisterResponseModel> RegisterAsync(RegisterRequestModel request)
        {
            User user = new User(request.Username, request.Email);
            IdentityResult result = await this._userManager.CreateAsync(user, request.Password);

            return new RegisterResponseModel(result.Succeeded, result.Errors?.Select(e => e.Description));
        }

        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel request)
        {
            User user = await this._userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new LoginResponseModel(false, null);
            }

            bool isPasswordValid = await this._userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return new LoginResponseModel(false, null);
            }

            IList<string> roles = await this._userManager.GetRolesAsync(user);

            string encryptedToken = this._tokenHelper.GetToken(user);

            return new LoginResponseModel(true, encryptedToken);
        }
    }
}
