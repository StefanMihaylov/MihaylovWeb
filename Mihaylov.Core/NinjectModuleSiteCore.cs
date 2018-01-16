using Mihaylov.Common.MessageBus;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Core.Helpers.Site;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Core.Managers.Site;
using Mihaylov.Core.Providers.Site;
using Mihaylov.Core.Writers.Site;
using Mihaylov.Data;

using Ninject.Modules;

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

            Kernel.Bind<IMessageBus>().To<SimpleMessageBus>().InSingletonScope();

            Kernel.Bind<IPersonsProvider>().To<PersonsProvider>();
            Kernel.Bind<IPersonAdditionalInfoProvider>().To<PersonAdditionalInfoProvider>();
            Kernel.Bind<IPhrasesProvider>().To<PhrasesProvider>();

            Kernel.Bind<IPersonsManager>().To<PersonsManager>().InSingletonScope();
            Kernel.Bind<IPersonAdditionalInfoManager>().To<PersonAdditionalInfoManager>().InSingletonScope();
            Kernel.Bind<IPhrasesManager>().To<PhrasesManager>().InSingletonScope();

            Kernel.Bind<ICountriesWriter>().To<CountriesWriter>();
            Kernel.Bind<IPersonsWriter>().To<PersonsWriter>();
            Kernel.Bind<IPhrasesWriter>().To<PhrasesWriter>();

            Kernel.Bind<ISiteHelper>().To<SiteHelper>().WithConstructorArgument("url", this.url);
        }
    }
}
