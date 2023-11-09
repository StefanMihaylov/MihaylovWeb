using System;
using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class RecordType
    {
        public RecordType()
        {
            RecordRecordTypes = new HashSet<RecordRecordType>();
        }

        public int RecordTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RecordRecordType> RecordRecordTypes { get; set; }
    }
}
