using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Base.Models;
using Mihaylov.Api.Other.Contracts.Show.Models;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface IConcertService
    {
        Task<Grid<ConcertExtended>> GetConcertsAsync(GridRequest request);

        Task<ConcertExtended> AddOrUpdateConcertAsync(Concert model);

        Task<Grid<BandExtended>> GetBandsAsync(GridRequest request);

        Task<Band> AddOrUpdateBandAsync(Band model);

        Task<Grid<Location>> GetLocationsAsync(GridRequest request);

        Task<Location> AddOrUpdateLocationAsync(Location model);

        Task<Grid<TicketProvider>> GetTicketProvidersAsync(GridRequest request);

        Task<TicketProvider> AddOrUpdateTicketProviderAsync(TicketProvider model);
    }
}