using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface ICountriesProvider
    {
        IEnumerable<Country> GetAll();

        Country GetById(int id);

        Country GetByName(string name);
    }
}