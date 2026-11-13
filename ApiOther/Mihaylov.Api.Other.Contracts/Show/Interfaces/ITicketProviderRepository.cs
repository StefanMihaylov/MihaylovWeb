using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface ITicketProviderRepository
    {
        Task<BaseGrid<TicketProvider>> GetAllAsync(BaseGridRequest request);

        Task<TicketProvider> AddOrUpdateAsync(TicketProvider model);
    }
}