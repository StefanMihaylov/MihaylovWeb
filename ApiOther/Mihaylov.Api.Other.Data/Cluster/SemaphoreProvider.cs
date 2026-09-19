using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;

namespace Mihaylov.Api.Other.Data.Cluster;

public sealed class SemaphoreProvider(ILogger<SemaphoreProvider> logger) : ISemaphoreProvider
{
    private record Entry(int MaxCount, SemaphoreSlim Semaphore);

    private readonly ConcurrentDictionary<string, Entry> _endties = new();

    public async ValueTask<IAsyncDisposable> AcquireAsync(string key, int? maxConcurrentRequests, CancellationToken cancellationToken = default)
    {
        if (maxConcurrentRequests is null || maxConcurrentRequests <= 0)
        {
            return new NullGuard();
        }

        var maxConcurrent = maxConcurrentRequests.Value;
        var entry = _endties.GetOrAdd(key, _ => new Entry(maxConcurrent, new SemaphoreSlim(maxConcurrent, maxConcurrent)));

        if (entry.MaxCount != maxConcurrent)
        {
            var replasement = new Entry(maxConcurrent, new SemaphoreSlim(maxConcurrent, maxConcurrent));
            _endties.TryUpdate(key, replasement, entry);
            entry = _endties[key];
        }

        if (entry.Semaphore.CurrentCount == 0)
        {
            logger.LogWarning("All {Limit} concurrent slots occupied for '{Key}' - waiting...", maxConcurrent, key);
        }

        await entry.Semaphore.WaitAsync(cancellationToken);

        return new SemaphoreReleaser(entry.Semaphore);
    }

    private sealed class SemaphoreReleaser(SemaphoreSlim semaphore) : IAsyncDisposable
    {
        public async ValueTask DisposeAsync()
        {
            semaphore.Release();
            await Task.CompletedTask;
        }
    }

    private sealed class NullGuard : IAsyncDisposable
    {
        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
