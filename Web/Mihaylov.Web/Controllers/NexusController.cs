using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common.Host.Authorization;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Models.Nexus;

namespace Mihaylov.Web.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
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
            _client.AddToken(Request.GetToken());
            NexusImages images = await _client.ImagesAsync().ConfigureAwait(false);
            NexusBlobs blobs = await _client.BlobsAsync().ConfigureAwait(false);

            var model = new NexusMainModel
            {
                Images = images,
                Blobs = blobs
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Clear()
        {
            _client.AddToken(Request.GetToken());

            await _client.ClearImagesAsync().ConfigureAwait(false);
            await _client.RunTasksAsync().ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }
    }
}
