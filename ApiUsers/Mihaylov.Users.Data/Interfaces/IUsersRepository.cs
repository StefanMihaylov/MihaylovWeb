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

        Task<GenericResponse> AddRoleAsync(AddRoleToUserRequest request);

        Task<GenericResponse> DeleteUserAsync(Guid id);


        Task<IEnumerable<RoleModel>> GetRolesAsync();

        Task<GenericResponse> AddRoleAsync(CreateRoleRequest request);

        Task<GenericResponse> ChangePasswordAsync(ChangePasswordRequest request);

        Task InitializeDatabaseAsync(string adminRole);
    }
}