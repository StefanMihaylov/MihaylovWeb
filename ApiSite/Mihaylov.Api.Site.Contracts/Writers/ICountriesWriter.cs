using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Writers
{
    public interface ICountriesWriter
    {
        Task<Country> AddAsync(Country inputCountry);

        Task<Country> AddAsync(string countryName);
    }
}
