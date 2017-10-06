using System.Collections.Generic;
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

        int UpdatePersons();
    }
}