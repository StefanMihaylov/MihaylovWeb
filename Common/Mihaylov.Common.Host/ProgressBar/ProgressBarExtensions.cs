using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Mihaylov.Common;

/// <summary>
/// Service registration for the SignalR progress bar support.
/// </summary>
public static class ProgressBarExtensions
{
    /// <summary>
    /// Registers <see cref="IProgressReporterFactory{THub, TModel}"/> with its <see cref="ProgressReporterFactory{THub, TModel}"/> implementation as a scoped service.
    /// Call it once for each hub and model pair. SignalR itself must be registered separately (<c>AddSignalR</c>).
    /// </summary>
    /// <typeparam name="THub">The hub whose clients receive the progress updates.</typeparam>
    /// <typeparam name="TModel">The progress model that is sent to the clients.</typeparam>
    /// <param name="services">The service collection to add the factory to.</param>
    /// <returns>The same <paramref name="services"/> instance, so calls can be chained.</returns>
    public static IServiceCollection AddProgressBar<THub, TModel>(this IServiceCollection services) where THub : Hub where TModel : class
    {
        services.AddScoped<IProgressReporterFactory<THub, TModel>, ProgressReporterFactory<THub, TModel>>();

        return services;
    }
}
