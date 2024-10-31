using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Data.Helpers;
using Mihaylov.Api.Site.Data.Managers;
using Mihaylov.Api.Site.Data.Models;
using Mihaylov.Api.Site.Data.Writers;

namespace Mihaylov.Api
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

            services.AddScoped<ISiteHelper, SiteHelper>();

            services.AddHttpClient("SiteHelper")
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        return new HttpClientHandler()
                        {
                            AllowAutoRedirect = false
                        };
                    });

            return services;
        }
    }
}
