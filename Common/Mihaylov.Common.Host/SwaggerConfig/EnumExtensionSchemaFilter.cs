using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mihaylov.Common;

internal class EnumExtensionSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema model, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
        {
            return;
        }

        var schema = model as OpenApiSchema;
        if (schema == null)
        {
            return;
        }

        schema.Extensions ??= new Dictionary<string, IOpenApiExtension>();

        schema.Extensions.Add("x-enumNames", new JsonNodeExtension(GetNSwagEnumFilter(context.Type)));
        schema.Extensions.Add("x-ms-enum", new JsonNodeExtension(GetEnumFilter(context.Type)));
    }

    private static JsonNode GetNSwagEnumFilter(Type type)
    {
        var names = new JsonArray();
        foreach (var name in Enum.GetNames(type))
        {
            names.Add(name);
        }

        return names;
    }

    private static JsonNode GetEnumFilter(Type type)
    {
        var names = Enum.GetNames(type)
        .Distinct()
        .Select(value =>
        {
            return new JsonObject()
            {
                ["value"] = Convert.ToInt32(Enum.Parse(type, value)),
                ["name"] = value
            };
        })
        .ToArray();

        var result = new JsonObject()
        {
            ["name"] = type.Name,
            ["modelAsString"] = false,
            ["values"] = new JsonArray(names)
        };

        return result;
    }
}
