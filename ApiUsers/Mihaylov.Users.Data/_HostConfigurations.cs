using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common.Abstract.Databases;
using Mihaylov.Users.Data.Database;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Data.Helpers;
using Mihaylov.Users.Data.Interfaces;

namespace Mihaylov.Users.Data
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddUserDatabase(this IServiceCollection services, Action<ConnectionStringSettings> connectionString, Action<PasswordOptions> passwordOptions)
        {
            var connectionStringSettings = new ConnectionStringSettings();
            connectionString(connectionStringSettings);

            var password = new PasswordOptions();

            if (passwordOptions != null)
            {
                passwordOptions(password);
            }

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IExternalRepository, ExternalRepository>();

            services.AddDbContext<MihaylovUsersDbContext>(options =>
                        options.UseSqlServer(connectionStringSettings.GetConnectionString()));

            services.AddIdentity<User, IdentityRole>(options =>
                        {
                            options.User.RequireUniqueEmail = true;
                            options.Password = password;
                        })
                    //.AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<MihaylovUsersDbContext>();

            return services;
        }

        public static IServiceCollection AddJwtTokenGenerator(this IServiceCollection services, Action<TokenSettings> settings)
        {
            services.Configure<TokenSettings>(settings);
            services.AddScoped<ITokenHelper, TokenHelper>();

            return services;
        }

        //public static IApplicationBuilder InitializeUsersDb(this IApplicationBuilder app, string adminRole)
        //{
        //    var serviceProvider = app.ApplicationServices;

        //    using var serviceScope = serviceProvider.CreateScope();
        //    var provider = serviceScope.ServiceProvider;
        //    var dbContect = provider.GetRequiredService<MihaylovUsersDbContext>();
        //    dbContect.Database.Migrate();

        //    var usersRepository = provider.GetRequiredService<IUsersRepository>();
        //    usersRepository.InitializeDatabaseAsync(adminRole).GetAwaiter().GetResult();

        //    return app;
        //}

        public static IServiceCollection InitializeUsersDb(this IServiceCollection serviceProvider, string adminRole)
        {
            using var provider = GetServiceProvider(serviceProvider);
            var usersRepository = provider.GetRequiredService<IUsersRepository>();
            usersRepository.InitializeDatabaseAsync(adminRole).GetAwaiter().GetResult();

            return serviceProvider;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
