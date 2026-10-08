using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mihaylov.Common;

internal class SwaggerEnumDocumentFilter(IEnumerable<Assembly> assemblies) : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // add enum descriptions to result models
        foreach (var property in swaggerDoc.Components.Schemas)
        {
            IList<JsonNode> enumProperties = property.Value?.Enum;
            if (enumProperties?.Count > 0)
            {
                Type enumType = GetEnumTypeByName(property.Key);
                property.Value.Description += DescribeEnum(enumType, enumProperties);
            }
        }
    }

    private static string DescribeEnum(Type enumType, IEnumerable<JsonNode> enumNodes)
    {
        if (enumType == null)
        {
            return null;
        }

        var parsedEnums = new List<string>();
        foreach (var enumNode in enumNodes)
        {
            JsonValue enumValue = enumNode.AsValue();
            if (enumValue.TryGetValue<int>(out int enumInt))
            {
                parsedEnums.Add($"{enumInt} - {Enum.GetName(enumType, enumInt)}");
            }
        }

        return string.Join(", ", parsedEnums);
    }

    private Type GetEnumTypeByName(string enumTypeName)
    {
        if (string.IsNullOrEmpty(enumTypeName))
        {
            return null;
        }

        try
        {
            var type = assemblies.SelectMany(x => x.GetTypes())
                                 .Single(x => x.FullName != null && x.Name == enumTypeName);

            return type;
        }
        catch (InvalidOperationException e)
        {
            throw new Exception($"SwaggerDoc: Can not find a unique Enum for specified typeName '{enumTypeName}'. Please provide a more unique enum name. Error: {e.Message}");
        }
    }
}
