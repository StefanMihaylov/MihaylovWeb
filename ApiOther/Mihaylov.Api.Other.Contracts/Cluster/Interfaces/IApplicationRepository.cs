using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IApplicationRepository
    {
        Task<IEnumerable<ApplicationExtended>> GetAllAsync();

        Task<ApplicationExtended> AddOrUpdateAsync(Application model);
    }
}