using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Velero;
using Mihaylov.Api.Other.Extensions;
using Mihaylov.Api.Other.Models.Cluster;
using Mihaylov.Common.Host.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Mihaylov.Api.Other.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class VeleroController : ControllerBase
    {
        private readonly IVeleroService _service;

        public VeleroController(IVeleroService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExeVersion()
        {
            string version = await _service.GetExeVersionAsync().ConfigureAwait(false);

            return Ok(version);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ScheduleResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Schedules()
        {
            ScheduleResponse schedules = await _service.GetSchedulesAsync().ConfigureAwait(false);

            return Ok(schedules);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateBackup")]
        public async Task<IActionResult> Backup([FromBody]CreateBackupModel model)
        {
            string result = await _service.CreateBackupAsync(model.ScheduleName).ConfigureAwait(false);

            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBackup([FromQuery] string backupName)
        {
            string result = await _service.DeleteBackupAsync(backupName).ConfigureAwait(false);

            return Ok(result);
        }
    }
}
