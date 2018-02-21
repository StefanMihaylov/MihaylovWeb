using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Data.Interfaces.Site
{
    public interface IPersonsRepository
    {
        IEnumerable<Person> GetAll();

        Person GetById(int id);

        Person GetByName(string username);

        Person AddOrUpdatePerson(Person inputPerson, out bool isNewPerson);

        PersonStatistics GetStatictics();
    }
}