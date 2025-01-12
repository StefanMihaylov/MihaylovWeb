using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Nexus.Models;

namespace Mihaylov.Api.Other.Contracts.Nexus.Interfaces
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
