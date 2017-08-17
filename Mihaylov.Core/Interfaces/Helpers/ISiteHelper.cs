using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface ISiteHelper
    {
        string GetUserName(string url);

        Person GetUserInfo(string username);

        void AddAdditionalInfo(Person person);

        IEnumerable<Unit> GetAllUnits();

        IEnumerable<AnswerType> GetAllAnswerTypes();

        void UpdatePersons();
    }
}