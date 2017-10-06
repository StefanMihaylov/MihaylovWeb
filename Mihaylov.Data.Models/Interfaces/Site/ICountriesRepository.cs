using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Data.Interfaces.Site
{
    public interface ICountriesRepository
    {
        Country AddCountry(Country inputCountry);

        IEnumerable<Country> GetAll();

        Country GetById(int id);

        Country GetByName(string name);
    }
}