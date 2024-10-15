using System.Collections.Generic;

namespace Mihaylov.Web.Models.Site
{
    public class MergeModel
    {
        public long From { get; set; }

        public long To { get; set; }

        public IEnumerable<int> Checks { get; set; }
    }
}
