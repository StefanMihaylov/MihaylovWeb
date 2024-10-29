namespace Mihaylov.Api.Dictionary.Database.Models
{
    public partial class _IncorrectAnswer
    {
        public int TestId { get; set; }

        public int RecordId { get; set; }

        public Record Record { get; set; }

        public _Test Test { get; set; }
    }
}
