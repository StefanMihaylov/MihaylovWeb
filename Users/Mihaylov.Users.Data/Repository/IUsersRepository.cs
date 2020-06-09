using Mihaylov.Users.Data.Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mihaylov.Users.Data.Repository
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

        Task InitializeDatabase();
    }
}