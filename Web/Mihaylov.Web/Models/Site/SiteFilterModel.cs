using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class SiteFilterModel : IGridRequest
    {
        public int? Page { get; set; }        

        public string Name { get; set; }

        public string AccountName { get; set; }

        public int? AccountTypeId { get; set; }

        public int? StatusId { get; set; }


        public long? PersonId { get; set; }

        public bool? IsNewPerson { get; set; }


        public IEnumerable<AccountStatus> AccountStates { get; set; }

        public IEnumerable<AccountType> AccountTypes { get; set; }

        public string AccountNameExact { get; set; }
    }
}
