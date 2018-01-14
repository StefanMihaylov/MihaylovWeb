using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Data.Models.Dictionaries;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Interfaces.Dictionaries
{
    public interface IRecordTypesRepository : IGetAllRepository<RecordType>, IRepository<DAL.RecordType>
    {
    }
}
