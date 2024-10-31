using Mihaylov.Common.Generic.AssemblyVersion;

namespace Mihaylov.Common.Host.AssemblyVersion.Models
{
    public class ModuleInfo : IModuleInfo
    {        
        public string ModuleName { get; private set; }
        
        public string Version { get; private set; }
        
        public string Framework { get; private set; }
        
        public string BuildDate { get; private set; }
        
        public string GitCommit { get; private set; }

        public string JenkinsBuildNumber { get; private set; }


        public ModuleInfo(string moduleName, string version, string framework, string buildDate, string gitCommit, string jenkinsBuildNumber)
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
