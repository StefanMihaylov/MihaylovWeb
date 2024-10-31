using System;
using System.Linq;
using System.Text.Json;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mihaylov.Common.Host.SwaggerConfig
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
}
