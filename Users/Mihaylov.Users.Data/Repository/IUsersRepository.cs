using Mihaylov.Users.Data.Repository.Models;
using System.Threading.Tasks;

namespace Mihaylov.Users.Data.Repository
{
    public interface IUsersRepository
    {
        Task<RegisterResponseModel> RegisterAsync(RegisterRequestModel request);

        Task<LoginResponseModel> LoginAsync(LoginRequestModel request);
    }
}