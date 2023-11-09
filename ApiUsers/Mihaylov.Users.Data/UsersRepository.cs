using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Data.Interfaces;
using Mihaylov.Users.Models.Requests;
using Mihaylov.Users.Models.Responses;

namespace Mihaylov.Users.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenHelper _tokenHelper;
        private readonly ILogger _logger;


        public UsersRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            ITokenHelper tokenHelper, ILoggerFactory factory)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._tokenHelper = tokenHelper;
            this._logger = factory.CreateLogger(this.GetType().Name);
        }

        public async Task<GenericResponse> RegisterAsync(RegisterRequestModel request)
        {
            User user = new User(request.Username, request.Email);
            user.Profile = new UserProfile(request.FirstName, request.LastName);

            IdentityResult result = await this._userManager.CreateAsync(user, request.Password)
                                                           .ConfigureAwait(false);

            return GetGenericResponse(result);
        }

        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel request)
        {
            User user = await this._userManager.FindByNameAsync(request.UserName)
                                               .ConfigureAwait(false);
            if (user == null)
            {
                return new LoginResponseModel(false, null, null);
            }

            bool isPasswordValid = await this._userManager.CheckPasswordAsync(user, request.Password)
                                                          .ConfigureAwait(false);
            if (!isPasswordValid)
            {
                return new LoginResponseModel(false, user.UserName, null);
            }

            IList<string> roles = await this._userManager.GetRolesAsync(user)
                                                         .ConfigureAwait(false);

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
                                        FirstName = u.Profile.FirstName,
                                        LastName = u.Profile.LastName,
                                        Roles = this._userManager.GetRolesAsync(u).Result
                                    })
                                    .ToListAsync()
                                    .ConfigureAwait(false);

            return users;
        }

        public async Task<GenericResponse> AddRoleAsync(AddRoleToUserRequest request)
        {
            IdentityResult result = null;

            var user = await this._userManager.FindByIdAsync(request.UserId.ToString()).ConfigureAwait(false);
            if (user != null)
            {
                var role = await this._roleManager.FindByIdAsync(request.RoleId.ToString()).ConfigureAwait(false);

                if (role != null)
                {
                    result = await this._userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
                }
            }

            return GetGenericResponse(result);
        }

        public async Task<GenericResponse> DeleteUserAsync(Guid id)
        {
            IdentityResult result = null;

            var user = await this._userManager.FindByIdAsync(id.ToString())
                                              .ConfigureAwait(false);
            if (user != null)
            {
                result = await this._userManager.DeleteAsync(user)
                                                .ConfigureAwait(false);
            }

            return GetGenericResponse(result);
        }


        public async Task<IEnumerable<RoleModel>> GetRolesAsync()
        {
            try
            {
                var roles = await this._roleManager.Roles
                             .Select(r => new RoleModel()
                             {
                                 Id = new Guid(r.Id),
                                 Name = r.Name
                             })
                             .ToListAsync()
                             .ConfigureAwait(false);
                return roles;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "GetRoles failed.");
                throw;
            }

        }

        public async Task<GenericResponse> AddRoleAsync(CreateRoleRequest request)
        {
            try
            {
                var result = await this._roleManager.CreateAsync(new IdentityRole(request.RoleName))
                                                    .ConfigureAwait(false);
                
                return GetGenericResponse(result);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "AddRole failed.");
                return new GenericResponse(ex);
            }
        }

        public async Task<GenericResponse> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                User user = await this._userManager.FindByNameAsync(request.UserName).ConfigureAwait(false);
                var result = await this._userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword).ConfigureAwait(false);

                return GetGenericResponse(result);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "ChangePassword failed.");
                return new GenericResponse(ex);
            }
        }


        public async Task InitializeDatabaseAsync()
        {
            var roles = await this.GetRolesAsync().ConfigureAwait(false);
            if (roles.Any() == false)
            {
                var newRole = new CreateRoleRequest()
                {
                    RoleName = UserConstants.AdminRole,
                };

                var result = await this.AddRoleAsync(newRole).ConfigureAwait(false);
            }
            else
            {
                var users = await this.GetUsersAsync().ConfigureAwait(false);
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

                        var result = await this.AddRoleAsync(addRoleRequest).ConfigureAwait(false);
                    }
                }
            }
        }

        private GenericResponse GetGenericResponse(IdentityResult result)
        {
            return new GenericResponse(result?.Succeeded ?? false, result?.Errors.Select(e => e.Description));
        }
    }
}
