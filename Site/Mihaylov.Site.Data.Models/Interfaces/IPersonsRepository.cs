using System.Collections.Generic;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Data.Interfaces
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