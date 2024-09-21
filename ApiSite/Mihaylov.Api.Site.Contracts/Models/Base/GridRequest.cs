namespace Mihaylov.Api.Site.Contracts.Models.Base
{
    public class GridRequest
    {
        public int? AccountTypeId { get; set; }

        public int? AccountStatusId { get; set; }

        public string Name { get; set; }

        public string AccountName { get; set; }

        public int? Page { get; set; }

        public int? PageSize { get; set; }
    }
}
