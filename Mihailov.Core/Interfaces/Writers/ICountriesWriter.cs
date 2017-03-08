using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface ICountriesWriter
    {
        Country Add(Country inputCountry);

        Country Add(string countryName);
    }
}