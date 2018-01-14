using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Data.Interfaces.Dictionaries;
using Mihaylov.Data.Models.Dictionaries;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Interfaces.Dictionaries
{
    public interface IPrepositionTypesRepository : IGetAllRepository<PrepositionType>, IRepository<DAL.PrepositionType>
    {
    }
}
