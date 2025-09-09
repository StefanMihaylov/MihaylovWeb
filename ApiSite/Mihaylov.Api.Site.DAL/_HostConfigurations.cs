using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.DAL;
using Mihaylov.Api.Site.DAL.Repositories;

namespace Mihaylov.Api
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddSiteRepositories(this IServiceCollection services)
        {
            services.RegisterDbMapping();

            services.AddScoped<ICollectionRepository, CollectionRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();

            return services;
        }
    }
}
