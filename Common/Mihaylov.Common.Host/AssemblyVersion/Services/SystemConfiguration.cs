using System;

namespace Mihaylov.Common;

/// <summary>
/// Provides system configuration details, including the .NET runtime version and build metadata sourced from
/// environment variables.
/// </summary>
public class SystemConfiguration : ISystemConfiguration
{
    /// <summary>
    /// Gets the .NET runtime version as a string prefixed with ".NET ".
    /// </summary>
    /// <returns>A string containing the runtime version prefixed with ".NET "; if the runtime version is unavailable, returns
    /// ".NET ".</returns>
    public string GetVersion()
    {
        var version = Environment.Version?.ToString();

        return $".NET {version}";
    }

    /// <summary>
    /// Gets the Git commit hash from the GIT_COMMIT environment variable.
    /// </summary>
    /// <returns>The commit hash from the GIT_COMMIT environment variable, or an empty string if the variable is not set.</returns>
    public string GetGitCommit()
    {
        var commit = Config.GetEnvironmentVariable("GIT_COMMIT", string.Empty);

        return commit;
    }

    /// <summary>
    /// Gets the Jenkins build number from the 'Jenkins_Build' environment variable.
    /// </summary>
    /// <returns>The Jenkins build number, or an empty string if the 'Jenkins_Build' environment variable is not set.</returns>
    public string GetJenkinsBuildNumber()
    {
        var build = Config.GetEnvironmentVariable("Jenkins_Build", string.Empty);

        return build;
    }
}
