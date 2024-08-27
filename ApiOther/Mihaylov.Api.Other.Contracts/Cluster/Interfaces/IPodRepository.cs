using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IPodRepository
    {
        Task<Pod> AddOrUpdateAsync(Pod model, int applicationId);
    }
}