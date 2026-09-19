using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface ISemaphoreProvider
    {
        ValueTask<IAsyncDisposable> AcquireAsync(string key, int? maxConcurrentRequests, CancellationToken cancellationToken = default);
    }
}