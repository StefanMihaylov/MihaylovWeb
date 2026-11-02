using System.Reflection;
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
        var versionInfo = VersionHelper.GetVersionInfo(_rootAssembly);

        var framework = _systemConfig.GetVersion();
        if (string.IsNullOrWhiteSpace(framework))
        {
            framework = versionInfo.Framework;
        }

        var gitCommit = _systemConfig.GetGitCommit();
        var buildNumber = _systemConfig.GetJenkinsBuildNumber();

        var result = new ModuleInfo(versionInfo.ModuleName, versionInfo.Version, framework, versionInfo.BuildDate, gitCommit, buildNumber);

        return result;
    }
}
