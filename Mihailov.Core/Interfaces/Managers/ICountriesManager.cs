using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface ICountriesManager
    {
        IEnumerable<Country> GetAllCountries();

        Country GetById(int id);

        Country GetByName(string name);
    }
}