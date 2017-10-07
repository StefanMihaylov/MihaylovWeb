using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IPersonAdditionalInfoProvider
    {
        IEnumerable<AnswerType> GetAllAnswerTypes();
        IEnumerable<Country> GetAllCountries();
        IEnumerable<Ethnicity> GetAllEthnicities();
        IEnumerable<Orientation> GetAllOrientations();
        IEnumerable<Unit> GetAllUnits();
        Country GetCountryById(int id);
        Country GetCountryByName(string name);
    }
}