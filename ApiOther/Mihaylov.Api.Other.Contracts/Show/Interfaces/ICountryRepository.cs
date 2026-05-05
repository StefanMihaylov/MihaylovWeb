using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Base.Models;
using Mihaylov.Api.Other.Contracts.Show.Models;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface ICountryRepository
    {
        Task<Country> AddOrUpdateAsync(Country model);

        Task<Grid<CountryExtended>> GetAllAsync(GridRequest request);
    }
}