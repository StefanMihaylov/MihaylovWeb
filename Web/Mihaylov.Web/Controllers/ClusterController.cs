using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Other.Client;
using Mihaylov.Web.Areas;
using Mihaylov.Web.Models.Cluster;

namespace Mihaylov.Web.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            ClusterMainModel result = await FillModelAsync(null, null, null).ConfigureAwait(false);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddApp(AddApplicationModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ClusterMainModel viewModel = await FillModelAsync(inputModel, null, null).ConfigureAwait(false);
                return View(nameof(Index), viewModel);
            }

            var model = new ApplicationModel()
            {
                Name = inputModel?.Name,
                ReleaseUrl = inputModel?.ReleaseUrl,
                ResourceUrl = inputModel?.ResourceUrl,
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
                ClusterMainModel viewModel = await FillModelAsync(null, inputModel, null).ConfigureAwait(false);
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
                ClusterMainModel viewModel = await FillModelAsync(null, null, inputModel).ConfigureAwait(false);
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

        private async Task<ClusterMainModel> FillModelAsync(AddApplicationModel inputModel, AddAdditionalModel additional, AddVersionModel version)
        {
            _client.AddToken(Request.GetToken());
            var applications = await _client.ApplicationsAsync().ConfigureAwait(false);

            var deploymentTypes = Enum.GetValues(typeof(DeploymentType))
                      .Cast<DeploymentType>()
                      .Select(v => new SelectListItem(v.ToString(), ((int)v).ToString()))
                      .ToList();

            var applicationDropDown = applications.Select(v => new SelectListItem(v.Name, v.Id.ToString())).ToList();

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

            inputModel.DeploymentTypes = deploymentTypes;
            additional.Applications = applicationDropDown;
            version.Applications = applicationDropDown;

            var result = new ClusterMainModel()
            {
                Applications = applications,
                Input = inputModel,
                Additional = additional,
                Version = version,
            };

            return result;
        }
    }
}
