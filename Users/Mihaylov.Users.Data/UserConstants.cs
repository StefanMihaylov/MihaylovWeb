using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Mihaylov.Users.Data
{
    public class UserConstants
    {
        public const string AdminRole = "Administrator";

        public const string AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
    }
}
