using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class SiteMainModel
    {
        public SiteFilterModel Filter { get; set; }

        public NewPersonViewModel NewPersonFilter { get; set; }

        public PersonGrid Grid { get; set; }

        public PersonExtended Person { get; set; }

        public PersonFormatedStatistics Statistics { get; set; }

        public SiteAdminModel AdminData { get; set; }

        public OtherTabModel OtherTabModel { get; set; }
    }
}
