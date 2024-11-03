using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common.Host.Authorization;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Models.Cluster;

namespace Mihaylov.Web.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    public class ClusterController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOtherApiClient _client;

        public ClusterController(ILoggerFactory loggerFactory, IOtherApiClient client)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? settingId)
        {
            ClusterMainModel result = await FillModelAsync(id, settingId, null, null, null, null).ConfigureAwait(false);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddApp(AddApplicationModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ClusterMainModel viewModel = await FillModelAsync(null, null, inputModel, null, null, null).ConfigureAwait(false);
                return View(nameof(Index), viewModel);
            }

            var model = new ApplicationModel()
            {
                Id = inputModel.Id,
                Name = inputModel?.Name,
                ReleaseUrl = inputModel?.ReleaseUrl,
                ResourceUrl = inputModel?.ResourceUrl,
                SiteUrl = inputModel?.SiteUrl,
                GithubVersionUrl = inputModel?.GithubVersionUrl,
                Deployment = inputModel?.Deployment ?? DeploymentType.Yaml,
                Notes = inputModel?.Notes,
            };

            _client.AddToken(Request.GetToken());
            var applications = await _client.ApplicationAsync(model).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddAdditional(AddAdditionalModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ClusterMainModel viewModel = await FillModelAsync(null, null, null, inputModel, null, null).ConfigureAwait(false);
                return View(nameof(Index), viewModel);
            }

            _client.AddToken(Request.GetToken());

            if (!string.IsNullOrWhiteSpace(inputModel.File) && inputModel.ApplicationId.HasValue)
            {
                var model = new DeploymentFileModel()
                {
                    ApplicationId = inputModel.ApplicationId.Value,
                    Name = inputModel?.File,
                };

                var files = await _client.FileAsync(model).ConfigureAwait(false);
            }

            if (!string.IsNullOrWhiteSpace(inputModel.Pod) && inputModel.ApplicationId.HasValue)
            {
                var model = new PodModel()
                {
                    ApplicationId = inputModel.ApplicationId.Value,
                    Name = inputModel?.Pod,
                };

                var files = await _client.PodAsync(model).ConfigureAwait(false);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddVersion(AddVersionModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ClusterMainModel viewModel = await FillModelAsync(null, null, null, null, inputModel, null).ConfigureAwait(false);
                return View(nameof(Index), viewModel);
            }

            _client.AddToken(Request.GetToken());

            var model = new AppVersionModel()
            {
                ApplicationId = inputModel.ApplicationId.Value,
                ReleaseDate = inputModel?.ReleaseDate.Value ?? DateTime.UtcNow.Date,
                Version = inputModel?.Version,
                HelmVersion = inputModel?.HelmVersion,
                HelmAppVersion = inputModel?.HelmAppVersion,
            };

            var files = await _client.VersionAsync(model).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddParserSetting(AddParserSettingModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ClusterMainModel viewModel = await FillModelAsync(null, null, null, null, null, inputModel).ConfigureAwait(false);
                return View(nameof(Index), viewModel);
            }

            var model = new ParserSettingModel()
            {
                Id = inputModel.Id,
                ApplicationId = inputModel.ApplicationId.Value,
                VersionUrlType = inputModel.VersionUrlType.Value,
                VersionSelector = inputModel.VersionSelector,
                VersionCommand = inputModel.VersionCommand,
                ReleaseDateUrlType = inputModel.ReleaseDateUrlType,
                ReleaseDateSelector = inputModel.ReleaseDateSelector,
                ReleaseDateCommand = inputModel.ReleaseDateCommand,
            };

            _client.AddToken(Request.GetToken());
            var setting = await _client.ParserSettingAsync(model).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ReloadLastVersion(int Id)
        {
            _client.AddToken(Request.GetToken());
            await _client.ReloadLastVersionAsync(Id).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }

        private async Task<ClusterMainModel> FillModelAsync(int? id, int? settingId, AddApplicationModel inputModel,
        AddAdditionalModel additional, AddVersionModel version, AddParserSettingModel parserSetting)
        {
            _client.AddToken(Request.GetToken());
            var applications = await _client.ApplicationsAsync().ConfigureAwait(false);

            var deploymentTypes = Enum.GetValues(typeof(DeploymentType))
                      .Cast<DeploymentType>()
                      .Select(v => new SelectListItem(v.ToString(), ((int)v).ToString()))
                      .ToList();

            var applicationDropDown = applications.Select(v => new SelectListItem(v.Name, v.Id.ToString())).ToList();

            var UrlTypes = Enum.GetValues(typeof(VersionUrlType))
                      .Cast<VersionUrlType>()
                      .Select(v => new SelectListItem(v.ToString(), ((int)v).ToString()))
                      .ToList();

            var parserSettings = await _client.ParserSettingsAsync().ConfigureAwait(false);

            var appIds = applications.Select(a => a.Id);
            var versionDictionary = new Dictionary<int, LastVersionModel>();
            var availableSettings = new HashSet<int>(parserSettings.Select(p => p.ApplicationId));

            foreach (var appId in appIds)
            {
                var lastVersion = await _client.LastVersionAsync(appId).ConfigureAwait(false);
                versionDictionary.Add(appId, lastVersion.LastVersion);
            }

            var applicationsExtended = applications.Select(application =>
            {
                var lastVersion = versionDictionary[application.Id];

                var isLastVersion = false;
                if (lastVersion != null && application.Version != null)
                {
                    isLastVersion = application.Version.Version == lastVersion.Version;
                }

                return new ApplicationViewModel()
                {
                    Main = application,
                    LastVersion = lastVersion,
                    IsLatestVersion = isLastVersion,
                    ParserSettingAvailable = availableSettings.Contains(application.Id),
                };
            })
            .ToList();

            if (id.HasValue)
            {
                var app = applications.Where(a => a.Id == id.Value).FirstOrDefault();
                if (app != null)
                {
                    inputModel = new AddApplicationModel()
                    {
                        Id = app.Id,
                        Name = app.Name,
                        Deployment = app.Deployment,
                        SiteUrl = app.SiteUrl,
                        ReleaseUrl = app.ReleaseUrl,
                        ResourceUrl = app.ResourceUrl,
                        GithubVersionUrl = app.GithubVersionUrl,
                        Notes = app.Notes,
                    };
                }
            }

            if (settingId.HasValue)
            {
                var settings = parserSettings.Where(p => p.Id == settingId.Value).FirstOrDefault();
                if (settings != null)
                {
                    parserSetting = new AddParserSettingModel()
                    {
                        Id = settings.Id,
                        ApplicationId = settings.ApplicationId,
                        VersionUrlType = settings.VersionUrlType,
                        VersionSelector = settings.VersionSelector,
                        VersionCommand = settings.VersionCommand,
                        ReleaseDateUrlType = settings.ReleaseDateUrlType,
                        ReleaseDateSelector = settings.ReleaseDateSelector,
                        ReleaseDateCommand = settings.ReleaseDateCommand,
                    };
                }
            }

            if (inputModel == null)
            {
                inputModel = new AddApplicationModel();
            }

            if (additional == null)
            {
                additional = new AddAdditionalModel();
            }

            if (version == null)
            {
                version = new AddVersionModel()
                {
                    ReleaseDate = DateTime.UtcNow.Date,
                };
            }

            if (parserSetting == null)
            {
                parserSetting = new AddParserSettingModel();
            }

            inputModel.DeploymentTypes = deploymentTypes;
            additional.Applications = applicationDropDown;
            version.Applications = applicationDropDown;
            parserSetting.UrlTypes = UrlTypes;
            parserSetting.Applications = applicationDropDown;

            var result = new ClusterMainModel()
            {
                Applications = applicationsExtended,
                Input = inputModel,
                Additional = additional,
                Version = version,
                ParserSettings = parserSettings,
                ParserSetting = parserSetting,
            };

            return result;
        }
    }
}
