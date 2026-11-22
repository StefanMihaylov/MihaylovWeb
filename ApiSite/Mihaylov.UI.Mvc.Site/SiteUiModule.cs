using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Mihaylov.Api.Site.Client;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Site.Areas.Site.Controllers;

namespace Mihaylov.UI.Mvc.Site
{
    public class SiteUiModule : BaseUiModule, IUiModule
    {
        public SiteUiModule() : base("Site", typeof(ISiteApiClient))
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
           services.AddSiteApiClient(Name);
        }

        public override IEnumerable<MenuItem> Menus =>
            [
             new MenuItem(40, "Снимки", null, AccessLevel.Admin, null,
                 [
                 new MenuItem(10, "Сайт", new MenuRoute<SiteController>(c => c.Index(null)),AccessLevel.Admin, null)
                 ]),
            ];
    }
}
