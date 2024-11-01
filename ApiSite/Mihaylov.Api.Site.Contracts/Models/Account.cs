using System;
using Mihaylov.Common;

namespace Mihaylov.Api.Site.Contracts.Models
{
    public class Account
    {
        public const int NameMaxLength = 50;
        public const int DisplayNameMaxLength = 50;
        public const int DetailsMaxLength = 1000;

        public long Id { get; set; }

        public long? PersonId { get; set; }

        public int AccountTypeId { get; set; }

        public string AccountType { get; set; }

        public int? StatusId { get; set; }

        public string Status { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public DateTime AskDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? Age => CreateDate.GetDays();

        public DateTime? LastOnlineDate { get; set; }

        public DateTime? ReconciledDate { get; set; }

        public string Details { get; set; }
    }
}
