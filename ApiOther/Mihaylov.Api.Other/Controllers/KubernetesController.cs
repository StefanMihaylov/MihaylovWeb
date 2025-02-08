using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes;
using Mihaylov.Api.Other.Extensions;
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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class KubernetesController : ControllerBase
    {
        private readonly IKubernetesHelper _client;

        public KubernetesController(IKubernetesHelper client)
        {
            _client = client;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<KubeNamespace>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Namespaces()
        {
            IEnumerable<KubeNamespace> namespaces = await _client.GetNamespacesAsync().ConfigureAwait(false);

            return Ok(namespaces);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StorageClass>), StatusCodes.Status200OK)]
        public async Task<IActionResult> StorageClasses()
        {
            IEnumerable<StorageClass> storage = await _client.GetStorageClassesAsync().ConfigureAwait(false);

            return Ok(storage);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersistentVolumeClaim>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PersistentVolumeClaims([FromQuery] string namespaceName)
        {
            IEnumerable<PersistentVolumeClaim> pvcs = await _client.GetPersistanceVolumeClaimsAsync(namespaceName).ConfigureAwait(false);

            return Ok(pvcs);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersistentVolume>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PersistentVolumes()
        {
            IEnumerable<PersistentVolume> pvs = await _client.GetPersistanceVolumesAsync().ConfigureAwait(false);

            return Ok(pvs);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomResource>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CustomResources()
        {
            IEnumerable<CustomResource> crds = await _client.GetCustomResourcesAsync().ConfigureAwait(false);

            return Ok(crds);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<KubernetesSchedule>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VeleroSchedules()
        {
            IEnumerable<KubernetesSchedule> schedules = await _client.GetVeleroSchedulesAsync().ConfigureAwait(false);

            return Ok(schedules);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<KubernetesBackup>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VeleroBackups()
        {
            IEnumerable<KubernetesBackup> backups = await _client.GetVeleroBackupsAsync().ConfigureAwait(false);

            return Ok(backups);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DataUploadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VeleroDataUploads()
        {
            IEnumerable<DataUploadModel> uploads = await _client.GetDataUploadsAsync().ConfigureAwait(false);

            return Ok(uploads);
        }
    }
}
