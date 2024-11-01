using Mihaylov.Api.Site.Contracts.Helpers.Models;
using System;
using System.Collections.Generic;

namespace Mihaylov.Api.Site.Contracts.Helpers
{
    public interface IMediaService
    {
        IEnumerable<MediaInfoModel> GetAllFiles(string directoryPath, bool includeSubdirectories, bool readOnly,
            bool calculateChecksum, Action<ScanProgressModel> progress);

        byte[] GetThumbnail(string filePath, int size);

        DuplicateResponse GetDuplicates(IEnumerable<MediaInfoModel> files);

        SortResponse GetSorted(IEnumerable<MediaInfoModel> files);

        T GetReportData<T>(string fileName, string basePath) where T : class;

        string GetBasePath();
    }
}