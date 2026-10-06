namespace Mihaylov.Common;

/// <summary>
/// Provides access to build and version information for the running system, including application version, Git commit
/// identifier, and CI build number.
/// </summary>
public interface ISystemConfiguration
{
    /// <summary>
    /// Gets the version string for the current assembly or component.
    /// </summary>
    /// <returns>A string that represents the version, typically using semantic versioning (for example, 1.2.3).</returns>
    string GetVersion();

    /// <summary>
    /// Gets the Git commit identifier associated with the current build or source state.
    /// </summary>
    /// <returns>A hexadecimal Git commit SHA (full or abbreviated), or an empty string if the commit identifier is unavailable.</returns>
    string GetGitCommit();

    /// <summary>
    /// Gets the Jenkins build number for the current build.
    /// </summary>
    /// <returns>The Jenkins build number as a string, or null if not available.</returns>
    string GetJenkinsBuildNumber();
}