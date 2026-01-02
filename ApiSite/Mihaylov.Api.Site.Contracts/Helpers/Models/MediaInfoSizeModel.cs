using System;

namespace Mihaylov.Api.Site.Contracts.Helpers.Models
{
    public class MediaInfoSizeModel
    {
        public TimeSpan? Lenght { get; set; }

        public SizeModel Size { get; set; }

        public string Checksum { get; set; }

        public ulong? PerceptualHash { get; set; }
    }
}
