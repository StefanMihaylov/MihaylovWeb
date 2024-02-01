using System;

namespace Mihaylov.Site.Media.Models
{
    public class MediaInfoModel : MediaInfoSizeModel
    {
        public string FullPath { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string SubDirectories { get; set; }

        public bool IsImage { get; set; }

        public long Size { get; set; }

        public DateTime DownloadedOn { get; set; }

        public bool Readonly { get; set; }
    }
}
