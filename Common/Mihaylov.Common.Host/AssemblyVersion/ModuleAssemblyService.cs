using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.Options;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Common.Host.AssemblyVersion
{
    public class ModuleAssemblyService : IModuleAssemblyService
    {
        private readonly Assembly _rootAssembly;

        public ModuleAssemblyService(IOptions<AssemblyWrapper> currentAssembly)
        {
            _rootAssembly = currentAssembly.Value.Assembly;
        }

        public ModuleInfo GetModuleInfo()
        {
            var assemblyName = _rootAssembly.GetName();

            var framework = Environment.Version?.ToString();
            if (string.IsNullOrEmpty(framework))
            {
                framework = _rootAssembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName;

                if (string.IsNullOrEmpty(framework))
                {
                    framework = assemblyName.Name;
                }
            }

            var name = _rootAssembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? assemblyName.Name;
            var version = _rootAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? assemblyName.Version.ToString();

            DateTime buildDate = new FileInfo(_rootAssembly.Location).LastWriteTimeUtc;

            var result = new ModuleInfo(name, version, framework, buildDate.ToString("yyyy.MM.dd HH:mm"));
            return result;
        }
    }
}
