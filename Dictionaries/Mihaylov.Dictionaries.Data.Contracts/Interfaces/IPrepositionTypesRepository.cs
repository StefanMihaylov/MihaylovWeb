using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Dictionaries.Data.Models;
using DAL = Mihaylov.Dictionaries.Database.Models;

namespace Mihaylov.Dictionaries.Data.Interfaces
{
    public interface IPrepositionTypesRepository : IGetAllRepository<PrepositionType>, IRepository<DAL.PrepositionType>
    {
    }
}
