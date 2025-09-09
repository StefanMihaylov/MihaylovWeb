using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class NewPersonViewModel
    {
        public int? AccountTypeId { get; set; }

        public string AccountName { get; set; }

        public bool? IsPreview { get; set; }

        public IEnumerable<AccountType> AccountTypes { get; set; }
    }
}
