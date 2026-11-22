using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

namespace Mihaylov.Common;

/// <summary>
/// Swagger extension methods - Swagger generation and UI
/// </summary>
public static class SwaggerServiceExtensions
{
    /// <summary>
    /// Configures Swagger/OpenAPI generation and registers Swagger services with the provided document metadata.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="name">The Swagger document name.</param>
    /// <param name="apiVersion">The API version string for the Swagger document.</param>
    /// <param name="title">The title for the Swagger document.</param>
    /// <param name="description">The description for the Swagger document.</param>
    /// <param name="isPrivate">True to add the authentication scheme (Authorize button) and security requirements to the Swagger document.</param>
    /// <returns>The configured IServiceCollection for chaining.</returns>
    public static IServiceCollection AddSwaggerCustom(this IServiceCollection services, string name, string apiVersion, string title, string description, bool isPrivate)
    {
        services.AddSwaggerGen(options =>
        {
            using var provider = services.BuildServiceProvider();
            var assemblyWrapper = provider.GetRequiredService<IOptions<AssemblyWrapper>>();
            var assembly = assemblyWrapper.Value.Assembly;
            var moduleInfo = VersionHelper.GetVersionInfo(assembly);

            options.SwaggerDoc(name, new OpenApiInfo
            {
                Version = apiVersion,
                Title = title,
                Description = $"{description} | ApiName = {moduleInfo.ModuleName}, Version = {moduleInfo.Version}",
            });

            if (isPrivate)
            {
                var message = new StringBuilder();
                message.AppendLine($"JWT Authorization header using the {UserConstants.AuthenticationScheme} scheme.");
                message.AppendLine($"Enter '{UserConstants.AuthenticationScheme}' [space] and then your token in the text input below.");
                message.AppendLine($"Example: '{UserConstants.AuthenticationScheme} 12345abcdef'");

                options.AddSecurityDefinition(UserConstants.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,  // ApiKey or Http
                    Scheme = UserConstants.AuthenticationScheme,
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Description = message.ToString(),
                });

                options.OperationFilter<AuthorizeOperationFilter>(UserConstants.AuthenticationScheme);
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
    /// <param name="apiVersion">API documentation version segment used to construct the Swagger JSON endpoint path.</param>
    /// <param name="name">Display name for the Swagger UI endpoint.</param>
    /// <returns>The same IApplicationBuilder instance to allow further middleware configuration.</returns>
    public static IApplicationBuilder UseSwaggerCustom(this IApplicationBuilder app, string schemeKey, string pathPrefixKey, string apiVersion, string name)
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
            c.SwaggerEndpoint($"{basePath.TrimEnd('/')}/swagger/{apiVersion}/swagger.json", name);
            c.RoutePrefix = string.Empty;
        });

        return app;
    }
}
