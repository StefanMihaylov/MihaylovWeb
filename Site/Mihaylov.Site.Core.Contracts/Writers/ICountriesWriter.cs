using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface ICountriesWriter
    {
        Country Add(Country inputCountry);

        Country Add(string countryName);
    }
}