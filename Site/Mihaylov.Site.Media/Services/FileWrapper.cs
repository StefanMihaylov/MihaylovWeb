using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Mihaylov.Site.Media.Interfaces;
using Mihaylov.Site.Media.Models;

namespace Mihaylov.Site.Media.Services
{
    public class FileWrapper : IFileWrapper
    {
        private readonly FileWrapperConfig _config;

        public FileWrapper(IOptions<FileWrapperConfig> settings)
        {
            _config = settings.Value;
        }

        public string GetBasePath()
        {
            return _config.BasePath;
        }

        public byte[] ReadFile(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return bytes;
        }

        public Stream GetStreamFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var stream = File.OpenRead(path);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public void SaveFile(string path, object content)
        {
            DeleteFile(path);
            File.WriteAllText(path, JsonSerializer.Serialize(content));
        }

        public IEnumerable<FileInfoModel> GetAllFiles(string directoryPath, bool includeSubdirectories, string basePath = null)
        {
            if (basePath == null)
            {
                basePath = directoryPath;
            }

            var directoryInfo = new DirectoryInfo(directoryPath);
            var files = directoryInfo.GetFiles()
                                     .Select(f =>
                                     {
                                         var subPath = f.FullName.Replace(basePath, string.Empty).Replace(f.Name, string.Empty).TrimEnd('/').TrimEnd('\\');
                                         return GetFileInfoModel(f, subPath);
                                     })
                                     .ToList();

            if (includeSubdirectories)
            {
                var directories = directoryInfo.GetDirectories();
                if (directories.Any())
                {
                    foreach (DirectoryInfo subDirectory in directories)
                    {
                        var subFiles = GetAllFiles(subDirectory.FullName, includeSubdirectories, basePath);
                        files.AddRange(subFiles);
                    }
                }
            }

            return files;
        }

        public IEnumerable<DirInfoModel> GetDirectories(string directoryPath)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            var directories = directoryInfo.GetDirectories();

            var result = directories
                .OrderByDescending(f => f.CreationTime)
                .Select(d => new DirInfoModel()
                {
                    Name = d.Name,
                    FullPath = d.FullName,
                    FilesCount = d.GetFiles().Length,
                });

            return result;
        }

        public string CreateDierctory(string basePath, string name)
        {
            var fullPath = Path.Combine(basePath, name);
            var info = Directory.CreateDirectory(fullPath);
            return info.FullName;
        }

        public void MoveFiles(string dir, IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var fileName = new FileInfo(file).Name;
                var destination = Path.Combine(dir, fileName);
                File.Move(file, destination);
            }
        }

        public FileInfoModel GetFileInfo(string path)
        {
            var fileInfo = new FileInfo(path);
            return GetFileInfoModel(fileInfo, null);
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private FileInfoModel GetFileInfoModel(FileInfo input, string subDirectories)
        {
            return new FileInfoModel()
            {
                FullName = input.FullName,
                Name = input.Name,
                Extension = input.Extension,
                SubDirectories = subDirectories,
                Exists = input.Exists,
                Length = input.Exists ? input.Length : 0,
                LastWriteTime = input.LastWriteTime,
            };
        }
    }
}
