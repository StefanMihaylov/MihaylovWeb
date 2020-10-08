using System;

namespace Mihaylov.Site.Database.Models
{
    public class MediaFile
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public virtual Account Account { get; set; }

        public int SourceId { get; set; }

        public virtual AccountType Source { get; set; }

        public byte[] Data { get; set; }

        public string Extension { get; set; }

        public long SizeInBytes { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
