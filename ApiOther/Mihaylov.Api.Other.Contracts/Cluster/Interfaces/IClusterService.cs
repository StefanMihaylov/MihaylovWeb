using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Cluster;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IClusterService
    {
        Task<IEnumerable<ApplicationExtended>> GetAllApplicationsAsync();

        Task<ApplicationExtended> AddOrUpdateApplicationAsync(Application model);

        Task<DeploymentFile> AddOrUpdateFileAsync(DeploymentFile model, int applicationId);

        Task<Pod> AddOrUpdatePodAsync(Pod model, int applicationId);

        Task<IEnumerable<VersionHistory>> GetVersionsAsync(int applicationId, int size);

        Task<AppVersion> AddOrUpdateVersionAsync(AppVersion model, int applicationId);

        Task<IEnumerable<ParserSetting>> GetParserSettingsAsync();

        Task<ParserSetting> AddOrUpdateParserSettingAsync(ParserSetting model);
    }
}