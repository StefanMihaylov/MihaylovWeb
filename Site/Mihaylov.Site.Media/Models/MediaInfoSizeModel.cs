using System;

namespace Mihaylov.Site.Media.Models
{
    public class MediaInfoSizeModel
    {
        public TimeSpan? Lenght { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string Checksum { get; set; }
    }
}
