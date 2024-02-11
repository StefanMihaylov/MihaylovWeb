using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Users.Models.Requests;
using Mihaylov.Users.Models.Responses;

namespace Mihaylov.Users.Data.Interfaces
{
    public interface IUsersRepository
    {
        Task<GenericResponse> RegisterAsync(RegisterRequestModel request);

        Task<LoginResponseModel> LoginAsync(LoginRequestModel request);

        Task<IEnumerable<UserModel>> GetUsersAsync();

        Task<UserModel> GetUserAsync(string userId);

        Task<GenericResponse> UpdateUserAsync(UpdateUserModel update);

        Task<GenericResponse> AddRoleToUserAsync(AddRoleToUserRequest request);

        Task<GenericResponse> ChangePasswordAsync(ChangePasswordRequest request);

        Task<GenericResponse> DeleteUserAsync(Guid userId);


        Task<IEnumerable<RoleModel>> GetRolesAsync();

        Task<RoleModel> GetRoleByIdAsync(Guid roleId);

        Task<GenericResponse> AddRoleAsync(CreateRoleRequest request);

        Task<GenericResponse> UpdateRoleAsync(UpdateRoleRequest request, string adminRole);

        Task<GenericResponse> DeleteRoleAsync(Guid roleId);        


        Task InitializeDatabaseAsync(string adminRole);
    }
}