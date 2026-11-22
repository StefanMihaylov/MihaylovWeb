using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Gear.Client;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Gear.Areas.Gear.Controllers;

namespace Mihaylov.UI.Mvc.Gear;

public class GearUiModule : BaseUiModule, IUiModule
{
    public GearUiModule() : base("Gear", typeof(IGearApiClient))
    {
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddGearApiClient(Name);
    }

    public override IEnumerable<MenuItem> Menus =>
        [
            new MenuItem(50, "Екипировка", null, AccessLevel.Admin, null,
            [
                new MenuItem(10, "Пътешествия", new MenuRoute<GearController>(c => c.Index()),AccessLevel.Admin, null),
                new MenuItem(20, "Екипировка", new MenuRoute<GearController>(c => c.Inventory(null)),AccessLevel.Admin, null, null, true),
                new MenuItem(30, "Списъци", new MenuRoute<GearController>(c => c.Lists()),AccessLevel.Admin, null),
            ]),
        ];
}
