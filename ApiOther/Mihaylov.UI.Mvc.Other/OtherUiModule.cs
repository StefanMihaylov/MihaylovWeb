using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Other.Areas.Cluster.Controllers;
using Mihaylov.UI.Mvc.Other.Areas.Concert.Controllers;
using Mihaylov.UI.Mvc.Other.Areas.Gallery.Controllers;
using Mihaylov.UI.Mvc.Other.Areas.Weather.Controllers;

namespace Mihaylov.UI.Mvc.Other;

public class OtherUiModule : BaseUiModule, IUiModule
{
    public OtherUiModule() : base("Other")
    {
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddOtherApiClient(builder => builder.BindSettings(Name));
    }

    public override async Task<ComponentModuleInfo> GetModuleInfoAsync(IServiceScopeFactory factory)
    {
        return await this.GetModelInfoAsync<IOtherApiClient>(factory).ConfigureAwait(false);
    }

    public override IEnumerable<MenuItem> Menus =>
    [
        new MenuItem(10, "Галерия", new MenuRoute<GalleryController>(c => c.Index()), AccessLevel.Public, "fa-camera"), // fa-camera bi-card-image
        new MenuItem(20, "Време", new MenuRoute<WeatherController>(c => c.Index()), AccessLevel.Public, "bi-thermometer-half"), // bi-sun
        new MenuItem(100, "Други", null, AccessLevel.Admin, "bi-list-check",
        [
            new MenuItem(10, "Концерти", new MenuRoute<ConcertController>(c => c.Index(null)),AccessLevel.Admin, "bi-music-note-beamed"),
            new MenuItem(20, "Клъстър", new MenuRoute<ClusterController>(c => c.Index(null, null)), AccessLevel.Admin, "bi-cpu-fill", null, true),
            new MenuItem(30, "Nexus", new MenuRoute<NexusController>(c => c.Index()), AccessLevel.Admin, "bi-floppy"),
            new MenuItem(40, "Velero", new MenuRoute<VeleroController>(c => c.Index()), AccessLevel.Admin, "bi-file-earmark-zip"),
            new MenuItem(50, "Velero Snapshots", new MenuRoute<VeleroController>(c => c.SnapshotIndex()), AccessLevel.Admin, "bi-file-earmark-zip"),
        ]),
    ];
}
