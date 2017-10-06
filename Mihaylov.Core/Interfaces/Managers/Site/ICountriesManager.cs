using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface ICountriesManager
    {
       // IEnumerable<Country> GetAllCountries();

        Country GetById(int id);

        Country GetByName(string name);
    }
}