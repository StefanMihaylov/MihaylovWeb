using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Users.Data.Database;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Data.Repository.Helpers;

namespace Mihaylov.Users.Data
{
    public static class HostConfigurations
    {
        public static IServiceCollection AddUserDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MihaylovUsersDbContext>(options =>
                        options.UseSqlServer(connectionString));

            services.AddIdentity<User, IdentityRole>(options =>
                        {
                            options.User.RequireUniqueEmail = true;
                            
                            options.Password.RequireNonAlphanumeric = false;
                            options.Password.RequireUppercase = false;
                            options.Password.RequireDigit = false;
                            options.Password.RequiredLength = 6;
                        })
                    .AddEntityFrameworkStores<MihaylovUsersDbContext>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var secret = Environment.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET");

            services.Configure<AppUserSettings>((x)=> x.Secret = secret);
            services.AddScoped<ITokenHelper, TokenHelper>();

            ITokenHelper tokenHelper = GetTokenHelper(services);

            services.AddAuthentication(TokenHelper.AUTHENTICATION_SCHEME)
                    .AddJwtBearer((options) => tokenHelper.SetJwtBearerOptions(options));

            return services;
        }

        private static ITokenHelper GetTokenHelper(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var tokenHelper = sp.GetService<ITokenHelper>();
            return tokenHelper;
        }
    }
}
