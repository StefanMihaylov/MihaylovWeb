using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class DefaultFilterExtended : DefaultFilter
    {
        public IEnumerable<AccountType> AccountTypes { get; set; }

        public IEnumerable<AccountStatus> States { get; set; }

        public DefaultFilterExtended(DefaultFilter input)
        {
            Id = input?.Id ?? 0;
            IsEnabled = input?.IsEnabled ?? false;
            AccountTypeId = input?.AccountTypeId;
            AccountType = input?.AccountType;
            StatusId = input?.StatusId;
            Status = input?.Status;
            IsArchive = input?.IsArchive ?? false;
            IsPreview = input?.IsPreview;
        }
    }
}
