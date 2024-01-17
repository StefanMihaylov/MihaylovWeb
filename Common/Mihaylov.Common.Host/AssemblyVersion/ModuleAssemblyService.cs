using System;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.Options;
using Mihaylov.Common.Host.BuildDate;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Common.Host.AssemblyVersion
{
    public class ModuleAssemblyService : IModuleAssemblyService
    {
        private readonly Assembly _rootAssembly;
        private readonly Assembly _currentAssembly;

        public ModuleAssemblyService(IOptions<AssemblyWrapper> currentAssembly)
        {
            _rootAssembly = currentAssembly.Value.Assembly;
            _currentAssembly = Assembly.GetExecutingAssembly();
        }

        public ModuleInfo GetModuleInfo()
        {
            var assemblyName = _rootAssembly.GetName();
            var targetFramework = _rootAssembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName ?? string.Empty;
            var name = _rootAssembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? assemblyName.Name;
            var version = _rootAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? assemblyName.Version.ToString();

            DateTime? buildDate = _currentAssembly.GetCustomAttribute<ReleaseDateAttribute>()?.ReleaseDate;
            var buildDateStr = buildDate?.ToString("yyyy.MM.dd HH:mm") ?? string.Empty;

            var result = new ModuleInfo(name, version, targetFramework, buildDateStr);
            return result;
        }
    }
}
