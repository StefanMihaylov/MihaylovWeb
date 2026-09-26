using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IPlaywrightManager
    {
        Task<IBrowserContext> NewContextAsync(BrowserNewContextOptions? contextOptions = null);
    }
}
