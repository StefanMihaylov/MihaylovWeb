using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Gallery.Controllers
{
    [Area("Gallery")]
    public class GalleryController : Controller
    {
        private readonly IOtherApiClient _client;

        public GalleryController(IOtherApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AlbumModel> images = await _client.GalleryImagesAsync().ConfigureAwait(false);

            return View(images);
        }

        [HttpGet]
        [OutputCache(Duration = 3600, VaryByRouteValueNames = new[] { "id" })]
        public async Task<IActionResult> Thumbnail(string id)
        {
            FileResponse image = await _client.GalleryThumbnailAsync(id).ConfigureAwait(false);
            if (image == null || image.Stream == null || image.StatusCode != 200)
            {
                return NotFound();
            }

            return File(image.Stream, "image/jpeg");
        }
    }
}
