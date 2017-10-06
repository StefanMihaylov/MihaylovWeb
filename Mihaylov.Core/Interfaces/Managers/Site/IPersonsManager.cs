using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IPersonsManager
    {
        IEnumerable<Person> GetAllPersons(bool descOrder = false, int? pageNumber = null, int? pageSize = null);

        Person GetById(int id);

        Person GetByName(string name);

        PersonStatistics GetStatictics();
    }
}