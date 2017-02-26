using Mihaylov.Core.Helpers;
using Mihaylov.Core.Interfaces;
using Mihaylov.Core.Managers;
using Mihaylov.Core.Providers;
using Mihaylov.Data;
using Ninject.Modules;

namespace Mihailov.Core
{
    public class NinjectModuleCore : NinjectModule
    {
        private readonly string connectionString;
        private readonly string url;

        public NinjectModuleCore(string connectionString, string url)
        {
            this.connectionString = connectionString;
            this.url = url;
        }

        public override void Load()
        {
            Kernel.Load(new INinjectModule[] { new NinjectModuleData(this.connectionString) });

            Kernel.Bind<IAnswerTypesProvider>().To<AnswerTypesProvider>();
            Kernel.Bind<ICountriesProvider>().To<CountriesProvider>();
            Kernel.Bind<IEthnicitiesProvider>().To<EthnicitiesProvider>();
            Kernel.Bind<IOrientationsProvider>().To<OrientationsProvider>();
            Kernel.Bind<IUnitsProvider>().To<UnitsProvider>();

            Kernel.Bind<IAnswerTypesManager>().To<AnswerTypesManager>().InSingletonScope();
            Kernel.Bind<ICountriesManager>().To<CountriesManager>().InSingletonScope();
            Kernel.Bind<IEthnicitiesManager>().To<EthnicitiesManager>().InSingletonScope();
            Kernel.Bind<IOrientationsManager>().To<OrientationsManager>().InSingletonScope();
            Kernel.Bind<IUnitsManager>().To<UnitsManager>().InSingletonScope();

            Kernel.Bind<ISiteHelper>().To<SiteHelper>().WithConstructorArgument("url", this.url);
        }
    }
}
