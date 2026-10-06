namespace Mihaylov.Common
{
    /// <summary>
    /// Represents metadata for a module, including its name, version, target framework, build date, source commit, and CI build number.
    /// </summary>
    public interface IModuleInfo
    {        
        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        string ModuleName { get; }

        /// <summary>
        /// Gets the version identifier for the component.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the target framework identifier for the current component.
        /// </summary>
        string Framework { get; }

        /// <summary>
        /// Gets the build date and time of the assembly or application.
        /// </summary>
        string BuildDate { get; }

        /// <summary>
        /// Gets the Git commit SHA identifying the source revision used to produce the build.
        /// </summary>
        string GitCommit { get; }

        /// <summary>
        /// Gets the Jenkins build number.
        /// </summary>
        string JenkinsBuildNumber { get; }
    }
}