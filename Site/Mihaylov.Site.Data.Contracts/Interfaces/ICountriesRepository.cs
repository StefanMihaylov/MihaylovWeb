using System.Collections.Generic;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Data.Interfaces
{
    public interface ICountriesRepository
    {
        Country AddCountry(Country inputCountry);

        IEnumerable<Country> GetAll();

        Country GetById(int id);

        Country GetByName(string name);
    }
}