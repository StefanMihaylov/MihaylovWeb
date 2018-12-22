using System;
using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class Record
    {
        public Record()
        {
            IncorrectAnswers = new HashSet<IncorrectAnswer>();
            RecordRecordTypes = new HashSet<RecordRecordType>();
        }

        public int RecordId { get; set; }
        public int CourseId { get; set; }
        public int? ModuleNumber { get; set; }
        public string Original { get; set; }
        public string Translation { get; set; }
        public string Comment { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<IncorrectAnswer> IncorrectAnswers { get; set; }
        public virtual ICollection<RecordRecordType> RecordRecordTypes { get; set; }
    }
}
