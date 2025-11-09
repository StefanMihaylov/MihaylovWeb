using System;

namespace Mihaylov.Api.Other.Data.Gallery.Models
{
    public class ImmichAlbum
    {
        public string Id { get; set; }

        public string AlbumName { get; set; }

        public string Description { get; set; }

        public string AlbumThumbnailAssetId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int AssetCount { get; set; }
    }
}
