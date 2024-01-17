using System;
using System.Threading.Tasks;

namespace Mihaylov.Api.Users.Client
{
    public interface IUsersApiClient
    {
        Task<GenericResponse> AddRoleAsync(CreateRoleRequest body);
        Task<GenericResponse> AddRoleToUserAsync(AddRoleToUserRequest body);
        Task<GenericResponse> ChangePasswordAsync(ChangePasswordRequest body);
        Task<GenericResponse> DeleteUserAsync(Guid id);
        Task<ModuleInfo> GetInfoAsync();
        Task<RoleModel> GetRolesAsync();
        Task<UserModel> GetUsersAsync();
        Task<LoginResponseModel> LoginAsync(LoginRequestModel body);
        Task<GenericResponse> RegisterAsync(RegisterRequestModel body);
    }
}