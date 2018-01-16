using System.Collections.Generic;
using Mihaylov.Core.Models.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
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