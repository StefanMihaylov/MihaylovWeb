using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IClusterService
    {
        Task<IEnumerable<ApplicationExtended>> GetAllApplicationsAsync();

        Task<ApplicationExtended> AddOrUpdateApplicationAsync(Application model);

        Task<DeploymentFile> AddOrUpdateFileAsync(DeploymentFile model, int applicationId);

        Task<Pod> AddOrUpdatePodAsync(Pod model, int applicationId);

        Task<AppVersion> AddOrUpdateVersionAsync(AppVersion model, int applicationId);
    }
}