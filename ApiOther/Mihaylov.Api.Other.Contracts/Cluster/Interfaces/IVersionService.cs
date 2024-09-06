using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IVersionService
    {
        Task<LastVersionModel> GetLastVersionAsync(int applicationId);

        Task<LastVersionModel> GetLastVersionOnlineAsync(int applicationId);

        Task<LastVersionModel> TestLastVersionAsync(LastVersionSettings configuration);

        void Reload(int applicationId);
    }
}
