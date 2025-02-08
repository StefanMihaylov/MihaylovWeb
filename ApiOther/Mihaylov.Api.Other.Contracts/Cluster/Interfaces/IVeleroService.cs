using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Velero;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IVeleroService
    {
        Task<string> GetExeVersionAsync();

        Task<ScheduleResponse> GetSchedulesAsync();

        Task<string> CreateBackupAsync(string scheduleName);

        Task<string> DeleteBackupAsync(string backupName);
    }
}