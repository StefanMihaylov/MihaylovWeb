using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface INexusApiService
    {
        Task<NexusImages> GetDockerImagesAsync();

        Task<NugetPackages> GetNugetPackagesAsync();

        Task<DependencyNode> GetNugetPackageAsync(string packageName, string version, string framework);

        Task ClearImages();

        Task<bool> DeleteDockerImageAsync(string imageId);

        Task<NexusBlobs> GetBlobsAsync();

        Task RunTasksAsync();
    }
}
