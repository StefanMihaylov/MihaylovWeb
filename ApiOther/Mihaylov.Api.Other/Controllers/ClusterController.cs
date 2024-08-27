using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models;
using Mihaylov.Api.Other.Extensions;
using Mihaylov.Api.Other.Models.Cluster;
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

    public class ClusterController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IClusterService _service;

        public ClusterController(ILoggerFactory loggerFactory, IClusterService service)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApplicationExtended>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Applications()
        {
            IEnumerable<ApplicationExtended> applications = await _service.GetAllApplicationsAsync().ConfigureAwait(false);
            
            return Ok(applications);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApplicationExtended), StatusCodes.Status200OK)]
        public async Task<IActionResult> Application(ApplicationModel model)
        {
            var request = new Application()
            {
                Id = model?.Id ?? 0,
                Name = model?.Name,
                ReleaseUrl = model?.ReleaseUrl,
                ResourceUrl = model?.ResourceUrl,
                Deployment = model?.Deployment ?? DeploymentType.Yaml,
                Notes = model?.Notes,
            };

            ApplicationExtended application = await _service.AddOrUpdateApplicationAsync(request).ConfigureAwait(false);

            return Ok(application);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Pod), StatusCodes.Status200OK)]
        public async Task<IActionResult> Pod(PodModel model)
        {
            var request = new Pod()
            {
                Id = model?.Id ?? 0,
                Name = model?.Name,
            };

            Pod pod = await _service.AddOrUpdatePodAsync(request, model.ApplicationId).ConfigureAwait(false);

            return Ok(pod);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DeploymentFile), StatusCodes.Status200OK)]
        public async Task<IActionResult> File(DeploymentFileModel model)
        {
            var request = new DeploymentFile()
            {
                Id = model?.Id ?? 0,
                Name = model?.Name,
            };

            DeploymentFile file = await _service.AddOrUpdateFileAsync(request, model.ApplicationId).ConfigureAwait(false);

            return Ok(file);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AppVersion), StatusCodes.Status200OK)]
        public async Task<IActionResult> Version(AppVersionModel model)
        {
            var request = new AppVersion()
            {
                Id = model?.Id ?? 0,
                Version = model?.Version,
                HelmVersion = model?.HelmVersion,
                HelmAppVersion = model?.HelmAppVersion,
                ReleaseDate = model?.ReleaseDate ?? DateTime.UtcNow.Date,
            };

            AppVersion version = await _service.AddOrUpdateVersionAsync(request, model.ApplicationId).ConfigureAwait(false);

            return Ok(version);
        }
    }
}
