using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Extensions;
using System.Collections.Generic;
using Mihaylov.Api.Other.Models.Cluster;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Version;

namespace Mihaylov.Api.Other.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IVersionService _versionService;

        public TestController(ILoggerFactory loggerFactory, IVersionService versionService)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _versionService = versionService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LastVersionResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> TestLastVersion(LastVersionSettings model)
        {
            var result = await _versionService.TestLastVersionAsync(model).ConfigureAwait(false);

            return Ok(new LastVersionResponse()
            {
                LastVersion = result,
            });
        }
    }
}
