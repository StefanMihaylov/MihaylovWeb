using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Common.Host.AssemblyVersion
{
    public class ModuleAssemblyService : IModuleAssemblyService
    {
        private readonly Assembly _rootAssembly;
        private readonly ISystemConfiguration _config;

        public ModuleAssemblyService(IOptions<AssemblyWrapper> currentAssembly, ISystemConfiguration configuration)
        {
            _rootAssembly = currentAssembly.Value.Assembly;
            _config = configuration;
        }

        public ModuleInfo GetModuleInfo()
        {
            var assemblyName = _rootAssembly.GetName();

            var framework = _config.GetVersion();
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

            var gitCommit = _config.GetGitCommit();

            var result = new ModuleInfo(name, version, framework, buildDate.ToString("yyyy.MM.dd HH:mm"), gitCommit);
            return result;
        }
    }
}
