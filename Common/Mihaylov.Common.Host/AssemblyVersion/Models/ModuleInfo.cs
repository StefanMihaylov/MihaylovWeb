using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Common.Host.AssemblyVersion.Models
{
    public class ModuleInfo
    {
        [Display(Name = "Module")]
        public string ModuleName { get; private set; }

        [Display(Name = "Version")]
        public string Version { get; private set; }

        [Display(Name = "Framework")]
        public string Framework { get; private set; }

        [Display(Name = "Build Date")]
        public string BuildDate { get; private set; }


        public ModuleInfo(string moduleName, string version, string framework, string buildDate)
        {
            ModuleName = moduleName;
            Version = version;
            Framework = framework;
            BuildDate = buildDate;
        }
    }
}
