using Mihaylov.Core.Helpers.Site;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Core.Managers.Site;
using Mihaylov.Core.Providers.Site;
using Mihaylov.Core.Writers.Site;
using Mihaylov.Database.Interfaces;
using DAL = Mihaylov.Database.Site;
using Ninject.Modules;
using Mihaylov.Data;

namespace Mihaylov.Core
{
    public class NinjectModuleSiteCore : NinjectModule
    {
        private readonly string connectionString;
        private readonly string url;

        public NinjectModuleSiteCore(string connectionString, string url)
        {
            this.connectionString = connectionString;
            this.url = url;
        }

        public override void Load()
        {
            Kernel.Load(new INinjectModule[] { new NinjectModuleSiteData(this.connectionString) });

            Kernel.Bind<IPersonsProvider>().To<PersonsProvider>();
            Kernel.Bind<IPersonAdditionalInfoProvider>().To<PersonAdditionalInfoProvider>();

            Kernel.Bind<IPersonsManager>().To<PersonsManager>().InSingletonScope();
            Kernel.Bind<IPersonAdditionalInfoManager>().To<PersonAdditionalInfoManager>().InSingletonScope();

            Kernel.Bind<ICountriesWriter>().To<CountriesWriter>();
            Kernel.Bind<IPersonsWriter>().To<PersonsWriter>();

            Kernel.Bind<ISiteHelper>().To<SiteHelper>().WithConstructorArgument("url", this.url);
        }
    }
}
