using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Mihaylov.Users.Data.Database.Models;

namespace Mihaylov.Users.Data.Interfaces
{
    public interface ITokenHelper
    {
        string GetToken(User user, IEnumerable<string> roles);

        void SetJwtBearerOptions(JwtBearerOptions options);
    }
}