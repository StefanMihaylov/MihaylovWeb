using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class MergeViewModel
    {
        public Person PersonFrom { get; set; }

        public Person PersonTo { get; set; }

        public long From => PersonFrom.Id;

        public long To => PersonTo.Id;
    }
}
