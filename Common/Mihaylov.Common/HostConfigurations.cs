using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mihaylov.Common.Databases.Interfaces;
using Mihaylov.Common.Databases.Services;
using Mihaylov.Common.Infrastructure.Interfaces;
using Mihaylov.Common.Infrastructure.Services;
using Mihaylov.Common.Mapping;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mihaylov.Common
{
    public static class HostConfigurations
    {
        public static IServiceCollection AddMapping(this IServiceCollection services, Assembly currentAssembly, params string[] assemblies)
        {
            var autoMapper = new AutoMapperConfigurator(currentAssembly, assemblies);
            autoMapper.Execute();

            return services;
        }

        public static IServiceCollection AddCommon(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services
                .Add<ICurrentUserService, CurrentUserService>(lifetime)
                .Add<IAuditService, AuditService>(lifetime);

            return services;
        }
        
        public static IServiceCollection Add<TInterface, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TImplementation), lifetime));

            return services;
        }

        public static void AddSwaggerAuthentication(this SwaggerGenOptions options, string authenticationScheme)
        {
            options.AddSecurityDefinition(authenticationScheme, new OpenApiSecurityScheme
            {
                Description = $@"JWT Authorization header using the {authenticationScheme} scheme.
                      Enter '{authenticationScheme}' [space] and then your token in the text input below. 
                      Example: '{authenticationScheme} 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = authenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = authenticationScheme
                        },
                        Scheme = "oauth2",
                        Name = authenticationScheme,
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        }

        public static IServiceCollection MigrateDatabase<T>(this IServiceCollection serviceProvider) where T : DbContext
        {
            using (var provider = serviceProvider.BuildServiceProvider())
            {
                var dbContect = provider.GetRequiredService<T>();
                dbContect.Database.Migrate();
            }

            return serviceProvider;
        }

    }
}
