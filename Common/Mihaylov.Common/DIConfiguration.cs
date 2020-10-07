using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Common.Databases.Interfaces;
using Mihaylov.Common.Databases.Services;
using Mihaylov.Common.Infrastructure.Interfaces;
using Mihaylov.Common.Infrastructure.Services;

namespace Mihaylov.Common
{
    public static class DIConfiguration
    {
        public static IServiceCollection AddDICommon(this IServiceCollection services)
        {
            services
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<IAuditService, AuditService>();

            return services;
        }
    }
}
