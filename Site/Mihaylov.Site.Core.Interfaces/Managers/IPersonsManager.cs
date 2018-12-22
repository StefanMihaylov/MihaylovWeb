using System.Collections.Generic;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface IPersonsManager
    {
        IEnumerable<Person> GetAllPersons(bool descOrder = false, int? pageNumber = null, int? pageSize = null);

        Person GetById(int id);

        Person GetByName(string name);

        PersonStatistics GetStatictics();
    }
}