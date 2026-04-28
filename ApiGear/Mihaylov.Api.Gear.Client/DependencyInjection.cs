using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Api.Gear.Client;
public static class DependencyInjection
{
    public static IServiceCollection AddGearApiClient(this IServiceCollection services, string url)
    {
        services.AddHttpClient(GearApiClient.GEAR_API_CLIENT_NAME, c =>
        {
            c.BaseAddress = new Uri(url);
            c.Timeout = TimeSpan.FromSeconds(60);
        });

        services.AddScoped<IGearApiClient>(provider =>
        {
            var dependency = provider.GetRequiredService<IHttpClientFactory>();

            return new GearApiClient(dependency);
        });

        return services;
    }
}
