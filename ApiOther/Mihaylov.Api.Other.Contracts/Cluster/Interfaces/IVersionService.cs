using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Version;

namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IVersionService
    {
        Task<LastVersionModel> GetLastVersionAsync(int applicationId, bool? reload);

        Task<LastVersionModel> GetLastVersionOnlineAsync(int applicationId);

        Task<LastVersionModel> TestLastVersionAsync(LastVersionSettings configuration);
    }
}
