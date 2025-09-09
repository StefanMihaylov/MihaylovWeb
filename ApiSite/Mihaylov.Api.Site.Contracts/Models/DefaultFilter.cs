namespace Mihaylov.Api.Site.Contracts.Models
{
    public class DefaultFilter
    {
        public int Id { get; set; }

        public bool IsEnabled { get; set; }

        public int? AccountTypeId { get; set; }

        public string AccountType { get; set; }

        public int? StatusId { get; set; }

        public string Status { get; set; }

        public bool IsArchive { get; set; }

        public bool? IsPreview { get; set; }
    }
}
