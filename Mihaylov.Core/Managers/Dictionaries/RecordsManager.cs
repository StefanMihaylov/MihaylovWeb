using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Dictionaries;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Core.Managers.Dictionaries
{
    public class RecordsManager : IRecordsManager
    {
        private readonly IRecordsProvider recordsProvider;

        private readonly Lazy<ConcurrentDictionary<int, IEnumerable<PrepositionType>>> prepositionTypesByLanguageId;
        private readonly Lazy<ConcurrentDictionary<int, RecordType>> recordTypesById;

        public RecordsManager(IRecordsProvider recordsProvider)
        {
            this.recordsProvider = recordsProvider;

            this.prepositionTypesByLanguageId = new Lazy<ConcurrentDictionary<int, IEnumerable<PrepositionType>>>(() =>
            {
                var prepositionTypes = this.recordsProvider.GetAllPrepositionTypes()
                                                           .GroupBy(k => k.LanguageId)
                                                           .ToDictionary(k => k.Key, v => (IEnumerable<PrepositionType>)v);

                return new ConcurrentDictionary<int, IEnumerable<PrepositionType>>(prepositionTypes);
            });

            this.recordTypesById = new Lazy<ConcurrentDictionary<int, RecordType>>(() =>
            {
                var recordTypes = this.recordsProvider.GetAllRecordTypes().ToDictionary(k => k.Id, v => v);
                return new ConcurrentDictionary<int, RecordType>(recordTypes);
            });
        }

        public Record AddRecord(Record inputRecord)
        {
            Record record = this.recordsProvider.AddRecord(inputRecord);
            return record;
        }

        public IEnumerable<Record> GetRecords(RecordSearchModel searchModel)
        {
            IEnumerable<Record> records = this.recordsProvider.GetRecords(searchModel);
            return records;
        }

        public IEnumerable<PrepositionType> GetAllPrepositionTypes()
        {
            IEnumerable<PrepositionType> prepositionTypes = this.recordsProvider.GetAllPrepositionTypes();
            return prepositionTypes;
        }

        public IEnumerable<PrepositionType> GetAllPrepositionTypes(int languageId)
        {
            if (this.prepositionTypesByLanguageId.Value.TryGetValue(languageId, out IEnumerable<PrepositionType> prepositionTypes))
            {
                return prepositionTypes;
            }
            else
            {
                throw new ApplicationException($"The prepositions for LanguageId {languageId} does not exist.");
            }
        }

        public IEnumerable<RecordType> GetAllRecordTypes()
        {
            IEnumerable<RecordType> recordTypes = this.recordTypesById.Value.Values;
            return recordTypes;
        }
    }
}
