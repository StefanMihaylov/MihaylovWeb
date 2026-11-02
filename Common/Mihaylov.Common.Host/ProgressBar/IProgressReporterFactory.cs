using System;
using Microsoft.AspNetCore.SignalR;

namespace Mihaylov.Common;

/// <summary>
/// Creates <see cref="IProgress{T}"/> reporters that push progress updates to a single SignalR client.
/// </summary>
/// <typeparam name="THub">The hub whose <see cref="IHubContext{THub}"/> is used to reach the client.</typeparam>
/// <typeparam name="TModel">The progress model that is sent to the client.</typeparam>
public interface IProgressReporterFactory<THub, TModel> where THub : Hub where TModel : class
{
    /// <summary>
    /// Creates a reporter that sends every reported <typeparamref name="TModel"/> to the client with the given connection.
    /// </summary>
    /// <param name="connectionId">
    /// The SignalR connection id of the client that should receive the updates.
    /// When it is <see langword="null"/>, the returned reporter does nothing.
    /// </param>
    /// <param name="jsFunctionName">The name of the client-side function that is invoked for each update.</param>
    /// <returns>A reporter that can be passed to long-running work to report its progress.</returns>
    IProgress<TModel> GetLoadingBarReporter(string connectionId, string jsFunctionName);
}
