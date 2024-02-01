using Mihaylov.Site.Media.Models;
using System;
using System.Collections.Generic;

namespace Mihaylov.Site.Media.Interfaces
{
    public interface IMediaService
    {
        IEnumerable<MediaInfoModel> GetAllFiles(string directoryPath, bool includeSubdirectories, bool readOnly, Action<ScanProgressModel> progress);

        byte[] GetThumbnail(string filePath, int size);

        DuplicateResponse GetDuplicates(IEnumerable<MediaInfoModel> files);

        DuplicateResponse GetDuplicates(string fileName, string basePath);
    }
}