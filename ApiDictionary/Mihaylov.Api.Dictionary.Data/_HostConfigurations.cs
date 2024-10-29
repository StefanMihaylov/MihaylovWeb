using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Dictionary.Contracts.Managers;
using Mihaylov.Api.Dictionary.Contracts.Writers;
using Mihaylov.Api.Dictionary.Data.Managers;
using Mihaylov.Api.Dictionary.Data.Writers;

namespace Mihaylov.Api.Dictionary.Data
{
    public static class _HostConfigurations
    {
        public static IServiceCollection AddDictionaryServices(this IServiceCollection services)
        {
            services.AddScoped<ICourseManager, CourseManager>();
            services.AddScoped<IRecordManager, RecordManager>();

            services.AddScoped<IRecordWriter, RecordWriter>();

            return services;
        }
    }
}
