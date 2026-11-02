using System;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Mihaylov.Common;

/// <summary>
/// Provides extension methods to configure host services, including module information registration, client JWT authentication
/// </summary>
public static class HostServiceExtensions
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
    /// Adds and configures JWT bearer authentication and an authorization policy using TokenSettings and optional WebHostSettings.
    /// </summary>
    /// <param name="services">The service collection to which authentication and authorization services are added.</param>
    /// <param name="defaultScheme"></param>
    /// <param name="authorizationOptions"></param>
    /// <param name="webSettings">Optional configuration action for WebHostSettings used for cookie-based token retrieval, username claim mapping,
    /// and login redirection in web scenarios.</param>
    /// <param name="settings">Configuration action for TokenSettings that supplies secret, issuer, and audience values used to validate JWTs.</param>
    /// <returns>The IServiceCollection with authentication and authorization configured.</returns>
    public static IServiceCollection AddClientJwtAuthentication(this IServiceCollection services,
        string defaultScheme, Action<AuthorizationOptions> authorizationOptions,
        Action<WebHostSettings> webSettings, Action<TokenSettings> settings)
    {
        WebHostSettings webConfig = null;
        if (webSettings != null)
        {
            webConfig = new WebHostSettings();
            webSettings(webConfig);
        }

        services.Configure<TokenSettings>(settings);

        var config = new TokenSettings();
        settings(config);

        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Secret));

        var authBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = defaultScheme;
            options.DefaultScheme = defaultScheme;
            options.DefaultChallengeScheme = defaultScheme;
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
                NameClaimType = config.NameClaimType,
                RoleClaimType = ClaimsIdentity.DefaultRoleClaimType,
            };
            options.Events = GetEvents(webConfig);
        });

        if (webConfig != null)
        {
            authBuilder.AddIdentityCookies(o => { });
        }

        services.AddAuthorization(authorizationOptions);

        return services;
    }

    private static JwtBearerEvents GetEvents(WebHostSettings webConfig)
    {
        return new JwtBearerEvents
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
                if (webConfig != null)
                {
                    context.HttpContext.Response.Cookies.Delete(webConfig.CookieName);
                    context.HttpContext.Response.Redirect(webConfig.LoginUrl);
                }
                else
                {
                    context.Response.StatusCode = 401;
                }

                return Task.CompletedTask;
            }
        };
    }
}
