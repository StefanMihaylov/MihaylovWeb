using System;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common.Databases;
using Mihaylov.Common.MessageBus;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Core.Helpers.Site;
using Mihaylov.Site.Core.CsQuery;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Core.Managers;
using Mihaylov.Site.Core.Writers;
using Mihaylov.Site.Data;

namespace Mihaylov.Site.Core
{
    public static class HostConfigurations
    {
        public static IServiceCollection AddSiteCore(this IServiceCollection services, 
            Action<ConnectionStringSettings> connectionString, Action<SiteCoreOptions> siteOption)
        {
            services.AddSiteRepositories(connectionString, ServiceLifetime.Singleton);

            services.Configure<SiteCoreOptions>(siteOption);
            services.AddSingleton<IMessageBus, SimpleMessageBus>();

            services.AddSingleton<IPersonsManager, PersonsManager>();
            services.AddSingleton<IPersonAdditionalInfoManager, PersonAdditionalInfoManager>();
            services.AddSingleton<IPhrasesManager, PhrasesManager>();

            services.AddScoped<ICountriesWriter, CountriesWriter>();
            services.AddScoped<IPersonsWriter, PersonsWriter>();
            services.AddScoped<IPhrasesWriter, PhrasesWriter>();

            services.AddScoped<ICsQueryWrapper, CsQueryWrapper>();
            
            services.AddScoped<ISiteHelper, SiteHelper>();

            return services;
        }
    }
}
