namespace Mihaylov.Common;

/// <summary>
/// Represents module metadata including name, version, target framework, build date, Git commit, and Jenkins build
/// number.
/// </summary>
/// <param name="ModuleName"></param>
/// <param name="Version"></param>
/// <param name="Framework"></param>
/// <param name="BuildDate"></param>
/// <param name="GitCommit"></param>
/// <param name="JenkinsBuildNumber"></param>
public record ModuleInfo(string ModuleName, string Version, string Framework, string BuildDate, string GitCommit, string JenkinsBuildNumber) : IModuleInfo;
