using System;
using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Dictionaries.Database.Models;

namespace Mihaylov.Dictionaries.Data.Models
{
    public class Course : IMapFrom<DAL.Course>, IHaveCustomMappings
    {
        public Course()
        {
        }

        public Course(int id, int levelId, DateTime? startDate, DateTime? endDate)
        {
            this.Id = id;
            this.LevelId = levelId;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        public int Id { get; set; }

        public int LevelId { get; private set; }

        public DateTime? StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public Level Level { get; private set; }

        public override string ToString()
        {
            string language = this.Level.LearningSystem.Language.Name;
            string date = this.StartDate.HasValue ? this.StartDate.Value.ToString("yyyy.MM") : string.Empty;
            return $"{language} {this.Level.Name} - '{this.Level.LearningSystem.Name}' - {date}";
        }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.Course, Course>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.CourseId))
                .ForMember(x => x.Level, opt => opt.MapFrom(m => m.Level));
        }
    }
}
