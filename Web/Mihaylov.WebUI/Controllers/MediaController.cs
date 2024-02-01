using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Site.Media.Interfaces;
using Mihaylov.WebUI.Hubs;
using Mihaylov.WebUI.Models;

namespace Mihaylov.WebUI.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IFileWrapper _file;
        private readonly IProgressReporterFactory _progressReporter;

        private readonly string _reportPath;

        public MediaController(IMediaService mediaService, IFileWrapper fileWrapper,
            IProgressReporterFactory progressReporter)
        {
            _mediaService = mediaService;
            _file = fileWrapper;
            _progressReporter = progressReporter;

            _reportPath = $"{_file.GetBasePath()}/response.json";
        }

        [HttpGet]
        public IActionResult Index()
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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ReadFiles(string connectionId)
        {
            var progressReporter = _progressReporter.GetLoadingBarReporter(connectionId, "updateLoadingBar");

            var path = "";
            var path2 = "";

            var files = _mediaService.GetAllFiles(path, true, true, progressReporter.Report).ToList();
            var files2 = _mediaService.GetAllFiles(path2, true, false, progressReporter.Report);

            files.AddRange(files2);

            var result = _mediaService.GetDuplicates(files);

            _file.SaveFile(_reportPath, result);

            return Ok();
        }
    }
}
