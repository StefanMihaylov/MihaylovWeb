using System;
using System.Collections.Generic;
using Mihaylov.Common.Database;
using Mihaylov.Data.Repositories.Dictionaries;
using Mihaylov.Dictionaries.Data.Interfaces;
using Mihaylov.Dictionaries.Data.Models;
using Mihaylov.Dictionaries.Database.Interfaces;

namespace Mihaylov.Data.UnitOfWorks
{
    public class RecordsData : BaseUnitOfWork<IDictionariesDbContext>, IRecordsData
    {
        public RecordsData(IDictionariesDbContext context)
            : base(context)
        {
        }

        public IRecordsRepository Records
        {
            get { return (RecordsRepository)this.GetRepository<Record>(); }
        }

        public IRecordTypesRepository RecordTypes
        {
            get { return (RecordTypesRepository)this.GetRepository<RecordType>(); }
        }

        public IPrepositionTypesRepository PrepositionTypes
        {
            get { return (PrepositionTypesRepository)this.GetRepository<PrepositionType>(); }
        }

        protected override void AddRepositoryTypes(IDictionary<Type, Type> repositoryTypes)
        {
            repositoryTypes.Add(typeof(Record), typeof(RecordsRepository));
            repositoryTypes.Add(typeof(RecordType), typeof(RecordTypesRepository));
            repositoryTypes.Add(typeof(PrepositionType), typeof(PrepositionTypesRepository));
        }
    }
}
