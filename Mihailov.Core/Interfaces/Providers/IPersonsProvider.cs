using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface IPersonsProvider
    {
        IEnumerable<Person> GetAll();

        Person GetById(int id);

        Person GetByName(string name);
    }
}