using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Repositories
{
    public interface ILocationsRepository
    {
        Task<Country> AddCountryAsync(Country inputCountry);

        Task<IEnumerable<Country>> GetAllCountriesAsync();

        Task<Country> GetCountryByIdAsync(int id);

        Task<Country> GetCountryByNameAsync(string name);

        Task<IDictionary<int, IEnumerable<CountryState>>> GetAllStatesAsync();
    }
}
