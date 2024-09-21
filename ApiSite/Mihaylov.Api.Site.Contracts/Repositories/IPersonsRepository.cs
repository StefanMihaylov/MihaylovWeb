using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;

namespace Mihaylov.Api.Site.Contracts.Repositories
{
    public interface IPersonsRepository
    {
        Task<Grid<Person>> GetAllPersonsAsync(GridRequest request);

        Task<IEnumerable<Person>> GetAllForUpdateAsync();

        Task<Person> GetByIdAsync(long id);

        Task<Person> GetByAccoutUserNameAsync(string username);

        Task<Person> AddOrUpdatePersonAsync(Person inputPerson);

        Task<PersonStatistics> GetStaticticsAsync();
    }
}
