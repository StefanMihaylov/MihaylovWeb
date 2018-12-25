using System.Collections.Generic;

namespace Mihaylov.Dictionaries.Data.Models
{
    public class RecordSearchModel
    {
        public int? Id { get; set; }

        public int? CourseId { get; set; }

        public int? ModuleNumber { get; set; }

        public ICollection<RecordType> RecordTypes { get; set; }
    }
}
