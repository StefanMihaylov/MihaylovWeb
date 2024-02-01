using System.Collections.Generic;
using Mihaylov.Site.Media.Models;

namespace Mihaylov.WebUI.Models
{
    public class DuplicatesViewModel
    {
        public int FilesCount { get; set; }

        public int DuplicatesCount { get; set; }

        public IEnumerable<DuplicateModel> Duplicates { get; set; }
    }
}
