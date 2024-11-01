using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Common.Generic.Servises.Models;
using System;
using System.Collections.Generic;

namespace Mihaylov.Web.Models
{
    public class SortViewModel
    {
        public int Page { get; set; }

        public int PageCount { get; set; }

        public int FilesCount { get; set; }

        public DateTime LastProcessed { get; set; }

        public IEnumerable<IEnumerable<MediaInfoModel>> Files { get; set; }

        public IEnumerable<DirInfoModel> Dirs { get; set; }

        public string RedirectUrl { get; set; }
    }
}
