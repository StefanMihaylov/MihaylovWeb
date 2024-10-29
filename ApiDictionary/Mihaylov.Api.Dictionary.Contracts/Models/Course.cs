using System;

namespace Mihaylov.Api.Dictionary.Contracts.Models
{
    public class Course
    {
        public int Id { get; set; }

        public int LevelId { get; set; }

        public string Level { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ModulesStartNumber { get; set; }

        public int? ModulesEndNumber { get; set; }

        public int LearningSystemId { get; set; }

        public string LearningSystem { get; set; }

        public string Language { get; set; }
    }
}
