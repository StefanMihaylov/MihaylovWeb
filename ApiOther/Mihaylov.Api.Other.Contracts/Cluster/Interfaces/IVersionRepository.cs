using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IVersionRepository
    {
        Task<AppVersion> AddOrUpdateAsync(AppVersion model, int applicationId);
    }
}