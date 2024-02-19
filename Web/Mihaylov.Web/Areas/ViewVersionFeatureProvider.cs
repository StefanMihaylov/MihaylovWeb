using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace Mihaylov.Web.Areas
{
    public class ViewVersionFeatureProvider : IApplicationFeatureProvider<ViewsFeature>
    {
        private readonly Assembly _assembly;

        public ViewVersionFeatureProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ViewsFeature feature)
        {
            var descriptors = feature.ViewDescriptors
                .Where(d => d.RelativePath.StartsWith("/Areas/Identity", StringComparison.OrdinalIgnoreCase))
                .Where(d => d.Type?.Assembly == _assembly)
                .ToList();

            var viewsToRemove = new List<CompiledViewDescriptor>();
            foreach (var descriptor in descriptors)
            {
                if (descriptor.Type?.FullName?.Contains("V5", StringComparison.Ordinal) == true)
                {
                    viewsToRemove.Add(descriptor);
                }
                else
                {
                    descriptor.RelativePath = descriptor.RelativePath.Replace("V4/", string.Empty);
                }
            }

            foreach (var descriptorToRemove in viewsToRemove)
            {
                feature.ViewDescriptors.Remove(descriptorToRemove);
            }
        }
    }
}
