using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Common.Generic.Servises.Interfaces;
using Mihaylov.Common.Host.Authorization;
using Mihaylov.Web.Hubs;
using Mihaylov.Web.Models;
using Mihaylov.Web.Models.Configs;

namespace Mihaylov.Web.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IFileWrapper _file;
        private readonly IProgressReporterFactory _progressReporter;
        private readonly IMemoryCache _cache;
        private readonly MediaConfig _config;

        private readonly string _reportPath;
        private readonly string _sortReportPath;

        public MediaController(IMediaService mediaService, IFileWrapper fileWrapper,
            IProgressReporterFactory progressReporter, IMemoryCache cache, IOptions<MediaConfig> settings)
        {
            _mediaService = mediaService;
            _file = fileWrapper;
            _progressReporter = progressReporter;
            _cache = cache;

            _reportPath = $"{_mediaService.GetBasePath()}/response.json";
            _sortReportPath = $"{_mediaService.GetBasePath()}/sortResponse.json";
            _config = settings.Value;
        }

        [HttpGet]
        public IActionResult Duplicates()
        {
            var model = _mediaService.GetReportData<DuplicateResponse>(_reportPath, null);

            var duplicates = model.Duplicates
                                 //.Where(d => d.List.Any(f => !f.IsImage))                                 
                                 .Select(d => new DuplicateModel()
                                 {
                                     Checksum = d.Checksum,
                                     List = d.List.Where(f => _file.GetFileInfo(f.FullPath).Exists),
                                     Count = d.Count
                                 })
                                 .Where(d => d.List.Count() > 1)
                                 .ToList();

            return View(new DuplicatesViewModel()
            {
                FilesCount = model.FileCount,
                DuplicatesCount = duplicates.Count,
                Duplicates = duplicates.Take(2),
                RedirectUrl = nameof(Duplicates)
            });
        }

        [HttpGet]
        public IActionResult Sort(int? page)
        {
            int pageSize = 8;
            int columnCount = 4;

            var model = _mediaService.GetReportData<SortResponse>(_sortReportPath, null);
            var files = model.Files.Where(f => _file.GetFileInfo(f.FullPath).Exists).ToList();

            var directories = _file.GetDirectories(_config.SoftPath);

            var picsOnPage = columnCount * pageSize;
            var maxPageCount = files.Count / picsOnPage;
            if (files.Count % picsOnPage > 0)
            {
                maxPageCount++;
            }

            if (page <= 0)
            {
                return Redirect(nameof(Sort));
            }

            if (page >= maxPageCount && maxPageCount > 0)
            {
                return Redirect($"Sort?page={maxPageCount - 1}");
            }

            if (!page.HasValue)
            {
                page = 0;
            }

            return View(new SortViewModel()
            {
                Page = page.Value,
                PageCount = maxPageCount,
                FilesCount = files.Count,
                LastProcessed = model.LastProcessed,
                Files = Convert(files, columnCount).Skip(page.Value * pageSize).Take(pageSize),
                Dirs = directories,
                RedirectUrl = nameof(Sort)
            });
        }

        [HttpGet]
        public IActionResult GetThumbnail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!_cache.TryGetValue(id, out byte[] bytes))
            {
                var path = WebUtility.UrlDecode(id);
                bytes = _mediaService.GetThumbnail(path, 200);

                _cache.Set(id, bytes, TimeSpan.FromMinutes(60));
            }

            if (bytes?.Any() != true)
            {
                return NotFound();
            }

            return File(bytes, "image/png");
        }

        [HttpGet]
        public IActionResult DeleteFile(string id, string redirectUrl)
        {
            var path = WebUtility.UrlDecode(id);

            _file.DeleteFile(path);

            return RedirectToAction(redirectUrl);
        }

        [HttpGet]
        public IActionResult DeleteFileById(Guid id, string redirectUrl)
        {
            var filesModel = _mediaService.GetReportData<SortResponse>(_sortReportPath, null);
            var path = filesModel.Files.Where(f => f.FileId == id).Select(f => f.FullPath).FirstOrDefault();

            _file.DeleteFile(path);

            return RedirectToAction(redirectUrl);
        }

        [HttpGet]
        public IActionResult ReadFiles(string connectionId)
        {
            var progressReporter = _progressReporter.GetLoadingBarReporter(connectionId, "updateLoadingBar");

            var files = new List<MediaInfoModel>();

            if (!string.IsNullOrEmpty(_config.DuplicatePathLocked))
            {
                var lockedFiles = _mediaService.GetAllFiles(_config.DuplicatePathLocked, true, true, true, progressReporter.Report);
                files.AddRange(lockedFiles);
            }

            if (!string.IsNullOrEmpty(_config.DuplicatePathNotLocked))
            {
                var nonlockedFiles = _mediaService.GetAllFiles(_config.DuplicatePathNotLocked, true, false, true, progressReporter.Report);
                files.AddRange(nonlockedFiles);
            }

            var content = _mediaService.GetDuplicates(files);
            var result = JsonSerializer.Serialize(content);
            _file.SaveFile(_reportPath, result);

            return Ok();
        }

        [HttpGet]
        public IActionResult ReadFilesSort(string connectionId)
        {
            var progressReporter = _progressReporter.GetLoadingBarReporter(connectionId, "updateLoadingBar");

            var files = _mediaService.GetAllFiles(_config.SoftPath, false, false, false, progressReporter.Report);

            var content = _mediaService.GetSorted(files);

            content.LastProcessed = new DateTime(2025, 03, 01);

            var result = JsonSerializer.Serialize(content);
            _file.SaveFile(_sortReportPath, result);

            return Ok();
        }

        [HttpPost]
        public IActionResult CreateDirectory([FromForm] string direcrotyName)
        {
            if (string.IsNullOrEmpty(direcrotyName))
            {
                return Redirect(nameof(Sort));
            }

            _file.CreateDirectory(_config.SoftPath, direcrotyName);
            return Redirect(nameof(Sort));
        }

        [HttpPost]
        public IActionResult MovePics([FromForm] MovePicsViewModel model)
        {
            if (model.Files != null && model.Files.Any() && !string.IsNullOrWhiteSpace(model.Dir))
            {
                var filesModel = _mediaService.GetReportData<SortResponse>(_sortReportPath, null);

                var fileNames = filesModel.Files.Where(f => model.Files.Contains(f.FileId))
                                                .Select(f => f.FullPath)
                                                .ToList();
                var dirPath = Path.Combine(_config.SoftPath, model.Dir);

                _file.MoveFiles(dirPath, fileNames);
            }

            return Redirect(nameof(Sort));
        }

        [HttpGet]
        public IActionResult RenamePics()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RenamePicsRun()
        {
            var directory = new DirectoryInfo(_config.RenamePath);
            if (directory.Exists)
            {
                var files = directory.GetFiles();
                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        var fileNameParts = file.Name.Split('_', StringSplitOptions.RemoveEmptyEntries);
                        var filePrefix = fileNameParts[0].Substring(0, 3);
                        int number = int.Parse(fileNameParts[0].Substring(3));

                        var newFilePath = $"{file.DirectoryName}\\{filePrefix}{number + 1}_{fileNameParts[1]}";

                        System.IO.File.Move(file.FullName, newFilePath);
                    }
                }
            }

            return Redirect(nameof(RenamePics));
        }

        private IEnumerable<IEnumerable<MediaInfoModel>> Convert(IEnumerable<MediaInfoModel> list, int columnCount)
        {
            var result = new List<List<MediaInfoModel>>();

            if (!list.Any())
            {
                return result;
            }

            var count = 0;

            do
            {
                var row = new List<MediaInfoModel>();
                result.Add(row);

                for (int j = 0; j < columnCount; j++)
                {
                    row.Add(list.ElementAt(count));
                    count++;

                    if (count >= list.Count())
                    {
                        return result;
                    }
                }
            }
            while (true);
        }
    }
}
