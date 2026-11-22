using System.Collections.Generic;

namespace Mihaylov.UI.Mvc.Site.Areas.Site.Models.Site
{
    public class MergeModel
    {
        public long From { get; set; }

        public long To { get; set; }

        public IEnumerable<int> Checks { get; set; }
    }
}
