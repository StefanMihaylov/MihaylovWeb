using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Client;
using Mihaylov.Web.Areas;

namespace Mihaylov.Web.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOtherApiClient _client;

        public GalleryController(ILoggerFactory loggerFactory, IOtherApiClient client)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _client.AddToken(Request.GetToken());
            IEnumerable<AlbumModel> images = await _client.GalleryImagesAsync().ConfigureAwait(false);

            return View(images);
        }

        [HttpGet]
        public async Task<IActionResult> Thumbnail(string id)
        {
            _client.AddToken(Request.GetToken());
            FileResponse image = await _client.GalleryThumbnailAsync(id).ConfigureAwait(false);
            if (image == null || image.Stream == null || image.StatusCode != 200)
            {
                return NotFound();
            }

            return File(image.Stream, "image/jpeg");
        }
    }
}
