using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IFileRepository
    {
        Task<DeploymentFile> AddOrUpdateAsync(DeploymentFile model, int applicationId);
    }
}