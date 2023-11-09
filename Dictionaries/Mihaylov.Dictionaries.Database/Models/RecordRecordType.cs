using System;
using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class RecordRecordType
    {
        public int RecordId { get; set; }
        public int RecordTypeId { get; set; }

        public virtual Record Record { get; set; }
        public virtual RecordType RecordType { get; set; }
    }
}
