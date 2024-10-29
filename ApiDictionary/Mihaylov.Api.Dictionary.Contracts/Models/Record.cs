namespace Mihaylov.Api.Dictionary.Contracts.Models
{
    public class Record
    {
        public const int OriginalMaxLength = 500;
        public const int TranslationMaxLength = 500;
        public const int CommentMaxLength = 500;

        public int Id { get; set; }

        public int CourseId { get; set; }

        public int? ModuleNumber { get; set; }

        public int RecordTypeId { get; set; }

        public string RecordType { get; set; }

        public string Original { get; set; }

        public string Translation { get; set; }

        public string Comment { get; set; }

        public int? PrepositionId { get; set; }

        public string Preposition { get; set; }
    }
}
