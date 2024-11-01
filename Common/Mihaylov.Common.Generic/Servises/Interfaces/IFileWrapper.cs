using System.Collections.Generic;
using System.IO;
using Mihaylov.Common.Generic.Servises.Models;

namespace Mihaylov.Common.Generic.Servises.Interfaces
{
    public interface IFileWrapper
    {
        byte[] ReadFile(string path);

        Stream GetStreamFile(string path);

        void SaveStreamFile(Stream stream, string path);

        void SaveFile(string path, string content);

        IEnumerable<FileInfoModel> GetAllFiles(string directoryPath, bool includeSubdirectories, string basePath = null);

        FileInfoModel GetFileInfo(string path);

        void DeleteFile(string filePath);

        IEnumerable<DirInfoModel> GetDirectories(string directoryPath);

        string CreateDirectory(string basePath, string name);

        void MoveFiles(string dir, IEnumerable<string> files);
    }
}
