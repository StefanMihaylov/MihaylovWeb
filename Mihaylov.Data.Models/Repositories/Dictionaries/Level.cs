using System;
using System.Linq.Expressions;
using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Models.Dictionaries
{
    public class Level : IMapFrom<DAL.Level>, IHaveCustomMappings
    {
        private Level()
        {
        }

        public Level(int id, int learningSystemId, string name, string descrition,
            int? modulesStartNumber, int? modulesEndNumber)
        {
            this.Id = id;
            this.LearningSystemId = learningSystemId;            
            this.Name = name;
            this.Descrition = descrition;
            this.ModulesStartNumber = modulesStartNumber;
            this.ModulesEndNumber = modulesEndNumber;
        }

        public int Id { get; private set; }

        public int LearningSystemId { get; private set; }        

        public string Name { get; private set; }

        public string Descrition { get; private set; }

        public int? ModulesStartNumber { get; private set; }

        public int? ModulesEndNumber { get; private set; }

        public LearningSystem LearningSystem { get; private set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.Level, Level>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.LevelId))
                .ForMember(x => x.LearningSystem, opt => opt.MapFrom(m => m.LearningSystem));
        }
    }
}
