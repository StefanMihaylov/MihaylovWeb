using System;
using Mihaylov.Api.Site.Contracts.Helpers.Models;

namespace Mihaylov.Api.Site.Data.Media.Interfaces
{
    public interface IBaseMediaService
    {
        MediaInfoSizeModel GetSize(string filePath, bool calculateChecksum, Action<int> progress);

        byte[] GetThumbnail(string filePath, SizeModel inputSize, int thumbnailWidth);
    }
}
