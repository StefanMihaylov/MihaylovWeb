using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface INexusApiService
    {
        Task<NexusImages> GetDockerImagesAsync();

        Task ClearImages();

        Task<bool> DeleteDockerImageAsync(string imageId);

        Task<NexusBlobs> GetBlobsAsync();

        Task RunTasksAsync();
    }
}
