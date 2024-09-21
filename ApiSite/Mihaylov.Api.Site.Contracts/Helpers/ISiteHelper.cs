using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Helpers
{
    public interface ISiteHelper
    {
        string GetUserName(string url);

        Task<Person> GetUserInfoAsync(string username);

        void AddAdditionalInfo(Person person);

        Task<int> UpdatePersonsAsync();

       // Task<PersonExtended> GetPersonByNameAsync(string userName);
    }
}
