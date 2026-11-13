using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface IConcertService
    {
        Task<BaseGrid<ConcertExtended>> GetConcertsAsync(BaseGridRequest request);

        Task<ConcertExtended> AddOrUpdateConcertAsync(Concert model);

        Task<BaseGrid<BandExtended>> GetBandsAsync(BaseGridRequest request);

        Task<Band> AddOrUpdateBandAsync(Band model);

        Task<BaseGrid<Location>> GetLocationsAsync(BaseGridRequest request);

        Task<Location> AddOrUpdateLocationAsync(Location model);

        Task<BaseGrid<TicketProvider>> GetTicketProvidersAsync(BaseGridRequest request);

        Task<TicketProvider> AddOrUpdateTicketProviderAsync(TicketProvider model);

        Task<BaseGrid<CountryExtended>> GetCountriesAsync(BaseGridRequest request);

        Task<Country> AddOrUpdateCountryAsync(Country model);

        Task<IEnumerable<ConcertType>> GetConcertTypesAsync();

        Task<ConcertType> AddOrUpdateConcertTypeAsync(ConcertType model);
    }
}