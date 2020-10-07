using System.Threading.Tasks;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Writers
{
    public class CountriesWriter : ICountriesWriter
    {
        private readonly ILocationsRepository repository;

        public CountriesWriter(ILocationsRepository locationsRepository)
        {
            this.repository = locationsRepository;
        }

        public async Task<Country> AddAsync(Country inputCountry)
        {
            Country country = await this.repository.AddCountryAsync(inputCountry).ConfigureAwait(false);
            return country;
        }

        public async Task<Country> AddAsync(string countryName)
        {
            Country newCountry = new Country() { Name = countryName.Trim() };
            Country country = await this.AddAsync(newCountry).ConfigureAwait(false);
            return country;
        }
    }
}
