using System.Threading.Tasks;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IVeleroClient
    {
        Task<string> GetVersionAsync();

        Task<string> CreateBackupAsync(string scheduleName);

        Task<string> DeleteBackupAsync(string backupName);
    }
}