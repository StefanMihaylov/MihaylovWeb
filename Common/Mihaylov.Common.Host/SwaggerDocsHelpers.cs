using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mihaylov.Common.Host
{
    public class SwaggerDocsHelpers
    {
        internal class EnumExtensionSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema model, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    model.Extensions.Add("x-enumNames", new NSwagEnumFilterOpenApiExtension(context));
                    model.Extensions.Add("x-ms-enum", new EnumFilterOpenApiExtension(context));
                }
            }

            private class NSwagEnumFilterOpenApiExtension : IOpenApiExtension
            {
                private readonly SchemaFilterContext _context;

                public NSwagEnumFilterOpenApiExtension(SchemaFilterContext context)
                {
                    _context = context;
                }

                public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
                {
                    var names = Enum.GetNames(_context.Type);

                    var options = new JsonSerializerOptions() { WriteIndented = true };
                    writer.WriteRaw(JsonSerializer.Serialize(names, options));
                }
            }

            private class EnumFilterOpenApiExtension : IOpenApiExtension
            {
                private readonly SchemaFilterContext _context;

                public EnumFilterOpenApiExtension(SchemaFilterContext context)
                {
                    _context = context;
                }

                public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
                {
                    var enumType = _context.Type;

                    var names = Enum.GetNames(enumType)
                            .Distinct()
                            .Select(value =>
                            {
                                return new
                                {
                                    value = Convert.ToInt32(Enum.Parse(enumType, value)),
                                    name = value
                                };
                            })
                            .ToArray();

                    var model = new
                    {
                        name = enumType.Name,
                        modelAsString = false,
                        values = names
                    };

                    var options = new JsonSerializerOptions() { WriteIndented = true };
                    writer.WriteRaw(JsonSerializer.Serialize(model, options));
                }
            }
        }

        internal class SwaggerEnumDocumentFilter : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                // add enum descriptions to result models
                foreach (var property in swaggerDoc.Components.Schemas)
                {
                    IList<IOpenApiAny> propertyEnums = property.Value.Enum;
                    if (propertyEnums.Count > 0)
                    {
                        property.Value.Description += DescribeEnum(propertyEnums, property.Key);
                    }
                }
            }


            private string DescribeEnum(IEnumerable<IOpenApiAny> enums, string propertyTypeName)
            {
                var enumType = GetEnumTypeByName(propertyTypeName);

                if (enumType == null)
                {
                    return null;
                }

                var parsedEnums = new List<OpenApiInteger>();
                foreach (var @enum in enums)
                {
                    if (@enum is OpenApiInteger enumInt)
                    {
                        parsedEnums.Add(enumInt);
                    }
                }

                return string.Join(", ", parsedEnums.Select(x => $"{x.Value} - {Enum.GetName(enumType, x.Value)}"));
            }

            private Type GetEnumTypeByName(string enumTypeName)
            {
                if (string.IsNullOrEmpty(enumTypeName))
                {
                    return null;
                }

                try
                {
                    return AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .SelectMany(x => x.GetTypes())
                                    .Single(x => x.FullName != null
                                              && x.Name == enumTypeName);
                }
                catch (InvalidOperationException e)
                {
                    throw new Exception($"SwaggerDoc: Can not find a unique Enum for specified typeName '{enumTypeName}'. Please provide a more unique enum name.");
                }
            }
        }
    }
}
