using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Writers
{
    public interface IPersonsWriter
    {
        Task<Person> AddOrUpdatePersonAsync(Person input, int? age);

        Task<Person> MergePersonsAsync(PersonMerge input);

        Task<Account> AddOrUpdateAccountAsync(Account input, int? age);

        Task<Person> AddNewPersonAsync(Person input, int? age);
    }
}
