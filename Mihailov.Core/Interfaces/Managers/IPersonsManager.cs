using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface IPersonsManager
    {
        IEnumerable<Person> GetAllPersons();

        Person GetById(int id);

        Person GetByName(string name);

        PersonStatistics GetStatictics();
    }
}