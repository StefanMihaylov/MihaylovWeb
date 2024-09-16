using System.Collections.Generic;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Contracts.Managers
{
    public interface ICollectionsManager
    {
        IEnumerable<AccountStatus> GetAllAnswerTypes();
        AccountStatus GetAnswerTypeById(int id);
        AccountStatus GetAnswerTypeByName(string name);

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

        IEnumerable<CountryState> GetAllStatesByCountryId(int countryId);
    }
}
