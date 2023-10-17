using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mihaylov.Common.Abstract.Databases.Interfaces;
using Mihaylov.Common.Abstract.Databases.Services;
using Mihaylov.Common.Abstract.Infrastructure.Interfaces;
using Mihaylov.Common.Abstract.Infrastructure.Service;
using Mihaylov.Common.Mapping;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mihaylov.Common.Abstract
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

        public static IServiceCollection AddSwaggerCustom(this IServiceCollection services, string name, string version, string title, string description, bool isPrivate)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(name, new OpenApiInfo
                {
                    Version = version,
                    Title = title,
                    Description = description
                });

                if (isPrivate)
                {
                    options.AddSwaggerAuthentication("Bearer");
                }
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerCustom(this IApplicationBuilder app, string schemeKey, string pathPrefixKey, string verion, string name)
        {
            string basePath = Config.GetEnvironmentVariable(pathPrefixKey, "/");
            basePath = $"/{basePath.Trim('/')}";

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    var scheme = Config.GetEnvironmentVariable(schemeKey, httpReq.Scheme);
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"{scheme}://{httpReq.Host.Value}{basePath}" }
                    };
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{basePath.TrimEnd('/')}/swagger/{verion}/swagger.json", name);
                c.RoutePrefix = string.Empty;
            });


            return app;
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

        private static void AddSwaggerAuthentication(this SwaggerGenOptions options, string authenticationScheme)
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
    }
}
