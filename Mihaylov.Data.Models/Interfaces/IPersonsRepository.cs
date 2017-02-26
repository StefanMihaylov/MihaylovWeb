using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Data.Models.Interfaces
{
    public interface IPersonsRepository
    {
        IEnumerable<Person> GetAll();

        Person GetById(int id);

        Person AddPerson(Person inputPerson);
    }
}