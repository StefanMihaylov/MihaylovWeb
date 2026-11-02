namespace Mihaylov.Common
{
    /// <summary>
    /// Represents module metadata including name, version, target framework, build date, Git commit, 
    /// and Jenkins build number.
    /// </summary>
    public class ModuleInfo : IModuleInfo
    {
        /// <inheritdoc />
        public string ModuleName { get; }

        /// <inheritdoc />
        public string Version { get; }

        /// <inheritdoc />
        public string Framework { get; }

        /// <inheritdoc />
        public string BuildDate { get; }

        /// <inheritdoc />
        public string GitCommit { get; }

        /// <inheritdoc />
        public string JenkinsBuildNumber { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInfo"/> class by copying the values of another <see cref="IModuleInfo"/>.
        /// </summary>
        /// <param name="input">The module info to copy.</param>
        public ModuleInfo(IModuleInfo input)
            : this(input.ModuleName, input.Version, input.Framework, input.BuildDate, input.GitCommit,input.JenkinsBuildNumber)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ModuleInfo class with module metadata values.
        /// </summary>
        /// <param name="moduleName">Module name.</param>
        /// <param name="version">Module version.</param>
        /// <param name="framework">.NET framework version.</param>
        /// <param name="buildDate">Build date of the module.</param>
        /// <param name="gitCommit">Git commit hash associated with the build.</param>
        /// <param name="jenkinsBuildNumber">Jenkins build number.</param>
        public ModuleInfo(string moduleName, string version, string framework, string buildDate, string gitCommit,
            string jenkinsBuildNumber)
        {
            ModuleName = moduleName;
            Version = version;
            Framework = framework;
            BuildDate = buildDate;
            GitCommit = gitCommit;
            JenkinsBuildNumber = jenkinsBuildNumber;
        }
    }
}
