using System.Collections.Generic;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Data.Models.Dictionaries
{
    public class RecordSearchModel
    {
        public int? Id { get; set; }

        public int? CourseId { get; set; }

        public int? ModuleNumber { get; set; }

        public ICollection<RecordType> RecordTypes { get; set; }
    }
}
