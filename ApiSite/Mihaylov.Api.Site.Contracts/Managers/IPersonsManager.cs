using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;

namespace Mihaylov.Api.Site.Contracts.Managers
{
    public interface IPersonsManager
    {
        Task<Grid<Person>> GetAllPersonsAsync(GridRequest request);

        Task<Person> GetPersonAsync(long id);

        //Person GetByName(string name);

        Task<Account> GetAccountAsync(long id);

        Task<PersonStatistics> GetStaticticsAsync();
    }
}
