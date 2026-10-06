using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.Options;

namespace Mihaylov.Common;

/// <summary>
/// Provides module metadata by inspecting the application assembly and consulting ISystemConfiguration.
/// </summary>
public class ModuleAssemblyService : IModuleAssemblyService
{
    private readonly Assembly _rootAssembly;
    private readonly ISystemConfiguration _systemConfig;

    /// <summary>
    /// Initializes a new instance of ModuleAssemblyService.
    /// </summary>
    /// <param name="currentAssembly">Options wrapping an AssemblyWrapper that provides the root assembly.</param>
    /// <param name="configuration">System configuration used by the service.</param>
    public ModuleAssemblyService(IOptions<AssemblyWrapper> currentAssembly, ISystemConfiguration configuration)
    {
        _rootAssembly = currentAssembly.Value.Assembly;
        _systemConfig = configuration;
    }

    /// <summary>
    /// Gets module metadata including product name, version, target framework, build date (UTC), git commit, and build
    /// number.
    /// </summary>
    /// <returns>An IModuleInfo containing the module's product name, version, target framework, build date (UTC) formatted as
    /// 'yyyy.MM.dd HH:mm', git commit, and build number.</returns>
    public IModuleInfo GetModuleInfo()
    {
        var assemblyName = _rootAssembly.GetName();

        var framework = _systemConfig.GetVersion();
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

        var gitCommit = _systemConfig.GetGitCommit();
        var buildNumber = _systemConfig.GetJenkinsBuildNumber();

        var result = new ModuleInfo(name, version, framework, buildDate.ToString("yyyy.MM.dd HH:mm"), gitCommit, buildNumber);
        
        return result;
    }
}
