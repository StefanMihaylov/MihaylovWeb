using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Configs;

namespace Mihaylov.Api.Other.Data.Cluster;

public sealed class PlaywrightManager : IPlaywrightManager, IAsyncDisposable
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    private readonly PlaywrightSettings _config;
    private readonly SemaphoreSlim _semaphoreLock;

    public PlaywrightManager(IOptions<PlaywrightSettings> settings)
    {
        _config = settings.Value;
        _semaphoreLock = new(1, 1);
    }

    public async Task<IBrowserContext> NewContextAsync(BrowserNewContextOptions? contextOptions = null)
    {
        if (contextOptions == null)
        {
            contextOptions = new BrowserNewContextOptions()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                ViewportSize = new ViewportSize { Width = 1280, Height = 720 }
            };
        }

        var browser = await GetBrowserAsync().ConfigureAwait(false);
        var context = await browser.NewContextAsync(contextOptions).ConfigureAwait(false);

        return context;
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync().ConfigureAwait(false);
        }

        _playwright?.Dispose();
    }


    private async Task<IBrowser> GetBrowserAsync(CancellationToken ct = default)
    {
        if (_browser?.IsConnected == true)
        {
            return _browser;
        }

        await _semaphoreLock.WaitAsync(ct).ConfigureAwait(false);

        try
        {
            // re-check after acquiring the lock in case another caller just connected
            if (_browser?.IsConnected == true)
            {
                return _browser;
            }

            _playwright ??= await Playwright.CreateAsync().ConfigureAwait(false);
            _browser = await _playwright.Chromium.ConnectAsync(_config.ServerUrl).ConfigureAwait(false);

            //var options = new BrowserTypeLaunchOptions
            //{
            //    Headless = true,
            //    Args = new[]
            //    {
            //        "--no-sandbox",
            //        "--disable-setuid-sandbox",
            //        "--disable-dev-shm-usage",
            //        "--disable-gpu"
            //    }
            //};

            // _browser = await _playwright.Chromium.LaunchAsync(options).ConfigureAwait(false);

            return _browser;
        }
        finally
        {
            _semaphoreLock.Release();
        }
    }
}
