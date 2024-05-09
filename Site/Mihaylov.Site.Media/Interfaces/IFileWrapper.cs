using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Mihaylov.Site.Media.Models;

namespace Mihaylov.Site.Media.Interfaces
{
    public interface IFileWrapper
    {
        string GetBasePath();

        byte[] ReadFile(string path);

        Stream GetStreamFile(string path);

        void SaveStreamFile(Stream stream, string path);

        void SaveFile(string path, object content);

        IEnumerable<FileInfoModel> GetAllFiles(string directoryPath, bool includeSubdirectories, string basePath = null);

        FileInfoModel GetFileInfo(string path);

        void DeleteFile(string filePath);

        IEnumerable<DirInfoModel> GetDirectories(string directoryPath);

        string CreateDierctory(string basePath, string name);

        void MoveFiles(string dir, IEnumerable<string> files);
    }
}
