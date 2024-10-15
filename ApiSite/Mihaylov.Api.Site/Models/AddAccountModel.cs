using System;
using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Api.Site.Models
{
    public class AddAccountModel
    {
        public long? Id { get; set; }

        public long? PersonId { get; set; }

        [Required]
        public int? AccountTypeId { get; set; }

        public int? StatusId { get; set; }

        [Required]
        public string Username { get; set; }

        public string DisplayName { get; set; }

        public DateTime AskDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? LastOnlineDate { get; set; }

        public DateTime? ReconciledDate { get; set; }

        public string Details { get; set; }
    }
}
