using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Models.Dictionaries
{
    public class Language : IMapFrom<DAL.Language>, IHaveCustomMappings
    {
        private Language()
        {
        }

        public Language(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.Language, Language>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.LanguageId));
                //.ConstructUsing(x => new Language(x.LanguageId, x.Name));
        }
    }
}
