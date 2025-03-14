﻿using System;
using Mihaylov.Common.Database.Models;

namespace Mihaylov.Api.Site.Database.Models
{
    public class Account : Entity
    {
        public long AccountId { get; set; }

        public int AccountTypeId { get; set; }

        public AccountType AccountType { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }


        public long? PersonId { get; set; }

        public Person Person { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? LastOnlineDate { get; set; }

        public DateTime? ReconciledDate { get; set; }

        public string Details { get; set; }

        public int? StatusId {  get; set; }

        public AccountStatus Status { get; set; }
    }
}
