using System;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Data.Helpers;
using Mihaylov.Api.Site.Data.Managers;
using Mihaylov.Api.Site.Data.Models;
using Mihaylov.Api.Site.Data.Writers;

namespace Mihaylov.Api.Site.Data
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddSiteServices(this IServiceCollection services, Action<SiteOptions> siteOption)
        {
            services.Configure<SiteOptions>(siteOption);

            services.AddScoped<ICollectionManager, CollectionManager>();
            services.AddScoped<IPersonsManager, PersonsManager>();
            services.AddScoped<IQuizManager, QuizManager>();

            services.AddScoped<ICollectionWriter, CollectionWriter>();
            services.AddScoped<IPersonsWriter, PersonsWriter>();
            services.AddScoped<IQuizWriter, QuizWriter>();

            services.AddScoped<ICsQueryWrapper, CsQueryWrapper>();
            services.AddScoped<ISiteHelper, SiteHelper>();

            return services;
        }
    }
}
