using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using Mihaylov.Common.Host.AssemblyVersion.Models;
using Mihaylov.Common.Host.AssemblyVersion.Services;
using Mihaylov.Common.Host.Authorization;
using Mihaylov.Common.Host.SwaggerConfig;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mihaylov.Common
{
    public static class _HostConfiguration
    {
        public static IServiceCollection AddModuleInfo(this IServiceCollection services, Assembly assembly = null)
        {
            services.Configure<AssemblyWrapper>(ar =>
            {
                ar.Assembly = assembly ?? Assembly.GetEntryAssembly();
            });

            services.AddScoped<IModuleAssemblyService, ModuleAssemblyService>();
            services.AddScoped<ISystemConfiguration, SystemConfiguration>();

            return services;
        }

        public static IServiceCollection AddClientJwtAuthentication(this IServiceCollection services,
            Action<WebHostSettings> webSettings, Action<JwtTokenSettings> settings)
        {
            WebHostSettings webConfig = null;
            if(webSettings != null)
            {
                webConfig = new WebHostSettings();
                webSettings(webConfig);
            }

            var config = new JwtTokenSettings();
            settings(config);

            services.Configure<JwtTokenSettings>(a => a.Copy(config));

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Secret));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = UserConstants.AuthenticationScheme;
                options.DefaultScheme = UserConstants.AuthenticationScheme;
                options.DefaultChallengeScheme = UserConstants.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = signingKey,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = config.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = config.Audience,
                    ValidateAudience = true,
                    NameClaimType = webConfig?.UsernameClaimType?.GetClaim() ?? ClaimTypes.Upn,
                    RoleClaimType = ClaimsIdentity.DefaultRoleClaimType,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (webConfig != null)
                        {
                            context.Token = context.Request.Cookies[webConfig.CookieName];
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        if (webConfig == null)
                        {
                            context.Response.StatusCode = 401;
                        }
                        else
                        {
                            context.HttpContext.Response.Cookies.Delete(webConfig.CookieName);
                            context.HttpContext.Response.Redirect(webConfig.LoginUrl);
                        }

                        return Task.CompletedTask;
                    }
                };
            })
            .AddIdentityCookies(o => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admim", policyBuilder =>
                {
                    policyBuilder.RequireClaim(ClaimTypes.Role, UserConstants.AdminRole);
                });
            });

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

                options.SchemaFilter<EnumExtensionSchemaFilter>();
                options.DocumentFilter<SwaggerEnumDocumentFilter>();
                options.UseAllOfToExtendReferenceSchemas();
                options.EnableAnnotations();
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
