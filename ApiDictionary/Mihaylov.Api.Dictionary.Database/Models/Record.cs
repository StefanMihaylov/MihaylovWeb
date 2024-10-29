using System.Collections.Generic;

namespace Mihaylov.Api.Dictionary.Database.Models
{
    public class Record
    {
        public int RecordId { get; set; }

        public int CourseId { get; set; }

        public int? ModuleNumber { get; set; }

        public int RecordTypeId { get; set; }

        public string Original { get; set; }

        public string Translation { get; set; }

        public string Comment { get; set; }

        public int? PrepositionId { get; set; }


        public Course Course { get; set; }

        public RecordType RecordType { get; set; }

        public Preposition Preposition { get; set; }
    }
}