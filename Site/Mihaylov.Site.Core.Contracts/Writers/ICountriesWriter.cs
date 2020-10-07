using System.Threading.Tasks;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface ICountriesWriter
    {
        Task<Country> AddAsync(Country inputCountry);

        Task<Country> AddAsync(string countryName);
    }
}