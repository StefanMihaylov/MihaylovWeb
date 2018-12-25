using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Dictionaries.Database.Interfaces;

namespace Mihaylov.Dictionaries.Data.Interfaces
{
    public interface IRecordsData : IUnitOfWork<IDictionariesDbContext>
    {
        IRecordsRepository Records { get; }

        IRecordTypesRepository RecordTypes { get; }

        IPrepositionTypesRepository PrepositionTypes { get; }
    }
}