using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class AccountExtended : Account
    {
        public AccountExtended(Account account)
        {
            Id = account.Id;
            PersonId = account.PersonId;
            AccountTypeId = account.AccountTypeId;
            AccountType = account.AccountType;
            Username = account.Username;
            DisplayName = account.DisplayName;
            StatusId = account.StatusId;
            Status = account.Status;
            AskDate = account.AskDate;
            CreateDate = account.CreateDate;
            Age = account.Age;
            LastOnlineDate = account.LastOnlineDate;
            ReconciledDate = account.ReconciledDate;          
            Details = account.Details;
        }

        public IEnumerable<AccountStatus> AccountStates { get; set; }

        public IEnumerable<AccountType> AccountTypes { get; set; }
    }
}
