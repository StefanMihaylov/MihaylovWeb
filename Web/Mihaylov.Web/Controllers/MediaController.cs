using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mihaylov.Common.Host.Authorization;
using Mihaylov.Site.Media.Interfaces;
using Mihaylov.Site.Media.Models;
using Mihaylov.Web.Hubs;
using Mihaylov.Web.Models;

namespace Mihaylov.Web.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IFileWrapper _file;
        private readonly IProgressReporterFactory _progressReporter;
        private readonly MediaConfig _config;

        private readonly string _reportPath;

        public MediaController(IMediaService mediaService, IFileWrapper fileWrapper,
            IProgressReporterFactory progressReporter, IOptions<MediaConfig> settings)
        {
            _mediaService = mediaService;
            _file = fileWrapper;
            _progressReporter = progressReporter;

            _reportPath = $"{_file.GetBasePath()}/response.json";
            _config = settings.Value;
        }

        [HttpGet]
        public IActionResult Duplicates()
        {
            var model = _mediaService.GetDuplicates(_reportPath, null);

            var duplicates = model.Duplicates
                                 //.Where(d => d.List.Any(f => !f.IsImage))
                                 .Where(d => d.List.Where(f => _file.GetFileInfo(f.FullPath).Exists).Count() > 1)
                                 .ToList();

            return View(new DuplicatesViewModel()
            {
                FilesCount = model.FileCount,
                DuplicatesCount = duplicates.Count,
                Duplicates = duplicates.Take(2)
            });
        }

        [HttpGet]
        public IActionResult GetThumbnail(string id)
        {
            var path = WebUtility.UrlDecode(id);

            var bytes = _mediaService.GetThumbnail(path, 300);
            if (bytes?.Any() != true)
            {
                return NotFound();
            }

            return File(bytes, "image/png");
        }

        [HttpGet]
        public IActionResult DeleteFile(string id)
        {
            var path = WebUtility.UrlDecode(id);

            _file.DeleteFile(path);

            return RedirectToAction("Duplicates");
        }

        [HttpGet]
        public IActionResult ReadFiles(string connectionId)
        {
            var progressReporter = _progressReporter.GetLoadingBarReporter(connectionId, "updateLoadingBar");

            var files = new List<MediaInfoModel>();

            var folderLocked = _config.DuplicatePathLocked;
            if (!string.IsNullOrEmpty(folderLocked))
            {
                var lockedFiles = _mediaService.GetAllFiles(folderLocked, true, true, progressReporter.Report);
                files.AddRange(lockedFiles);
            }

            var folderNotLocked = _config.DuplicatePathNotLocked;
            if (!string.IsNullOrEmpty(folderNotLocked))
            {
                var notlockedFiles = _mediaService.GetAllFiles(folderNotLocked, true, false, progressReporter.Report);
                files.AddRange(notlockedFiles);
            }

            var result = _mediaService.GetDuplicates(files);
            _file.SaveFile(_reportPath, result);

            return Ok();
        }
    }
}
