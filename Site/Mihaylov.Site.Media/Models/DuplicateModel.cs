using System;
using System.Collections.Generic;
using System.Text;

namespace Mihaylov.Site.Media.Models
{
    public class DuplicateModel
    {
        public string Checksum {  get; set; }
        
        public int Count { get; set; }

        public IEnumerable<MediaInfoModel> List { get; set; }

    }
}
