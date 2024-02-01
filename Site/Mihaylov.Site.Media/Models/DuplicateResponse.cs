using System.Collections.Generic;

namespace Mihaylov.Site.Media.Models
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
