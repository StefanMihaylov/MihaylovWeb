using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Managers
{
    public interface ICollectionManager
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();

        Task<Country> GetCountryByNameAsync(string name);

        Task<IEnumerable<CountryState>> GetAllStatesByCountryIdAsync(int countryId);

        Task<IEnumerable<Ethnicity>> GetAllEthnicitiesAsync();

        Task<IEnumerable<Orientation>> GetAllOrientationsAsync();

        Task<IEnumerable<AccountType>> GetAllAccountTypesAsync();

        Task<IEnumerable<AccountStatus>> GetAllAccountStatesAsync();
    }
}
