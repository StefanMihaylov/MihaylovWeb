using System;
using Mihaylov.Site.Media.Models;

namespace Mihaylov.Site.Media.Interfaces
{
    public interface IBaseMediaService
    {
        MediaInfoSizeModel GetSize(string filePath, bool calculateChecksum, Action<int> progress);

        byte[] GetThumbnail(string filePath, int width, int height, int thumbnailWidth);
    }
}
