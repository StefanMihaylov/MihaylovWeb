using System;

namespace Mihaylov.Api.Other.Contracts.Gallery.Models
{
    public class AlbumModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ThumbnailAssetId { get; set; }

        public string AlbumLink { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int AssetCount { get; set; }
    }
}
