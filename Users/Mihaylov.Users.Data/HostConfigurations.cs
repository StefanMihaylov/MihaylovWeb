using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mihaylov.Users.Data.Database;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Data.Repository;
using Mihaylov.Users.Data.Repository.Helpers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mihaylov.Users.Data
{
    public static class HostConfigurations
    {
        public static IServiceCollection AddUserDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();

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
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<MihaylovUsersDbContext>();

            ServiceProvider provider = GetServiceProvider(services);

            var dbContect = provider.GetService<MihaylovUsersDbContext>();
            dbContect.Database.Migrate();

            IUsersRepository usersRepository = provider.GetService<IUsersRepository>();

            usersRepository.InitializeDatabase();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var secret = Environment.GetEnvironmentVariable("JWT_AUTHENTICATION_SECRET");

            services.Configure<AppUserSettings>((x) => x.Secret = secret);
            services.AddScoped<ITokenHelper, TokenHelper>();

            ServiceProvider provider = GetServiceProvider(services);
            ITokenHelper tokenHelper = provider.GetService<ITokenHelper>();

            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = UserConstants.AuthenticationScheme;
                        options.DefaultScheme = UserConstants.AuthenticationScheme;
                        options.DefaultChallengeScheme = UserConstants.AuthenticationScheme;
                    })
                    .AddJwtBearer((options) => tokenHelper.SetJwtBearerOptions(options));

            return services;
        }

        public static void AddSwaggerAuthentication(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(UserConstants.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = $@"JWT Authorization header using the {UserConstants.AuthenticationScheme} scheme.
                      Enter '{UserConstants.AuthenticationScheme}' [space] and then your token in the text input below. 
                      Example: '{UserConstants.AuthenticationScheme} 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = UserConstants.AuthenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = UserConstants.AuthenticationScheme
                        },
                        Scheme = "oauth2",
                        Name = UserConstants.AuthenticationScheme,
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
