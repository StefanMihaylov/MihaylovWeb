using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IPersonsProvider
    {
        IEnumerable<Person> GetAll(bool descOrder = false, int? pageNumber = null, int? pageSize = null);

        Person GetById(int id);

        Person GetByName(string name);

        PersonStatistics GetStatictics();
    }
}