using System.Collections.Generic;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Core.Interfaces.Dictionaries
{
    public interface IRecordsManager : IRecordsProvider
    {
        IEnumerable<PrepositionType> GetAllPrepositionTypes(int languageId);
    }
}
