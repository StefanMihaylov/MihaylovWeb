using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface ICountriesProvider
    {
        Country AddCountry(Country inputCountry);

        IEnumerable<Country> GetAll();

        Country GetById(int id);
    }
}