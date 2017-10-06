using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IAnswerTypesManager
    {
        IEnumerable<AnswerType> GetAllAnswerTypes();

        AnswerType GetById(int id);

        AnswerType GetByName(string name);
    }
}
