using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Web.ViewModels.Dictionaries
{
    public class ChooseCourseModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? CourseId { get; set; }        
    }
}