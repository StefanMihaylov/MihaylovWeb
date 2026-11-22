using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface INugetClient
    {
        Task<IEnumerable<DependencyNode>> GetMetadataAsync(string repositoryUrl, CancellationToken ct);

        Task<DependencyNode> GetMetadataByIdAsync(string repositoryUrl, string packageName, string version, 
            string framework, CancellationToken ct);
    }
}
