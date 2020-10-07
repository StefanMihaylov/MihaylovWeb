using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Site.Core.Models;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface ISiteHelper
    {
        string GetUserName(string url);

        Task<Person> GetUserInfoAsync(string username);

        void AddAdditionalInfo(Person person);

        IEnumerable<Unit> GetAllUnits();

        IEnumerable<AnswerType> GetAllAnswerTypes();

        string GetSystemUnit();

        Task<int> UpdatePersonsAsync();

        Task<PersonExtended> GetPersonByNameAsync(string userName);
    }
}