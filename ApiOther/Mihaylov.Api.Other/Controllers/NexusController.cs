using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Other.Contracts.Nexus.Interfaces;
using Mihaylov.Api.Other.Contracts.Nexus.Models;
using Mihaylov.Api.Other.Extensions;
using Mihaylov.Common.Host.Authorization;

namespace Mihaylov.Api.Other.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class NexusController : ControllerBase
    {
        private readonly INexusApiService _service;

        public NexusController(INexusApiService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(NexusImages), StatusCodes.Status200OK)]
        public async Task<IActionResult> Images()
        {
            NexusImages images = await _service.GetDockerImagesAsync().ConfigureAwait(false);

            return Ok(images);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> ClearImages()
        {
            await _service.ClearImages().ConfigureAwait(false);

            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> Image(string imageId)
        {
            var result = await _service.DeleteDockerImageAsync(imageId).ConfigureAwait(false);
            return result ? Ok() : NotFound();
        }

        [HttpGet]
        [ProducesResponseType(typeof(NexusBlobs), StatusCodes.Status200OK)]
        public async Task<IActionResult> Blobs()
        {
            NexusBlobs blobs = await _service.GetBlobsAsync().ConfigureAwait(false);

            return Ok(blobs);
        }

        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> RunTasks()
        {
            await _service.RunTasksAsync().ConfigureAwait(false);

            return Ok();
        }
    }
}
