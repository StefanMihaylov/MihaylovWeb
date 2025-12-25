using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Other.Contracts.Gallery.Interfaces;
using Mihaylov.Api.Other.Contracts.Gallery.Models;
using Mihaylov.Api.Other.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace Mihaylov.Api.Other.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    public class GalleryController : ControllerBase
    {
        private readonly IImmichService _service;

        public GalleryController(IImmichService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AlbumModel>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GalleryImages")]
        public async Task<IActionResult> Images()
        {
            IEnumerable<AlbumModel> images = await _service.GetAlbumsAsync().ConfigureAwait(false);

            return Ok(images);
        }

        [HttpGet]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK, "application/octet-stream", "application/json")]
        [SwaggerOperation(OperationId = "GalleryThumbnail")]
        public async Task<IActionResult> Thumbnail(string id)
        {
            var file = await _service.GetThumbnailAsync(id).ConfigureAwait(false);
            if(file == null || file.Length == 0)
            {
                return BadRequest();
            }

            return File(file, "application/octet-stream");
        }
    }
}
