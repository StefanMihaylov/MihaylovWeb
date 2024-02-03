using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Mihaylov.Common.Host.Authorization
{
    public class UserConstants
    {
        public const string AdminRole = "Administrator";

        public const string AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
    }
}
