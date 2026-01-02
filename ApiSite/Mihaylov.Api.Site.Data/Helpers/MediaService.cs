using System;
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
            var duplicates = GetDuplucatesChecksum(files);
            GetDuplucatesHash(duplicates, files, 6);

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
                PerceptualHash = sizeInfo?.PerceptualHash,
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

        private void GetDuplucatesHash(List<DuplicateModel> duplicateList, IEnumerable<MediaInfoModel> files, double minDistance)
        {
            var vectorSize = 64;
            var filesData = files.Where(f => f.PerceptualHash.HasValue).ToList();

            var hashIdDict = new Dictionary<int, MediaInfoModel>();

            double[,] data = new double[filesData.Count, vectorSize];
            var tags = new int[filesData.Count];

            for (int n = 0; n < filesData.Count; n++)
            {
                var file = filesData[n];

                tags[n] = n;
                hashIdDict.Add(n, file);

                double[] vector = ULongToDoubleVector(file.PerceptualHash.Value, vectorSize);

                for (int d = 0; d < vector.Length; d++)
                {
                    data[n, d] = vector[d];
                }
            }

            alglib.kdtreebuildtagged(data, tags, data.GetLength(0), data.GetLength(1), 0, 2, out alglib.kdtree tree);

            var duplicateFiles = new HashSet<string>();
            foreach (var duplicate in duplicateList)
            {
                string key = GetDuplicateKey(duplicate.List.Select(f => f.FileId), 0);
                duplicateFiles.Add(key);
            }

            var radius = Math.Sqrt(minDistance * vectorSize / 100.0);

            for (int n = 0; n < tags.Length; n++)
            {
                var tag = tags[n];

                var vector = new double[vectorSize];
                for (int v = 0; v < vector.Length; v++)
                {
                    vector[v] = data[n, v];
                }

                int neighborsCount = alglib.kdtreequeryrnnu(tree, vector, radius, true);

                var nearestNeighbors = GetNeighbours(tree, tag);
                if (nearestNeighbors.Count == 0)
                {
                    continue;
                }

                var current = hashIdDict[tag];

                foreach (var neighborData in nearestNeighbors)
                {
                    var neighbours = new List<MediaInfoModel>()
                    {
                        current,
                        hashIdDict[neighborData.Tag]
                    };

                    var key = GetDuplicateKey(neighbours.Select(n => n.FileId), neighborData.Distance);
                    if (duplicateFiles.Contains(key))
                    {
                        continue;
                    }

                    duplicateFiles.Add(key);

                    var checksums = neighbours.Select(n => n.Checksum).Distinct().ToList();
                    var hashes = neighbours.Select(n => n.PerceptualHash).Distinct().ToList();

                    duplicateList.Add(new DuplicateModel()
                    {
                        Checksum = checksums.Count == 1 ? checksums.First() : null,
                        Hash = hashes.Count == 1 ? hashes.First() : null,
                        Similarity = 100 - neighborData.Distance,
                        List = neighbours,
                        Count = neighbours.Count()
                    });
                }
            }
        }

        private static string GetDuplicateKey(IEnumerable<Guid> fileIds, double distance)
        {
            return $"{string.Join('_', fileIds.OrderBy(d => d))}_{distance}";
        }

        private List<(int Tag, double Distance)> GetNeighbours(alglib.kdtree tree, int tag, double? minDistance = null)
        {
            var currentTags = new int[0];
            var currentDistance = new double[0];

            alglib.kdtreequeryresultsdistances(tree, ref currentDistance);
            alglib.kdtreequeryresultstags(tree, ref currentTags);

            var nearestNeighbors = new List<(int Tag, double Distance)>();
            for (int i = 0; i < currentTags.Length; i++)
            {
                if (currentTags[i] == tag)
                {
                    continue;
                }

                var bitcount = Math.Round(currentDistance[i] * currentDistance[i]);
                var distance = bitcount * 100 / 64.0;

                if (minDistance.HasValue)
                {
                    if (distance > minDistance.Value)
                    {
                        continue;
                    }
                }

                nearestNeighbors.Add((currentTags[i], distance));
            }

            return nearestNeighbors;
        }

        private double[] ULongToDoubleVector(ulong? value, int vectorLength)
        {
            if (!value.HasValue)
            {
                return null;
            }

            var vector = new double[vectorLength];
            for (int i = 0; i < vectorLength; i++)
            {
                // (value >> i) & 1 isolates the i-th bit
                vector[i] = ((value.Value >> i) & 1) == 1 ? 1.0 : 0.0;
            }

            return vector;
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
