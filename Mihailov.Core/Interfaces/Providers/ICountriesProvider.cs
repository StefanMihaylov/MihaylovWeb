using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface ICountriesProvider
    {
        IEnumerable<Country> GetAll();

        Country GetById(int id);

        Country GetByName(string name);
    }
}