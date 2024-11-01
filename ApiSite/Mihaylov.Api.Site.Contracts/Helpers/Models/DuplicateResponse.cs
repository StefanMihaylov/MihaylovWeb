using System.Collections.Generic;

namespace Mihaylov.Api.Site.Contracts.Helpers.Models
{
    public class DuplicateResponse
    {
        public int FileCount {  get; set; }

        public int DuplicateCount { get; set; }

        public IEnumerable<DuplicateModel> Duplicates { get; set; }


        public DuplicateResponse()
        {
            Duplicates = new List<DuplicateModel>();
        }
    }
}
