using System;

namespace Mihaylov.Api.Site.Database.Models
{
    public class MediaFile
    {
        public long MediaFileId { get; set; }

        public long? AccountId { get; set; }

        public Account Account { get; set; }

        public int SourceId { get; set; }

        public MediaFileSource Source { get; set; }


        public string FileName { get; set; }

        public int ExtensionId { get; set; }

        public MediaFileExtension Extension { get; set; }

        public long SizeInBytes { get; set; }

        public DateTime CreateDate { get; set; }

        public string CheckSum { get; set; }

        public int? GroupId { get; set; }
    }
}
