using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides file and directory operations using System.IO, including reading files as bytes, obtaining read
    /// streams, saving text or streams (overwriting existing files), deleting and moving files, enumerating files and
    /// directories, and retrieving file metadata.
    /// </summary>
    public class FileWrapper : IFileWrapper
    {
        /// <inheritdoc />
        public byte[] ReadFile(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return bytes;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void SaveFile(string path, string content)
        {
            DeleteFile(path);
            File.WriteAllText(path, content);
        }

        /// <inheritdoc />
        public void SaveStreamFile(Stream stream, string path)
        {
            DeleteFile(path);
            
            using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            stream.Seek(0, SeekOrigin.Begin);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public string CreateDirectory(string basePath, string name)
        {
            var fullPath = Path.Combine(basePath, name);
            var info = Directory.CreateDirectory(fullPath);
            return info.FullName;
        }

        /// <inheritdoc />
        public void MoveFiles(string dir, IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var fileName = new FileInfo(file).Name;
                var destination = Path.Combine(dir, fileName);
                File.Move(file, destination);
            }
        }

        /// <inheritdoc />
        public FileInfoModel GetFileInfo(string path)
        {
            var fileInfo = new FileInfo(path);
            return GetFileInfoModel(fileInfo, null);
        }

        /// <inheritdoc />
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
