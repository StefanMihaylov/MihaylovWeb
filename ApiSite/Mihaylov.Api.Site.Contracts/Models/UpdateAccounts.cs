using System.Collections.Generic;

namespace Mihaylov.Api.Site.Contracts.Models
{
    public class UpdateAccounts
    {
        public IEnumerable<Account> Accounts { get; set; }

        public int TotalCount { get; set; }

        public int? BatchSize { get; set; }
    }
}
