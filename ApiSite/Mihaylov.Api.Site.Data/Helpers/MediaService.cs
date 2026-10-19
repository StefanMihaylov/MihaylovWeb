using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Api.Site.Data.Media.Interfaces;
using Mihaylov.Common;

namespace Mihaylov.Api.Site.Data.Helpers
{
    public class MediaService : IMediaService
    {
        private readonly IFileWrapper _file;
        private readonly IImageMediaService _imageMediaService;
        private readonly IBaseMediaService _videoMediaService;
        private readonly IAlglibHelper _dublicateHelper;
        private readonly string _BasePath;

        public MediaService(IFileWrapper fileWrapper, IImageMediaService imageMediaService,
            IVideoMediaService videoMediaService, IAlglibHelper dublicateHelper)
        {
            _file = fileWrapper;
            _imageMediaService = imageMediaService;
            _videoMediaService = videoMediaService;
            _dublicateHelper = dublicateHelper;

            var assembly = Assembly.GetExecutingAssembly();
            var fileInfo = new FileInfo(assembly.Location);
            _BasePath = fileInfo.DirectoryName;
        }

        /// <summary>
        /// Json files location. Local setup ONLY.
        /// </summary>
        /// <returns>The configured base path.</returns>
        public string GetBasePath()
        {
            return _BasePath;
        }

        public IEnumerable<MediaInfoModel> GetAllFiles(string directoryPath, bool includeSubdirectories, bool isReadOnly,
            bool calculateChecksum, Action<ScanProgressModel> progress)
        {
            var fileInfos = _file.GetAllFiles(directoryPath, includeSubdirectories);
            var totalCount = fileInfos.Count();

            var result = new List<MediaInfoModel>();

            long count = 0;
            foreach (var fileInfo in fileInfos)
            {
                count++;
                var fileProcessed = count.GetPersentage(totalCount);

                if (fileInfo.Length == 0 && fileInfo.Extension == ".nomedia")
                {
                    _file.DeleteFile(fileInfo.FullName);
                    continue;
                }

                MediaInfoModel mediaInfo = GetMediaData(fileInfo, isReadOnly, calculateChecksum, p => progress(new ScanProgressModel(fileProcessed, p)));

                result.Add(mediaInfo);
            }

            return result;
        }

        public byte[] GetThumbnail(string filePath, int size)
        {
            var fileinfo = _file.GetFileInfo(filePath);
            if (!fileinfo.Exists)
            {
                return new byte[0];
            }

            var mediaInfo = GetMediaData(fileinfo, false, false, a => { });
            
            IBaseMediaService service = GetService(mediaInfo.IsImage);
            var bytes = service.GetThumbnail(filePath, mediaInfo.Size, size);

            return bytes;
        }

        public DuplicateResponse GetDuplicates(IEnumerable<MediaInfoModel> files)
        {
            var duplicates = GetDuplucatesChecksum(files);
            // _dublicateHelper.GetDuplucatesHash(duplicates, files, 6);

            var result = new DuplicateResponse()
            {
                FileCount = files.Count(),
                DuplicateCount = duplicates.Count(),
                Duplicates = duplicates,
            };

            return result;
        }

        public SortResponse GetSorted(IEnumerable<MediaInfoModel> files)
        {
            var sorted = files.OrderByDescending(f => f.DownloadedOn)
                                  .ToList();

            var result = new SortResponse()
            {
                LastProcessed = DateTime.Now,
                FileCount = files.Count(),
                Files = sorted
            };

            return result;
        }

        public T GetReportData<T>(string fileName, string basePath) where T : class
        {
            basePath = basePath ?? _BasePath;
            var path = Path.Combine(basePath, fileName);

            using var jsonStream = _file.GetStreamFile(path);
            if (jsonStream == null)
            {
                return Activator.CreateInstance(typeof(T)) as T;
            }

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            var model = JsonSerializer.Deserialize<T>(jsonStream, options);

            return model;
        }


        private MediaInfoModel GetMediaData(FileInfoModel fileInfo, bool readOnly, bool calculateChecksum, Action<int> progress)
        {
            bool isImage = _imageMediaService.IsImageFile(fileInfo.Extension);

            IBaseMediaService service = GetService(isImage);            
            var sizeInfo = service.GetSize(fileInfo.FullName, calculateChecksum, progress);

            var mediaInfo = new MediaInfoModel()
            {
                FileId = Guid.NewGuid(),
                FullPath = fileInfo.FullName,
                Name = fileInfo.Name,
                Extension = fileInfo.Extension,
                SubDirectories = fileInfo.SubDirectories,
                FileSize = fileInfo.Length,
                DownloadedOn = fileInfo.LastWriteTime,
                Readonly = readOnly,
                IsImage = isImage,

                Size = sizeInfo.Size,
                Lenght = sizeInfo.Lenght,
                Checksum = sizeInfo.Checksum,
                PerceptualHash = sizeInfo.PerceptualHash,
            };

            return mediaInfo;
        }

        private IBaseMediaService GetService(bool isImage)
        {
            if (isImage)
            {
                return _imageMediaService;
            }

            return _videoMediaService;
        }

        private List<DuplicateModel> GetDuplucatesChecksum(IEnumerable<MediaInfoModel> files)
        {
            var duplicates = files.Where(m => !string.IsNullOrEmpty(m.Checksum))
                                  .GroupBy(m => m.Checksum)
                                  .Select(g => new DuplicateModel()
                                  {
                                      Checksum = g.Key,
                                      Hash = g.Select(c => c.PerceptualHash).Distinct().SingleOrDefault(),
                                      Similarity = 100,
                                      Count = g.Count(),
                                      List = g.OrderBy(p => p.DownloadedOn)
                                  })
                                  .Where(m => m.Count > 1)
                                  .ToList();

            return duplicates;
        }
    }
}
