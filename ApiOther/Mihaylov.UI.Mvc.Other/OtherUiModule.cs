using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Other.Areas.Cluster.Controllers;
using Mihaylov.UI.Mvc.Other.Areas.Concert.Controllers;
using Mihaylov.UI.Mvc.Other.Areas.Gallery.Controllers;
using Mihaylov.UI.Mvc.Other.Areas.Weather.Controllers;

namespace Mihaylov.UI.Mvc.Other
{
    public class OtherUiModule : IUiModule
    {
        public string Name => "Other";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOtherApiClient(builder => builder.BindSettings(Name));
        }

        public IEnumerable<MenuItem> Menus =>
        [
            new MenuItem(10, "Галерия", new MenuRoute<GalleryController>(c => c.Index()), null, "fa-camera"), // fa-camera bi-card-image
            new MenuItem(20, "Време", new MenuRoute<WeatherController>(c => c.Index()), null, "bi-thermometer-half"), // bi-sun
            new MenuItem(100, "Други", null, "Admim", "bi-list-check",
            [
                new MenuItem(10, "Концерти", new MenuRoute<ConcertController>(c => c.Index(null, null)),"Admim", "bi-music-note-beamed"),
                new MenuItem(20, "Клъстър", new MenuRoute<ClusterController>(c => c.Index(null, null)), "Admim", "bi-cpu-fill", null, true),
                new MenuItem(30, "Nexus", new MenuRoute<NexusController>(c => c.Index()), "Admim", "bi-floppy"),
                new MenuItem(40, "Velero", new MenuRoute<VeleroController>(c => c.Index()), "Admim", "bi-file-earmark-zip"),
                new MenuItem(50, "Velero Snapshots", new MenuRoute<VeleroController>(c => c.SnapshotIndex()), "Admim", "bi-file-earmark-zip"),
            ]),
        ];
    }
}
