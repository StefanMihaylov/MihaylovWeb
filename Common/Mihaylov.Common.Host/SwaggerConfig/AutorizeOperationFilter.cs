using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mihaylov.Common;

/// <summary>
/// Adds an OpenAPI security requirement to an operation when an AuthorizeAttribute is present and no
/// AllowAnonymousAttribute is applied.
/// </summary>
/// <param name="authenticationScheme">Authentication scheme name used to create the OpenAPI security scheme reference.</param>
internal class AutorizeOperationFilter(string authenticationScheme) : IOperationFilter
{
    /// <summary>
    /// Adds a security requirement to the OpenAPI operation when the action or controller is protected by Authorize and
    /// not AllowAnonymous.
    /// </summary>
    /// <param name="operation">The OpenApiOperation to modify.</param>
    /// <param name="context">The OperationFilterContext providing MethodInfo, declaring type attributes, and the OpenAPI document used to
    /// build the security requirement.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attributes = context.MethodInfo.GetCustomAttributes(true)
                .Concat(context.MethodInfo.DeclaringType?.GetCustomAttributes(true) ?? [])
                .ToList();

        var hasAuthorize = attributes.OfType<AuthorizeAttribute>().Any();
        var hasAllowAnonymous = attributes.OfType<AllowAnonymousAttribute>().Any();

        if (hasAuthorize && !hasAllowAnonymous)
        {            
            var reference = new OpenApiSecuritySchemeReference(authenticationScheme, context.Document);
            var values = new List<string>();

            var requirement = new OpenApiSecurityRequirement();
            requirement.Add(reference, values);

            operation.Security ??= [];
            operation.Security.Add(requirement);
        }
    }
}
