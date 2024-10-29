using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Api.Dictionary.Models
{
    public class AddRecordModel
    {
        public int? Id { get; set; }

        [Required]
        public int? CourseId { get; set; }

        public int? ModuleNumber { get; set; }

        [Required]
        public int RecordTypeId { get; set; }

        [Required]
        public string Original { get; set; }

        public string Translation { get; set; }

        public string Comment { get; set; }

        public int? PrepositionId { get; set; }
    }
}
