using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Data.Repository.Helpers;
using Mihaylov.Users.Data.Repository.Models;

namespace Mihaylov.Users.Data.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenHelper _tokenHelper;


        public UsersRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            ITokenHelper tokenHelper)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._tokenHelper = tokenHelper;
        }

        public async Task<GenericResponse> RegisterAsync(RegisterRequestModel request)
        {
            User user = new User(request.Username, request.Email);
            IdentityResult result = await this._userManager.CreateAsync(user, request.Password);

            return new GenericResponse(result);
        }

        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel request)
        {
            User user = await this._userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new LoginResponseModel(false, null, null);
            }

            bool isPasswordValid = await this._userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return new LoginResponseModel(false, user.UserName, null);
            }

            IList<string> roles = await this._userManager.GetRolesAsync(user);
            string encryptedToken = this._tokenHelper.GetToken(user, roles);

            return new LoginResponseModel(true, user.UserName, encryptedToken);
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            var users = await this._userManager.Users
                                    .Select(u => new UserModel()
                                    {
                                        Id = new Guid(u.Id),
                                        UserName = u.UserName,
                                        Email = u.Email,
                                        Roles = this._userManager.GetRolesAsync(u).Result
                                    })
                                    .ToListAsync();

            return users;
        }

        public async Task<GenericResponse> AddRoleAsync(AddRoleToUserRequest request)
        {
            IdentityResult result = null;

            var user = await this._userManager.FindByIdAsync(request.UserId.ToString());
            if (user != null)
            {
                var role = await this._roleManager.FindByIdAsync(request.RoleId.ToString());

                if (role != null)
                {
                    result = await this._userManager.AddToRoleAsync(user, role.Name);
                }
            }

            return new GenericResponse(result);
        }

        public async Task<GenericResponse> DeleteUserAsync(Guid id)
        {
            IdentityResult result = null;

            var user = await this._userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                result = await this._userManager.DeleteAsync(user);
            }

            return new GenericResponse(result);
        }

        public async Task<IEnumerable<RoleModel>> GetRolesAsync()
        {
            var roles = await this._roleManager.Roles.Select(r => (RoleModel)r)
                                                     .ToListAsync();
            return roles;
        }

        public async Task<GenericResponse> AddRoleAsync(CreateRoleRequest request)
        {
            var result = await this._roleManager.CreateAsync(new IdentityRole(request.RoleName));
            return new GenericResponse(result);
        }

        public async Task InitializeDatabase()
        {
            var roles = await this.GetRolesAsync();
            if (roles.Any() == false)
            {
                var newRole = new CreateRoleRequest()
                {
                    RoleName = UserConstants.AdminRole,
                };

                var result = await this.AddRoleAsync(newRole);
            }
            else
            {
                var users = await this.GetUsersAsync();
                if (roles.Count() == 1 && users.Count() == 1)
                {
                    var role = roles.First();
                    var user = users.First();

                    if (user.Roles.Any() == false)
                    {
                        var addRoleRequest = new AddRoleToUserRequest()
                        {
                            UserId = user.Id,
                            RoleId = role.Id,
                        };

                        var result = await this.AddRoleAsync(addRoleRequest);
                    }
                }
            }
        }
    }
}
