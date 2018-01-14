using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Data.Interfaces.Dictionaries;
using Mihaylov.Database.Dictionaries;
using Mihaylov.Database.Interfaces;

namespace Mihaylov.Data.Interfaces
{
    public interface IRecordsData : IUnitOfWork<IDictionariesDbContext>
    {
        IRecordsRepository Records { get; }

        IRecordTypesRepository RecordTypes { get; }

        IPrepositionTypesRepository PrepositionTypes { get; }
    }
}