using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Repositories
{
    public interface IPersonsRepository
    {
        Task<IEnumerable<Person>> Search(bool descOrder = false, int? pageNumber = null, int? pageSize = null);

        Task<IEnumerable<Person>> GetAllAsync();

        Task<IEnumerable<Person>> GetAllForUpdateAsync();

        Task<Person> GetByIdAsync(long id);

        Task<Person> GetByAccoutUserNameAsync(string username);

        Task<Person> AddOrUpdatePersonAsync(Person inputPerson);

        Task<PersonStatistics> GetStaticticsAsync();
    }
}
