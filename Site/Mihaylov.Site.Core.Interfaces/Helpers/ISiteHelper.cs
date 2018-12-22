using System.Collections.Generic;
using Mihaylov.Site.Core.Models;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface ISiteHelper
    {
        string GetUserName(string url);

        Person GetUserInfo(string username);

        void AddAdditionalInfo(Person person);

        IEnumerable<Unit> GetAllUnits();

        IEnumerable<AnswerType> GetAllAnswerTypes();

        string GetSystemUnit();

        int UpdatePersons();

        PersonExtended GetPersonByName(string userName);
    }
}