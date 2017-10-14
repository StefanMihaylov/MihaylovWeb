using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IPersonAdditionalInfoManager: IPersonAdditionalInfoProvider
    {
        AnswerType GetAnswerTypeById(int id);
        AnswerType GetAnswerTypeByName(string name);

        Ethnicity GetEthnicityById(int id);
        Ethnicity GetEthnicityByName(string name);

        Orientation GetOrientationById(int id);
        Orientation GetOrientationByName(string name);

        Unit GetUnitById(int id);
        Unit GetUnitByName(string name);
    }
}