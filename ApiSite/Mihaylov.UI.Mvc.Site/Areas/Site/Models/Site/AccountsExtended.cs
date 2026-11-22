using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.UI.Mvc.Site.Areas.Site.Models.Site
{
    public class AccountsExtended
    {
        public long PersonId { get; }

        public IEnumerable<Account> Accounts { get; }

        public AccountsExtended(Person person)
        {
            PersonId = person.Id;
            Accounts = person.Accounts;
        }
    }
}
