using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IKubernetesHelper
    {
        Task<IEnumerable<KubeNamespace>> GetNamespacesAsync();

        Task<IEnumerable<StorageClass>> GetStorageClassesAsync();

        Task<IEnumerable<PersistentVolumeClaim>> GetPersistanceVolumeClaimsAsync(string namespaceName);

        Task<IEnumerable<PersistentVolume>> GetPersistanceVolumesAsync();

        Task<IEnumerable<CustomResource>> GetCustomResourcesAsync();

        Task<IEnumerable<KubernetesSchedule>> GetVeleroSchedulesAsync();

        Task<IEnumerable<KubernetesBackup>> GetVeleroBackupsAsync();

        Task<IEnumerable<DataUploadModel>> GetDataUploadsAsync();
    }
}
