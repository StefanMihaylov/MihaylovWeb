using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace Mihaylov.UI.Mvc.Users
{
    public static class ApplicationPartManagerExtension
    {
        public static ApplicationPartManager AddUsersIdentityPages(this ApplicationPartManager manager)
        {
            var assembly = typeof(IdentityBuilderUIExtensions).Assembly;
            var factory = new ConsolidatedAssemblyApplicationPartFactory();
            var parts = factory.GetApplicationParts(assembly).ToList();

            foreach (var part in parts)
            {
                manager.ApplicationParts.Add(part);
            }

            manager.FeatureProviders.Add(new ViewVersionFeatureProvider(assembly));

            return manager;
        }
    }

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
