using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Writers.Site
{
    public class CountriesWriter : ICountriesWriter
    {
        private readonly ICountriesRepository repository;

        public CountriesWriter(ICountriesRepository countryRepository)
        {
            this.repository = countryRepository;
        }

        public Country Add(Country inputCountry)
        {
            Country country = this.repository.AddCountry(inputCountry);
            return country;
        }

        public Country Add(string countryName)
        {
            Country newCountry = new Country() { Name = countryName.Trim() };
            Country country = this.Add(newCountry);
            return country;
        }
    }
}
