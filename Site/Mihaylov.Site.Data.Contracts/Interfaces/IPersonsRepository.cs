using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Data.Interfaces
{
    public interface IPersonsRepository
    {
        Task<IEnumerable<Person>> Search(bool descOrder = false, int? pageNumber = null, int? pageSize = null);

        Task<IEnumerable<Person>> GetAllAsync();

        Task<IEnumerable<Person>> GetAllForUpdateAsync();

        Task<Person> GetByIdAsync(Guid id);

        Task<Person> GetByAccoutUserNameAsync(string username);

        Task<Person> AddOrUpdatePersonAsync(Person inputPerson);

        Task<PersonStatistics> GetStaticticsAsync();
    }
}