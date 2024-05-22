using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Base.Models;
using Mihaylov.Api.Other.Contracts.Show.Models;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface ITicketProviderRepository
    {
        Task<Grid<TicketProvider>> GetAllAsync(GridRequest request);

        Task<TicketProvider> AddOrUpdateAsync(TicketProvider model);
    }
}