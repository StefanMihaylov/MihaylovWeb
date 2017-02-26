using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface ICountriesManager
    {
        IEnumerable<Country> GetAllUnits();

        Country GetById(int id);

        Country GetByName(string name);
    }
}