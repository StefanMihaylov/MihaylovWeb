using Mihaylov.Site.Media.Models;
using System;
using System.Collections.Generic;

namespace Mihaylov.Site.Media.Interfaces
{
    public interface IMediaService
    {
        IEnumerable<MediaInfoModel> GetAllFiles(string directoryPath, bool includeSubdirectories, bool readOnly,
            bool calculateChecksum, Action<ScanProgressModel> progress);

        byte[] GetThumbnail(string filePath, int size);

        DuplicateResponse GetDuplicates(IEnumerable<MediaInfoModel> files);

        SortResponse GetSorted(IEnumerable<MediaInfoModel> files);

        T GetReportData<T>(string fileName, string basePath) where T : class;
    }
}