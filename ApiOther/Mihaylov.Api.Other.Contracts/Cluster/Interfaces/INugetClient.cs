using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface INugetClient
    {
        Task<IEnumerable<NugetPackage>> GetMetadataAsync(string repositoryUrl, CancellationToken ct);

        Task<NugetPackage> GetMetadataByIdAsync(string repositoryUrl, string packageName, string version, CancellationToken ct);

        bool IsNugetLocal(string packageName);
    }
}
