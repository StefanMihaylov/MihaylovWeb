using System.Collections.Generic;

namespace Mihaylov.Api.Dictionary.Contracts.Models
{
    public class RecordSearchModel
    {
        public int? Id { get; set; }

        public int? CourseId { get; set; }

        public int? ModuleNumber { get; set; }

        public ICollection<RecordType> RecordTypes { get; set; }
    }
}
