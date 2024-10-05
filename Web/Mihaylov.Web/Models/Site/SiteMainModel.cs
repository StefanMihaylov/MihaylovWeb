using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class SiteMainModel
    {
        public PersonGrid Grid { get; set; }

        public PersonExtended Person { get; set; }

        public PersonStatistics Statistics { get; set; }

        public SiteAdminModel AdminData { get; set; }
    }
}
