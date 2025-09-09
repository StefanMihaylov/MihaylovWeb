namespace Mihaylov.Api.Site.Database.Models
{
    public class DefaultFilter
    {
        public int DefaultFilterId { get; set; }

        public bool IsEnabled { get; set; }

        public int? AccountTypeId { get; set; }

        public AccountType AccountType { get; set; }

        public int? StatusId { get; set; }

        public AccountStatus Status { get; set; }

        public bool IsArchive { get; set; }

        public bool? IsPreview { get; set; }
    }
}
