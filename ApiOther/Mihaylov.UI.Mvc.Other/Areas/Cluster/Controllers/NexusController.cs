using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Other.Areas.Cluster.Models.Nexus;

namespace Mihaylov.UI.Mvc.Other.Areas.Cluster.Controllers
{
    [AdminAuthorize]
    [Area("Cluster")]
    public class NexusController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOtherApiClient _client;

        public NexusController(ILoggerFactory loggerFactory, IOtherApiClient client)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            NexusImages images = await _client.ImagesAsync().ConfigureAwait(false);
            NexusBlobs blobs = await _client.BlobsAsync().ConfigureAwait(false);
            NugetPackages nugets = await _client.NugetsAsync().ConfigureAwait(false);

            var model = new NexusMainModel
            {
                Images = images,
                Blobs = blobs,
                Nugets = nugets,
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Clear()
        {
            await _client.ClearImagesAsync().ConfigureAwait(false);
            await _client.RunTasksAsync().ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(string imageId)
        {
            await _client.DeleteImageAsync(imageId).ConfigureAwait(false);
            await _client.RunTasksAsync().ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }
    }
}
