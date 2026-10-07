using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace Mihaylov.Common;

/// <summary>
/// Provides extension methods to configure host services, including module information registration, client JWT
/// authentication, and Swagger generation and UI.
/// </summary>
public static class DependenciesExtensions
{
    /// <summary>
    /// Registers module-related services and configures AssemblyWrapper with the specified assembly or the entry
    /// assembly.
    /// </summary>
    /// <param name="services">The IServiceCollection to which module services are added.</param>
    /// <param name="assembly">Optional assembly to assign to AssemblyWrapper; uses Assembly.GetEntryAssembly() when null.</param>
    /// <returns>The IServiceCollection after registrations to allow method chaining.</returns>
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

    /// <summary>
    /// Adds and configures JWT bearer authentication and an authorization policy using JwtTokenSettings and optional
    /// WebHostSettings.
    /// </summary>
    /// <param name="services">The service collection to which authentication and authorization services are added.</param>
    /// <param name="webSettings">Optional configuration action for WebHostSettings used for cookie-based token retrieval, username claim mapping,
    /// and login redirection in web scenarios.</param>
    /// <param name="settings">Configuration action for JwtTokenSettings that supplies secret, issuer, and audience values used to validate
    /// JWTs.</param>
    /// <returns>The IServiceCollection with authentication and authorization configured.</returns>
    public static IServiceCollection AddClientJwtAuthentication(this IServiceCollection services,
        Action<WebHostSettings> webSettings, Action<JwtTokenSettings> settings)
    {
        WebHostSettings webConfig = null;
        if (webSettings != null)
        {
            webConfig = new WebHostSettings();
            webSettings(webConfig);
        }

        var config = new JwtTokenSettings();
        settings(config);

        services.Configure<JwtTokenSettings>(a => a.Copy(config));

        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Secret));

        var authBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = UserConstants.AuthenticationScheme;
            options.DefaultScheme = UserConstants.AuthenticationScheme;
            options.DefaultChallengeScheme = UserConstants.AuthenticationScheme;
        });

        authBuilder.AddJwtBearer(options =>
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
        });

        if (webConfig != null)
        {
            authBuilder.AddIdentityCookies(o => { });
        }

        services.AddAuthorizationBuilder()
            .AddPolicy("Admim", policyBuilder =>
            {
                policyBuilder.RequireClaim(ClaimTypes.Role, UserConstants.AdminRole);
            });

        return services;
    }

    /// <summary>
    /// Configures Swagger/OpenAPI generation and registers Swagger services with the provided document metadata.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="name">The Swagger document name.</param>
    /// <param name="version">The API version string for the Swagger document.</param>
    /// <param name="title">The title for the Swagger document.</param>
    /// <param name="description">The description for the Swagger document.</param>
    /// <param name="isPrivate">True to require authentication for the Swagger endpoint; otherwise false.</param>
    /// <returns>The configured IServiceCollection for chaining.</returns>
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
                var authenticationScheme = UserConstants.AuthenticationScheme;

                var message = new StringBuilder();
                message.AppendLine($"JWT Authorization header using the {authenticationScheme} scheme.");
                message.AppendLine($"Enter '{authenticationScheme}' [space] and then your token in the text input below.");
                message.AppendLine($"Example: '{authenticationScheme} 12345abcdef'");

                options.AddSecurityDefinition(authenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,  // ApiKey or Http
                    Scheme = authenticationScheme,
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Description = message.ToString(),
                });

                options.OperationFilter<AutorizeOperationFilter>(authenticationScheme);
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Mihaylov")).ToList();

            options.SchemaFilter<EnumExtensionSchemaFilter>();
            options.DocumentFilter<SwaggerEnumDocumentFilter>(assemblies);
            options.UseAllOfToExtendReferenceSchemas();
            options.EnableAnnotations();
        });

        return services;
    }

    /// <summary>
    /// Adds and configures Swagger and Swagger UI to the application's request pipeline, using environment-configured
    /// scheme and path prefix to build the OpenAPI server URL.
    /// </summary>
    /// <param name="app">Application builder used to register Swagger and Swagger UI middleware.</param>
    /// <param name="schemeKey">Environment variable key that provides the URL scheme to use when generating the Swagger server URL; falls back
    /// to the incoming request scheme.</param>
    /// <param name="pathPrefixKey">Environment variable key that provides the base path prefix appended to the server URL; defaults to '/'.</param>
    /// <param name="verion">API documentation version segment used to construct the Swagger JSON endpoint path.</param>
    /// <param name="name">Display name for the Swagger UI endpoint.</param>
    /// <returns>The same IApplicationBuilder instance to allow further middleware configuration.</returns>
    public static IApplicationBuilder UseSwaggerCustom(this IApplicationBuilder app, string schemeKey, string pathPrefixKey, string verion, string name)
    {
        string basePath = Config.GetEnvironmentVariable(pathPrefixKey, "/");
        basePath = $"/{basePath.Trim('/')}";

        // app.MapOpenApi(); // Microsoft.AspNetCore.OpenApi, and 
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
}
