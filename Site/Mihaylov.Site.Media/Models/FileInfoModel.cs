using System;

namespace Mihaylov.Site.Media.Models
{
    public class FileInfoModel
    {
        public string FullName { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string SubDirectories { get; set; }

        public bool Exists { get; set; }

        public long Length { get; set; }

        public DateTime LastWriteTime { get; set; }
    }
}
