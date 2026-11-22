using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;
using Mihaylov.Api.Other.Extensions;
using Mihaylov.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace Mihaylov.Api.Other.Controllers
{
    [AdminAuthorize]
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

        [HttpGet]
        [ProducesResponseType(typeof(NugetPackages), StatusCodes.Status200OK)]
        public async Task<IActionResult> Nugets()
        {
            NugetPackages nugets = await _service.GetNugetPackagesAsync().ConfigureAwait(false);

            return Ok(nugets);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DependencyNodeFlat>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Nuget([FromQuery] string packageName, [FromQuery] string version, [FromQuery] string framework)
        {
            DependencyNode nuget = await _service.GetNugetPackageAsync(packageName, version, framework).ConfigureAwait(false);
            IEnumerable<DependencyNodeFlat> flat = nuget.ToFlatModel();

            return Ok(flat);
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
        [SwaggerOperation(OperationId = "DeleteImage")]
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
