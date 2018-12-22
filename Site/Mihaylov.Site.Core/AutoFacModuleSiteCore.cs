using Autofac;
using Mihaylov.Common.MessageBus;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Core.Helpers.Site;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Core.Managers;
using Mihaylov.Site.Core.Writers;
using Mihaylov.Site.CsQuery;
using Mihaylov.Site.Data;

namespace Mihaylov.Site.Core
{
    public class AutoFacModuleSiteCore : Module
    {
        private readonly string connectionString;
        private readonly string url;

        public AutoFacModuleSiteCore(string connectionString, string url)
        {
            this.connectionString = connectionString;
            this.url = url;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutoFacModuleSiteData(this.connectionString));

            builder.RegisterType<SimpleMessageBus>().As<IMessageBus>().SingleInstance();

            builder.RegisterType<PersonsManager>().As<IPersonsManager>().SingleInstance();
            builder.RegisterType<PersonAdditionalInfoManager>().As<IPersonAdditionalInfoManager>().SingleInstance();
            builder.RegisterType<PhrasesManager>().As<IPhrasesManager>().SingleInstance();

            builder.RegisterType<CountriesWriter>().As<ICountriesWriter>();
            builder.RegisterType<PersonsWriter>().As<IPersonsWriter>();
            builder.RegisterType<PhrasesWriter>().As<IPhrasesWriter>();

            builder.RegisterType<CsQueryWrapper>().As<ICsQueryWrapper>();

            builder.RegisterType<SiteHelper>().As<ISiteHelper>().WithParameter("url", this.url);
        }
    }
}
