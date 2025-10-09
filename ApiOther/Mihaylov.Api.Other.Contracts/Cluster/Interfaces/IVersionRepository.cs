using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IVersionRepository
    {
        Task<IEnumerable<VersionHistory>> GetVersionsAsync(int applicationId, int size);

        Task<AppVersion> AddOrUpdateAsync(AppVersion model, int applicationId);
    }
}