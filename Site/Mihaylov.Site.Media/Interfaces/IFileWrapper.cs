using System.Collections.Generic;
using System.IO;
using Mihaylov.Site.Media.Models;

namespace Mihaylov.Site.Media.Interfaces
{
    public interface IFileWrapper
    {
        string GetBasePath();

        byte[] ReadFile(string path);

        Stream GetStreamFile(string path);

        void SaveFile(string path, object content);

        IEnumerable<FileInfoModel> GetAllFiles(string directoryPath, bool includeSubdirectories, string basePath = null);

        FileInfoModel GetFileInfo(string path);

        void DeleteFile(string filePath);
    }
}
