using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Common.Host.AssemblyVersion.Models
{
    public interface IModuleInfo
    {
        [Display(Name = "Module")]
        string ModuleName { get; }

        [Display(Name = "Version")]
        string Version { get; }

        [Display(Name = "Framework")]
        string Framework { get; }

        [Display(Name = "Build Date")]
        string BuildDate { get; }

        [Display(Name = "Git Commit")]
        string GitCommit { get; }

        [Display(Name = "Build Number")]
        string JenkinsBuildNumber { get; }
    }
}