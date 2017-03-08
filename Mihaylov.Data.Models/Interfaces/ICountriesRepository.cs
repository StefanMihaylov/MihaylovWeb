using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Data.Models.Interfaces
{
    public interface ICountriesRepository
    {
        Country AddCountry(Country inputCountry);

        IEnumerable<Country> GetAll();

        Country GetById(int id);

        Country GetByName(string name);
    }
}