using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface ICountriesWriter
    {
        Country Add(Country inputCountry);

        Country Add(string countryName);
    }
}