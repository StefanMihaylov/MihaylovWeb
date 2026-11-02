using Mihaylov.Common;

namespace Mihaylov.Api.Site.Contracts.Models.Base
{
    public class GridRequest : BaseGridRequest
    {
        public int? AccountTypeId { get; set; }

        public int? StatusId { get; set; }

        public string Name { get; set; }

        public string AccountName { get; set; }

        public string AccountNameExact { get; set; }
    }
}