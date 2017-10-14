using System;
using System.Linq.Expressions;
using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Models.Dictionaries
{
    public class LearningSystem : IMapFrom<DAL.LearningSystem>, IHaveCustomMappings
    {
        private LearningSystem()
        {
        }

        public LearningSystem(int id, int languageId, string name)
        {
            this.Id = id;
            this.LanguageId = languageId;
            this.Name = name;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public int LanguageId { get; private set; }

        public Language Language { get; private set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.LearningSystem, LearningSystem>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.LearningSystemId))
                .ForMember(x => x.Language, opt => opt.MapFrom(m => m.Language));
        }
    }
}
