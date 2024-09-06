using System.Collections.Generic;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.Web.Models.Cluster
{
    public class ClusterMainModel
    {
        public IEnumerable<ApplicationViewModel> Applications {  get; set; } 

        public AddApplicationModel Input { get; set; }

        public AddAdditionalModel Additional { get; set; }

        public AddVersionModel Version { get; set; }

        public IEnumerable<ParserSetting> ParserSettings { get; set; }

        public AddParserSettingModel ParserSetting { get; set; }
    }
}
