using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IVersionService _versionService;

        public ClusterController(ILoggerFactory loggerFactory, IClusterService service, IVersionService versionService)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _service = service;
            _versionService = versionService;
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
                Order = model.Order ?? 0,
                Name = model?.Name,
                SiteUrl = model?.SiteUrl,
                ReleaseUrl = model?.ReleaseUrl,
                GithubVersionUrl = model?.GithubVersionUrl,
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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ParserSetting>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ParserSettings()
        {
            var settings = await _service.GetParserSettingsAsync().ConfigureAwait(false);

            return Ok(settings);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ParserSetting), StatusCodes.Status200OK)]
        public async Task<IActionResult> ParserSetting(ParserSettingModel model)
        {
            var request = new ParserSetting()
            {
                Id = model?.Id ?? 0,
                ApplicationId = model?.ApplicationId ?? 0,
                ApplicationName = null,
                VersionUrlType = model?.VersionUrlType ?? VersionUrlType.ReleaseUrl,
                VersionSelector = model?.VersionSelector,
                VersionCommand = model?.VersionCommand,
                ReleaseDateUrlType = model?.ReleaseDateUrlType,
                ReleaseDateSelector = model?.ReleaseDateSelector,
                ReleaseDateCommand = model?.ReleaseDateCommand,
            };

            ParserSetting settings = await _service.AddOrUpdateParserSettingAsync(request).ConfigureAwait(false);

            return Ok(settings);
        }

        [HttpGet]
        [ProducesResponseType(typeof(LastVersionResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> LastVersion(int id)
        {
            var result = await _versionService.GetLastVersionAsync(id).ConfigureAwait(false);

            return Ok(new LastVersionResponse()
            {
                LastVersion = result,
            });
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public IActionResult ReloadLastVersion(int id)
        {
            _versionService.Reload(id);

            return Ok();
        }
    }
}
