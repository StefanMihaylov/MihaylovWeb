using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Dictionary.Contracts.Repositories;
using Mihaylov.Api.Dictionary.DAL;
using Mihaylov.Api.Dictionary.DAL.Repositories;

namespace Mihaylov.Api
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddDictionaryRepositories(this IServiceCollection services)
        {
            services.RegisterDbMapping();

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IRecordRepository, RecordRepository>();

            return services;
        }
    }
}
