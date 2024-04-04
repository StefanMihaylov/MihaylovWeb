using System;
using System.Collections.Generic;

namespace Mihaylov.Site.Media.Models
{
    public class SortResponse
    {
        public int FileCount { get; set; }

        public DateTime LastProcessed { get; set; }

        public IEnumerable<MediaInfoModel> Files { get; set; }

        public SortResponse()
        {
            Files = new List<MediaInfoModel>();
        }
    }
}
