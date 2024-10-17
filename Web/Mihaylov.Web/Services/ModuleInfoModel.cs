using System.ComponentModel.DataAnnotations;
using Mihaylov.Common.Host.Abstract.AssemblyVersion;

namespace Mihaylov.Web.Services
{
    public class ModuleInfoModel
    {
        [Display(Name = "Module")]
        public string ModuleName { get; private set; }

        [Display(Name = "Version")]
        public string Version { get; private set; }

        [Display(Name = "Framework")]
        public string Framework { get; private set; }

        [Display(Name = "Build Date")]
        public string BuildDate { get; private set; }

        [Display(Name = "Git Commit")]
        public string GitCommit { get; private set; }

        [Display(Name = "Build Number")]
        public string JenkinsBuildNumber { get; private set; }


        public ModuleInfoModel(IModuleInfo input)
        {
            ModuleName = input.ModuleName;
            Version = input.Version;
            Framework = input.Framework;
            BuildDate = input.BuildDate;
            GitCommit = input.GitCommit;
            JenkinsBuildNumber = input.JenkinsBuildNumber;
        }
    }
}
