using System;

namespace Mihaylov.Site.Database.Models
{
    public class Media
    {
        public Guid Id { get; set; }

        public Guid SourceAccountId { get; set; }

        public byte[] Data { get; set; }

        public string Extension { get; set; }

        public long SizeInBytes { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
