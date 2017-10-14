using System;
using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Models.Dictionaries
{
    public class Course : IMapFrom<DAL.Cours>, IHaveCustomMappings
    {
        private Course()
        {
        }

        public Course(int id, int levelId, DateTime? startDate, DateTime? endDate)
        {
            this.Id = id;
            this.LevelId = levelId;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        public int Id { get; private set; }

        public int LevelId { get; private set; }        

        public DateTime? StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public Level Level { get; private set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.Cours, Course>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.CourseId))
                .ForMember(x => x.Level, opt => opt.MapFrom(m => m.Level));
        }
    }
}
