//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using Mihaylov.Dictionaries.Data.Models;

//namespace Mihaylov.Web.ViewModels.Dictionaries
//{
//    public class RecordViewModel
//    {
//        public RecordViewModel()
//        {
//            this.RecordTypes = new List<RecordTypeViewModel>();
//        }

//        public RecordViewModel(int courseId, IEnumerable<RecordType> recordTypes)
//        {
//            this.CourseId = courseId;
//            this.RecordTypes = recordTypes.Select(r => new RecordTypeViewModel(r)).ToList();
//        }

//        public RecordViewModel(Record record)
//            : this(record.CourseId, record.RecordTypes)
//        {
//            this.Id = record.Id;
//            this.ModuleNumber = record.ModuleNumber;
//            this.Original = record.Original;
//            this.Translation = record.Translation;
//            this.Comment = record.Comment;
//        }

//        [Required]
//        public int Id { get; set; }

//        [Required]
//        [Range(1, int.MaxValue)]
//        public int CourseId { get; set; }

//        public int? ModuleNumber { get; set; }

//        [Required]
//        public string Original { get; set; }

//        [Required]
//        public string Translation { get; set; }

//        public string Comment { get; set; }

//        [Required]
//        public ICollection<RecordTypeViewModel> RecordTypes { get; set; }
//    }
//}