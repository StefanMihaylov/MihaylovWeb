using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.Models.Site
{
    public class AnswerViewModel
    {
        public long? Id { get; set; }

        [Required]
        public long? PersonId { get; set; }
    }
}
