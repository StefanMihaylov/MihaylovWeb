using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Show.Models;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Contracts.Show.Interfaces
{
    public interface ICountryRepository
    {
        Task<Country> AddOrUpdateAsync(Country model);

        Task<BaseGrid<CountryExtended>> GetAllAsync(BaseGridRequest request);
    }
}