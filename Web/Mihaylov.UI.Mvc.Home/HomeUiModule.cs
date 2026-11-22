using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Home.Controllers;
using Mihaylov.UI.Mvc.Home.Hubs;
using Mihaylov.UI.Mvc.Home.Models.Configs;

namespace Mihaylov.UI.Mvc.Home;

public class HomeUiModule : BaseUiModule, IUiModule
{
    public HomeUiModule() : base("Home", null)
    {
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddMediaServices(ffMpeg =>
        {
            // https://ffmpeg.org/download.html, https://github.com/jrottenberg/ffmpeg/tree/main, https://hub.docker.com/r/jrottenberg/ffmpeg
            ffMpeg.FFMpegPath = Config.GetEnvironmentVariable("Media_FFMpegPath", string.Empty);
        });

        services.Configure<MediaConfig>(opt =>
        {
            opt.DuplicatePathLocked = Config.GetEnvironmentVariable("Media_Dub_Path_Lock", string.Empty);
            opt.DuplicatePathNotLocked = Config.GetEnvironmentVariable("Media_Dub_Path_NotLock", string.Empty);
            opt.SoftPath = Config.GetEnvironmentVariable("Media_Sort", string.Empty);
            opt.RenamePath = Config.GetEnvironmentVariable("Media_Rename", string.Empty);
        });

        services.AddSignalR();
        services.AddProgressBar<ScanProgressHub, ScanProgressModel>();
    }

    public override Task<ComponentModuleInfo> GetModuleInfoAsync(IServiceScopeFactory factory)
    {
        using var scope = factory.CreateScope();
        var provider = scope.ServiceProvider;

        var service = provider.GetRequiredService<IModuleAssemblyService>();
        IModuleInfo data = service.GetModuleInfo();

        var uiInfo = VersionHelper.GetVersionInfo<HomeUiModule>();
        var orders = Menus.Select(m => m.Order).ToList();

        return Task.FromResult(new ComponentModuleInfo(data, uiInfo, orders));
    }

    public override Task<ComponentDependencies> GetDependenciesAsync(IServiceScopeFactory factory)
    {
        using var scope = factory.CreateScope();
        var provider = scope.ServiceProvider;

        var service = provider.GetRequiredService<IModuleAssemblyService>();
        DependencyNode serverData = service.GetDependencies();

        var uiInfo = DependencyScanner.Scan(typeof(HomeUiModule).Assembly);

        return Task.FromResult(new ComponentDependencies(serverData, uiInfo));
    }

    public override IEnumerable<MenuItem> Menus =>
        [
            new MenuItem(1, "", new MenuRoute<HomeController>(c => c.Index()), AccessLevel.Public, "fa-home", null, false, true), // "bi-house-door-fill"
            new MenuItem(2, "", new MenuRoute<HomeController>(c => c.ModuleInfo()), AccessLevel.Admin, "bi-gear-fill"),
            new MenuItem(3, "", new MenuRoute<HomeController>(c => c.Dependencies()), AccessLevel.Admin, "bi-diagram-3-fill"),
            new MenuItem(30, "Медия", null, AccessLevel.Admin, null,
                [
                    new MenuItem(20, "Сортиране", new MenuRoute<MediaController>(c => c.Sort(null)), AccessLevel.Admin, null),
                    new MenuItem(10, "Дубликати", new MenuRoute<MediaController>(c => c.Duplicates()), AccessLevel.Admin, null),
                    new MenuItem(30, "Преименуване", new MenuRoute<MediaController>(c => c.RenamePics()), AccessLevel.Admin, null),
                ]),
        ];
}
