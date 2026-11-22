using System.ComponentModel.DataAnnotations;
using Mihaylov.Common;

namespace Mihaylov.UI.Mvc.Home.Models.Home
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

        [Display(Name = "UI Name")]
        public string UiModuleName { get; private set; }

        [Display(Name = "UI Version")]
        public string UiModuleVersion { get; private set; }

        [Display(Name = "UI Framework")]
        public string UiFramework { get; private set; }

        [Display(Name = "UI Build Date")]
        public string UiBuildDate { get; private set; }

        [Display(Name = "Menu Orders")]
        public string MenuOrders { get; private set; }


        public ModuleInfoModel(ComponentModuleInfo input)
        {
            ModuleName = input.ServerModuleInfo?.ModuleName;
            Version = input.ServerModuleInfo?.Version;
            Framework = input.ServerModuleInfo?.Framework;
            BuildDate = input.ServerModuleInfo?.BuildDate;
            GitCommit = input.ServerModuleInfo?.GitCommit;
            JenkinsBuildNumber = input.ServerModuleInfo?.JenkinsBuildNumber;

            UiModuleName = input.UiModuleInfo?.ModuleName;
            UiModuleVersion = input.UiModuleInfo?.Version;
            UiFramework = input.UiModuleInfo?.Framework;
            UiBuildDate = input.UiModuleInfo?.BuildDate;

            MenuOrders = string.Join(", ", input.MenuOrders ?? []);
        }
    }
}
