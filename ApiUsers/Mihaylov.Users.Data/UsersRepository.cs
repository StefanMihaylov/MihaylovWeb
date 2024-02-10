using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly SignInManager<User> _signManager;
        private readonly ITokenHelper _tokenHelper;
        private readonly ILogger _logger;


        public UsersRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<User> signManager, ITokenHelper tokenHelper, ILoggerFactory factory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signManager = signManager;
            _tokenHelper = tokenHelper;
            _logger = factory.CreateLogger(GetType().Name);
        }

        public async Task<GenericResponse> RegisterAsync(RegisterRequestModel request)
        {
            User user = new User(request.Username, request.Email);
            user.Profile = new UserProfile(request.FirstName, request.LastName);

            IdentityResult result = await _userManager.CreateAsync(user, request.Password).ConfigureAwait(false);

            return GetGenericResponse(result);
        }

        public async Task<LoginResponseModel> LoginAsync(LoginRequestModel request)
        {
            User user = await _userManager.FindByNameAsync(request.UserName).ConfigureAwait(false);
            if (user == null)
            {
                return new LoginResponseModel(false, false, null, null);
            }

            var passwordResult = await _signManager.CheckPasswordSignInAsync(user, request.Password, request.LockoutOnFailure).ConfigureAwait(false);
            if (!passwordResult.Succeeded)
            {
                return new LoginResponseModel(passwordResult.Succeeded, passwordResult.IsLockedOut, user.UserName, null);
            }

            IList<string> roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            string encryptedToken = _tokenHelper.GetToken(user, roles, request.ClaimTypes);

            return new LoginResponseModel(true, false, user.UserName, encryptedToken);
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            var users = await _userManager.Users
                                    .Select(FromDbUser())
                                    .ToListAsync()
                                    .ConfigureAwait(false);
            return users;
        }

        public async Task<UserModel> GetUserAsync(string userId)
        {
            var user = await _userManager.Users
                                    .Where(u => u.Id == userId)
                                    .Select(FromDbUser())
                                    .FirstOrDefaultAsync()
                                    .ConfigureAwait(false);

            return user;
        }

        public async Task<GenericResponse> UpdateUserAsync(UpdateUserModel update)
        {
            var dbUser = await _userManager.FindByIdAsync(update.Id).ConfigureAwait(false);
            if (dbUser == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(update.Email))
            {
                dbUser.Email = update.Email;
            }

            if (!string.IsNullOrEmpty(update.FirstName))
            {
                dbUser.Profile.FirstName = update.FirstName;
            }

            if (!string.IsNullOrEmpty(update.LastName))
            {
                dbUser.Profile.LastName = update.LastName;
            }

            IdentityResult result = await _userManager.UpdateAsync(dbUser).ConfigureAwait(false);

            return GetGenericResponse(result);
        }


        public async Task<GenericResponse> AddRoleToUserAsync(AddRoleToUserRequest request)
        {
            IdentityResult result = null;

            var user = await _userManager.FindByIdAsync(request.UserId.ToString()).ConfigureAwait(false);
            if (user != null)
            {
                var role = await _roleManager.FindByIdAsync(request.RoleId.ToString()).ConfigureAwait(false);

                if (role != null)
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
                }
            }

            return GetGenericResponse(result);
        }

        public async Task<GenericResponse> UpdateRoleAsync(UpdateRoleRequest request, string adminRole)
        {
            IdentityResult result = null;

            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString()).ConfigureAwait(false);
            if (role != null)
            {
                if (role.Name != adminRole && !string.IsNullOrWhiteSpace(request.RoleName))
                {
                    role.Name = request.RoleName;
                }

                result = await _roleManager.UpdateAsync(role).ConfigureAwait(false);
            }

            return GetGenericResponse(result);
        }

        public async Task<GenericResponse> DeleteRoleAsync(Guid roleId)
        {
            IdentityResult result = null;

            var dbRole = await _roleManager.FindByIdAsync(roleId.ToString()).ConfigureAwait(false);
            if (dbRole != null)
            {
                result = await _roleManager.DeleteAsync(dbRole).ConfigureAwait(false);
            }

            return GetGenericResponse(result);
        }

        public async Task<GenericResponse> DeleteUserAsync(Guid id)
        {
            IdentityResult result = null;

            var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
            if (user != null)
            {
                result = await _userManager.DeleteAsync(user).ConfigureAwait(false);
            }

            return GetGenericResponse(result);
        }


        public async Task<IEnumerable<RoleModel>> GetRolesAsync()
        {
            try
            {
                var roles = await _roleManager.Roles
                             .Select(FromDbRole())
                             .ToListAsync()
                             .ConfigureAwait(false);
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetRoles failed.");
                throw;
            }
        }

        public async Task<RoleModel> GetRoleByIdAsync(Guid roleId)
        {
            try
            {
                var role = await _roleManager.Roles
                            .Where(r => r.Id == roleId.ToString())
                            .Select(FromDbRole())
                            .FirstOrDefaultAsync()
                            .ConfigureAwait(false);

                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetRole failed.");
                throw;
            }
        }

        public async Task<GenericResponse> AddRoleAsync(CreateRoleRequest request)
        {
            try
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(request.RoleName)).ConfigureAwait(false);

                return GetGenericResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddRole failed.");
                return new GenericResponse(ex);
            }
        }

        public async Task<GenericResponse> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                User user = await _userManager.FindByNameAsync(request.UserName).ConfigureAwait(false);
                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword).ConfigureAwait(false);

                return GetGenericResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangePassword failed.");
                return new GenericResponse(ex);
            }
        }


        public async Task InitializeDatabaseAsync(string adminRole)
        {
            var roles = await GetRolesAsync().ConfigureAwait(false);
            if (roles.Any() == false)
            {
                var newRole = new CreateRoleRequest()
                {
                    RoleName = adminRole,
                };

                var result = await AddRoleAsync(newRole).ConfigureAwait(false);
            }
            else
            {
                var users = await GetUsersAsync().ConfigureAwait(false);
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

                        var result = await AddRoleToUserAsync(addRoleRequest).ConfigureAwait(false);
                    }
                }
            }
        }

        private GenericResponse GetGenericResponse(IdentityResult result)
        {
            return new GenericResponse(result?.Succeeded ?? false, result?.Errors.Select(e => e.Description));
        }

        private Expression<Func<User, UserModel>> FromDbUser()
        {
            return u => new UserModel()
            {
                Id = new Guid(u.Id),
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                Roles = _userManager.GetRolesAsync(u).Result
            };
        }

        private Expression<Func<IdentityRole, RoleModel>> FromDbRole()
        {
            return r => new RoleModel()
            {
                Id = new Guid(r.Id),
                Name = r.Name
            };
        }
    }
}
