using System.Collections.Generic;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface IPersonAdditionalInfoManager
    {
        IEnumerable<AnswerType> GetAllAnswerTypes();
        AnswerType GetAnswerTypeById(int id);
        AnswerType GetAnswerTypeByName(string name);

        IEnumerable<Ethnicity> GetAllEthnicities();
        Ethnicity GetEthnicityById(int id);
        Ethnicity GetEthnicityByName(string name);

        IEnumerable<Orientation> GetAllOrientations();
        Orientation GetOrientationById(int id);
        Orientation GetOrientationByName(string name);

        IEnumerable<AccountType> GetAllAccountTypes();
        AccountType GetAccountTypeById(int id);
        AccountType GetAccountTypeByName(string name);

        IEnumerable<Unit> GetAllUnits();
        Unit GetUnitById(int id);
        Unit GetUnitByName(string name);

        IEnumerable<Country> GetAllCountries();
        Country GetCountryById(int id);
        Country GetCountryByName(string name);

        IEnumerable<State> GetAllStates();
    }
}