using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Dictionaries.Database.Models;

namespace Mihaylov.Dictionaries.Data.Models
{
    public class PrepositionType : IMapFrom<DAL.PrepositionType>, IHaveCustomMappings
    {
        private PrepositionType()
        {
        }

        public PrepositionType(int id, int languageId, string value)
        {
            this.Id = id;
            this.LanguageId = languageId;
            this.Value = value;
        }

        public int Id { get; private set; }

        public int LanguageId { get; private set; }

        public string Value { get; private set; }


        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.PrepositionType, PrepositionType>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.PrepositionTypeId));
        }
    }
}
