using Mihaylov.Common.Log4net;
using Mihaylov.Common.WebConfigSettings;
using Mihaylov.Common.WebConfigSettings.Interfaces;
using Mihaylov.Core;
using Mihaylov.Web.Common.Toastr;
using Ninject;
using Ninject.Modules;

namespace Mihaylov.Web.App_Start
{
    public class NinjectConfig
    {
        public static void Register(IKernel kernel)
        {
            // Log4netConfiguration.Setup(@"D:\Logs\MihaylovWeb\log.txt");
            // var loggerPath = HostingEnvironment.MapPath("~/App_Data/Log/log.txt");

            kernel.Bind<ICustomSettingsManager>().To<CustomSettingsManager>().InSingletonScope();

            var customSettingsManager = kernel.Get<ICustomSettingsManager>();
            var loggerPath = customSettingsManager.Settings.LoggerPath;
            var connectionString = customSettingsManager.GetSettingByName("MihaylovDb");
            var siteUrl = customSettingsManager.GetSettingByName("SiteUrl");

            Log4netConfiguration.Setup(loggerPath);

            //var dataStoreFolder = HostingEnvironment.MapPath("~/App_Data/Google");
            //kernel.Bind<IGoogleDriveApiHelper>().To<GoogleDriveApiHelper>().InSingletonScope()
            //    .WithConstructorArgument("dataStoreFolder", dataStoreFolder);

            kernel.Load(new INinjectModule[] { new NinjectModuleCore(connectionString, siteUrl) });
            kernel.Bind<IToastrHelper>().To<ToastrHelper>();
        }
    }
}