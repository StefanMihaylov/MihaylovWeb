using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Mihaylov.Common;

/// <summary>
/// Default <see cref="IProgressReporterFactory{THub, TModel}"/> that sends progress updates to one SignalR client through <see cref="IHubContext{THub}"/>.
/// Delivery is best effort: a failed send is ignored, so a client that disconnected does not break the work being reported,
/// and the order of updates is not guaranteed because <see cref="Progress{T}"/> raises its callback on the thread pool when there is no synchronization context.
/// </summary>
/// <typeparam name="THub">The hub whose <see cref="IHubContext{THub}"/> is used to reach the client.</typeparam>
/// <typeparam name="TModel">The progress model that is sent to the client.</typeparam>
/// <param name="progressHubContext">The hub context that provides access to the connected clients.</param>
public class ProgressReporterFactory<THub, TModel>(IHubContext<THub> progressHubContext) : IProgressReporterFactory<THub, TModel> where THub : Hub where TModel : class
{
    /// <inheritdoc />
    public IProgress<TModel> GetLoadingBarReporter(string connectionId, string jsFunctionName)
    {
        if (connectionId == null)
        {
            return new Progress<TModel>();
        }

        var progress = new Progress<TModel>(async current => await GetProgressHandlerAsync(current, connectionId, jsFunctionName));

        return progress;
    }

    /// <summary>
    /// Sends one progress update to the client and swallows any failure.
    /// </summary>
    private async Task GetProgressHandlerAsync(TModel current, string connectionId, string jsFunctionName)
    {
        try
        {
            var client = progressHubContext.Clients.Client(connectionId);
            await client.SendAsync(jsFunctionName, current);
        }
        catch (Exception)
        {
            // Progress updates are best effort: a client that disconnected must not break the work being reported.
        }
    }
}
