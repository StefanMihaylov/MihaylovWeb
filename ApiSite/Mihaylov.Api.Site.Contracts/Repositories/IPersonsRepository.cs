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

       // Task<IEnumerable<Person>> GetAllForUpdateAsync();

        Task<Person> GetPersonAsync(long id);

        Task<Person> AddOrUpdatePersonAsync(Person input);

        Task DeletePersonAsync(long id);

        Task<PersonStatistics> GetStaticticsAsync();

        Task<Account> GetAccountAsync(long id);

        Task<Account> AddOrUpdateAccountAsync(Account input);
    }
}
