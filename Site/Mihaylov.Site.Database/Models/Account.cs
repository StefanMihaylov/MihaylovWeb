using System;

namespace Mihaylov.Site.Database.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public int AccountTypeId { get; set; }

        public virtual AccountType AccountType { get; set; }

        public string Username { get; set; }

        public Guid PersonId { get; set; }

        public virtual Person Person { get; set; }


        public DateTime? CreateDate { get; set; }

        public DateTime LastOnlineDate { get; set; }

        public bool IsAccountDisabled { get; set; }
    }
}
