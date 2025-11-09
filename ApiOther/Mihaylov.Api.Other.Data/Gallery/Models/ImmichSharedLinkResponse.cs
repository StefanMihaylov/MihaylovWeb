using System;

namespace Mihaylov.Api.Other.Data.Gallery.Models
{
    public class ImmichSharedLinkResponse
    {
        public string Key { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Type { get; set; }

        public ImmichSharedLinkAlbum Album { get; set; }
    }

    public class ImmichSharedLinkAlbum
    {
        public string Id { get; set; }
    }
}
