using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Repositories
{
    public interface ICollectionRepository
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();

        Task<Country> GetCountryByNameAsync(string name);

        Task<IDictionary<int, IEnumerable<CountryState>>> GetAllStatesAsync();

        Task<IEnumerable<Ethnicity>> GetAllEthnicitiesAsync();

        Task<IEnumerable<Orientation>> GetAllOrientationsAsync();

        Task<IEnumerable<AccountType>> GetAllAccountTypesAsync();

        Task<AccountType> AddAccountTypeAsync(AccountType imput);

        Task<IEnumerable<AccountStatus>> GetAllAccountStatesAsync();
    }
}
