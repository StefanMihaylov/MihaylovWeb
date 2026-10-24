using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mihaylov.Api.Other.Client;
using Mihaylov.Common;
using Mihaylov.UI.Mvc.Other.Areas.Cluster.Models.Cluster;

namespace Mihaylov.UI.Mvc.Other.Areas.Cluster.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Area("Cluster")]
    public class ClusterController(IOtherApiClient client) : Controller
    {
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
                Order = inputModel.Order,
                Name = inputModel?.Name,
                ReleaseUrl = inputModel?.ReleaseUrl,
                ResourceUrl = inputModel?.ResourceUrl,
                SiteUrl = inputModel?.SiteUrl,
                GithubVersionUrl = inputModel?.GithubVersionUrl,
                Deployment = inputModel?.Deployment ?? DeploymentType.Yaml,
                Notes = inputModel?.Notes,
                ParserSettingId = inputModel?.ParserSettingId,
            };

            var applications = await client.ApplicationAsync(model).ConfigureAwait(false);

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

            if (!string.IsNullOrWhiteSpace(inputModel.File) && inputModel.ApplicationId.HasValue)
            {
                var model = new DeploymentFileModel()
                {
                    ApplicationId = inputModel.ApplicationId.Value,
                    Name = inputModel?.File,
                };

                var files = await client.FileAsync(model).ConfigureAwait(false);
            }

            if (!string.IsNullOrWhiteSpace(inputModel.Pod) && inputModel.ApplicationId.HasValue)
            {
                var model = new PodModel()
                {
                    ApplicationId = inputModel.ApplicationId.Value,
                    Name = inputModel?.Pod,
                };

                var files = await client.PodAsync(model).ConfigureAwait(false);
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

            var model = new AppVersionModel()
            {
                ApplicationId = inputModel.ApplicationId.Value,
                ReleaseDate = inputModel?.ReleaseDate.Value ?? DateTime.UtcNow.Date,
                Version = inputModel?.Version,
                HelmVersion = inputModel?.HelmVersion,
                HelmAppVersion = inputModel?.HelmAppVersion,
            };

            var files = await client.VersionAsync(model).ConfigureAwait(false);

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
                Name = inputModel.Name,
                ApplicationId = inputModel.ApplicationId,
                VersionUrlType = inputModel.VersionUrlType.Value,
                VersionSelector = inputModel.VersionSelector,
                VersionCommand = inputModel.VersionCommand,
                ReleaseDateUrlType = inputModel.ReleaseDateUrlType,
                ReleaseDateSelector = inputModel.ReleaseDateSelector,
                ReleaseDateCommand = inputModel.ReleaseDateCommand,
            };

            var setting = await client.ParserSettingAsync(model).ConfigureAwait(false);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetLastVersion(int id, bool? reload)
        {
            var applications = await client.ApplicationsAsync().ConfigureAwait(false);
            var application = applications.FirstOrDefault(a => a.Id == id);

            LastVersionModel lastVersion = null;
            VersionIconType icon;
            if (!application.ParserSettingId.HasValue)
            {
                icon = VersionIconType.None;
            }
            else
            {
                var lastVersionResponse = await client.LastVersionAsync(id, reload).ConfigureAwait(false);
                lastVersion = lastVersionResponse.LastVersion;
                if (lastVersion == null || !lastVersion.IsSuccessful)
                {
                    icon = VersionIconType.Error;
                }
                else
                {
                    var isLastVersion = false;
                    if (lastVersion.IsSuccessful && application.Version != null)
                    {
                        isLastVersion = application.Version.Version == lastVersion.Version;

                        if (!isLastVersion)
                        {
                            try
                            {
                                var appVersion = new Version(application.Version.Version);
                                var lastVersionModel = new Version(lastVersion.Version);

                                if (appVersion > lastVersionModel)
                                {
                                    isLastVersion = true;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    icon = isLastVersion ? VersionIconType.OK : VersionIconType.New;
                }
            }

            var viewModel = new ApplicationViewModel()
            {
                Main = application,
                LastVersion = lastVersion,
                Icon = icon,
            };

            return PartialView("_Application", viewModel);
        }

        private async Task<ClusterMainModel> FillModelAsync(int? id, int? settingId, AddApplicationModel inputModel,
            AddAdditionalModel additional, AddVersionModel version, AddParserSettingModel parserSetting)
        {
            var applications = await client.ApplicationsAsync().ConfigureAwait(false);

            var deploymentTypes = Enum.GetValues(typeof(DeploymentType))
                      .Cast<DeploymentType>()
                      .Select(v => new SelectListItem(v.ToString(), ((int)v).ToString()))
                      .ToList();

            var applicationDropDown = applications.Select(v => new SelectListItem(v.Name, v.Id.ToString())).ToList();

            var UrlTypes = Enum.GetValues(typeof(VersionUrlType))
                      .Cast<VersionUrlType>()
                      .Select(v => new SelectListItem(v.ToString(), ((int)v).ToString()))
                      .ToList();

            var parserSettings = await client.ParserSettingsAsync().ConfigureAwait(false);
            var parserSettingsDropDown = parserSettings.Where(p => !string.IsNullOrWhiteSpace(p.Name))
                                            .OrderBy(p => p.Name)
                                            .Select(v => new SelectListItem(v.Name, v.Id.ToString()))
                                            .ToList();

            var applicationsExtended = applications.Select(application =>
            {
                return new ApplicationViewModel()
                {
                    Main = application,
                    LastVersion = null,
                    Icon = VersionIconType.Unknown,
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
                        Order = app.Order,
                        Name = app.Name,
                        Deployment = app.Deployment,
                        SiteUrl = app.SiteUrl,
                        ReleaseUrl = app.ReleaseUrl,
                        ResourceUrl = app.ResourceUrl,
                        GithubVersionUrl = app.GithubVersionUrl,
                        Notes = app.Notes,
                        ParserSettingId = app.ParserSettingId,
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
                        Name = settings.Name,
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
            inputModel.ParserSettings = parserSettingsDropDown;
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
