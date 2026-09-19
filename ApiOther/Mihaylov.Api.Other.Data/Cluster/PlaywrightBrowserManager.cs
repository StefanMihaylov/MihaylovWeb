using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Mihaylov.Api.Other.Data.Cluster;

public sealed class PlaywrightBrowserManager : IAsyncDisposable
{
    private static readonly Lazy<Task<PlaywrightBrowserManager>> _instance =
        new(() => InitializeAsync(), isThreadSafe: true);

    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;

    private PlaywrightBrowserManager() { }

    public static Task<PlaywrightBrowserManager> InstanceAsync => _instance.Value;

    private static async Task<PlaywrightBrowserManager> InitializeAsync()
    {
        var manager = new PlaywrightBrowserManager();

        manager._playwright = await Playwright.CreateAsync().ConfigureAwait(false);

        var options = new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args = new[]
            {
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-dev-shm-usage",
                "--disable-gpu"
            }
        };

        manager._browser = await manager._playwright.Chromium.LaunchAsync(options).ConfigureAwait(false);

        return manager;
    }

    public async Task<IBrowserContext> NewContextAsync(BrowserNewContextOptions? contextOptions = null)
    {
        if(contextOptions == null)
        {
            contextOptions = new BrowserNewContextOptions()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                ViewportSize = new ViewportSize { Width = 1280, Height = 720 }
            };
        }

        return await _browser.NewContextAsync(contextOptions).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync().ConfigureAwait(false);
        }            

        _playwright?.Dispose();
    }
}
