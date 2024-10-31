using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mihaylov.Common.Host.SwaggerConfig
{
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
                throw new Exception($"SwaggerDoc: Can not find a unique Enum for specified typeName '{enumTypeName}'. Please provide a more unique enum name. Error: {e.Message}");
            }
        }
    }
}
