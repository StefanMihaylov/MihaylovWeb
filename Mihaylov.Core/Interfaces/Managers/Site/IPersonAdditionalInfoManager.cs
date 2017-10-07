using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IPersonAdditionalInfoManager
    {
        IEnumerable<AnswerType> GetAllAnswerTypes();
        AnswerType GetAnswerTypeById(int id);
        AnswerType GetAnswerTypeByName(string name);

        Country GetCountryById(int id);
        Country GetCountryByName(string name);

        IEnumerable<Ethnicity> GetAllEthnicities();
        Ethnicity GetEthnicityById(int id);
        Ethnicity GetEthnicityByName(string name);

        IEnumerable<Orientation> GetAllOrientations();
        Orientation GetOrientationById(int id);
        Orientation GetOrientationByName(string name);

        IEnumerable<Unit> GetAllUnits();
        Unit GetUnitById(int id);
        Unit GetUnitByName(string name);
    }
}