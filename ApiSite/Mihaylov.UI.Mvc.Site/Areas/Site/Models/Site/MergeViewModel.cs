using Mihaylov.Api.Site.Client;

namespace Mihaylov.UI.Mvc.Site.Areas.Site.Models.Site
{
    public class MergeViewModel
    {
        public Person PersonFrom { get; set; }

        public Person PersonTo { get; set; }

        public long From => PersonFrom.Id;

        public long To => PersonTo.Id;
    }
}
