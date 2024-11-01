using System.Collections.Generic;
using Mihaylov.Api.Site.Contracts.Helpers.Models;

namespace Mihaylov.Web.Models
{
    public class DuplicatesViewModel
    {
        public int FilesCount { get; set; }

        public int DuplicatesCount { get; set; }

        public IEnumerable<DuplicateModel> Duplicates { get; set; }

        public string RedirectUrl { get; set; }
    }
}
