using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface IAnswerTypesManager
    {
        IEnumerable<AnswerType> GetAllAnswerTypes();

        AnswerType GetById(int id);

        AnswerType GetByName(string name);
    }
}
