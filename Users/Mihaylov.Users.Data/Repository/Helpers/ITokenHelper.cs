using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Mihaylov.Users.Data.Database.Models;

namespace Mihaylov.Users.Data.Repository.Helpers
{
    public interface ITokenHelper
    {
        string GetToken(User user);

        void SetJwtBearerOptions(JwtBearerOptions options);
    }
}