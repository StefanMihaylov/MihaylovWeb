using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Extensions;
using Mihaylov.Api.Site.Models;
using Mihaylov.Common.Host.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Mihaylov.Api.Site.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigurationManager _manager;
        private readonly IConfigurationWriter _writer;

        public ConfigController(IConfigurationManager manager, IConfigurationWriter writer)
        {
            _manager = manager;
            _writer = writer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DefaultFilter>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DefaultFilters()
        {
            IEnumerable<DefaultFilter> filters = await _manager.GetAllDefaultFiltersAsync().ConfigureAwait(false);
            return Ok(filters);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DefaultFilter), StatusCodes.Status200OK)]
        public async Task<IActionResult> DefaultFilter()
        {
            DefaultFilter filter = await _manager.GetDefaultFilterAsync().ConfigureAwait(false);
            return Ok(filter);
        }

        [HttpPost]
        [SwaggerOperation(OperationId = "AddDefailtFilter")]
        [ProducesResponseType(typeof(DefaultFilter), StatusCodes.Status200OK)]
        public async Task<IActionResult> DefaultFilter(AddDefaultFilterModel input)
        {
            var model = new DefaultFilter()
            {
                Id = input.Id ?? 0,
                IsEnabled = input.IsEnabled.Value,
                AccountTypeId = input.AccountTypeId,
                StatusId = input.StatusId,
                IsArchive = input.IsArchive.Value,
                IsPreview = input.IsPreview,
            };

            DefaultFilter filter = await _writer.AddDefaultFilterAsync(model).ConfigureAwait(false);
            return Ok(filter);
        }
    }
}
