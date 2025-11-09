using System;

namespace Mihaylov.Api.Other.Data.Gallery.Models
{
    public class ImmichSharedLinkModel
    {
        public string Key { get; set; }

        public string Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AlbumId { get; set; }
    }
}
