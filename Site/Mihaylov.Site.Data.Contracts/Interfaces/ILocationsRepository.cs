using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Data.Interfaces
{
    public interface ILocationsRepository
    {
        Task<Country> AddCountryAsync(Country inputCountry);

        Task<IEnumerable<Country>> GetAllCountriesAsync();

        Task<Country> GetCountryByIdAsync(int id);

        Task<Country> GetCountryByNameAsync(string name);

        Task<IDictionary<int, IEnumerable<State>>> GetAllStatesAsync();
    }
}