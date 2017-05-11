using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface IAnswerTypesProvider
    {
        IEnumerable<AnswerType> GetAllAnswerTypes();
    }
}