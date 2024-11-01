using System;

namespace Mihaylov.Api.Site.Contracts.Helpers.Models
{
    public class MediaInfoModel : MediaInfoSizeModel
    {
        public Guid FileId { get; set; }

        public string FullPath { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string SubDirectories { get; set; }

        public bool IsImage { get; set; }

        public long FileSize { get; set; }

        public DateTime DownloadedOn { get; set; }

        public bool Readonly { get; set; }
    }
}
