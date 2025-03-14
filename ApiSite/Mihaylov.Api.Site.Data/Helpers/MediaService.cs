﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Api.Site.Data.Media.Interfaces;
using Mihaylov.Common;
using Mihaylov.Common.Generic.Servises.Interfaces;
using Mihaylov.Common.Generic.Servises.Models;

namespace Mihaylov.Api.Site.Data.Helpers
{
    public class MediaService : IMediaService
    {
        private readonly IFileWrapper _file;
        private readonly IImageMediaService _imageMediaService;
        private readonly IVideoMediaService _videoMediaService;
        private readonly PathConfig _config;

        public MediaService(IFileWrapper fileWrapper, IImageMediaService imageMediaService,
            IVideoMediaService videoMediaService, IOptions<PathConfig> settings)
        {
            _file = fileWrapper;
            _imageMediaService = imageMediaService;
            _videoMediaService = videoMediaService;

            _config = settings.Value;
        }

        public string GetBasePath()
        {
            return _config.BasePath;
        }

        public IEnumerable<MediaInfoModel> GetAllFiles(string directoryPath, bool includeSubdirectories, bool readOnly,
            bool calculateChecksum, Action<ScanProgressModel> progress)
        {
            var fileInfos = _file.GetAllFiles(directoryPath, includeSubdirectories);

            var result = new List<MediaInfoModel>();

            long count = 0;
            foreach (var fileInfo in fileInfos)
            {
                count++;
                var fileProcessed = count.GetPersentage(fileInfos.Count());

                if (fileInfo.Length == 0 && fileInfo.Extension == ".nomedia")
                {
                    _file.DeleteFile(fileInfo.FullName);
                    continue;
                }

                MediaInfoModel mediaInfo = GetMediaData(fileInfo, readOnly, calculateChecksum, p => progress(new ScanProgressModel(fileProcessed, p)));

                result.Add(mediaInfo);
            }

            return result;
        }

        public byte[] GetThumbnail(string filePath, int size)
        {
            byte[] bytes = new byte[0];

            var fileinfo = _file.GetFileInfo(filePath);
            if (!fileinfo.Exists)
            {
                return bytes;
            }

            var mediaInfo = GetMediaData(fileinfo, false, false, a => { });
            IBaseMediaService service = GetService(mediaInfo.IsImage);

            bytes = service.GetThumbnail(filePath, mediaInfo.Size, size);

            return bytes;
        }

        public DuplicateResponse GetDuplicates(IEnumerable<MediaInfoModel> files)
        {
            var duplicates = files.GroupBy(m => m.Checksum)
                                  .Select(g => new DuplicateModel()
                                  {
                                      Checksum = g.Key,
                                      Count = g.Count(),
                                      List = g.OrderBy(p => p.DownloadedOn)
                                  })
                                  .Where(m => m.Count > 1)
                                  .ToList();

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
            basePath = basePath ?? _config.BasePath;
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
                Lenght = sizeInfo?.Lenght,
                Checksum = sizeInfo?.Checksum,
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
    }
}
