using System;

namespace Mihaylov.Api.Dictionary.Database.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        public int LearningSystemId { get; set; }

        public LearningSystem LearningSystem { get; set; }

        public int LevelId { get; set; }

        public Level Level { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ModulesStartNumber { get; set; }

        public int? ModulesEndNumber { get; set; }
    }
}
