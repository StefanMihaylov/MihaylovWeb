using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Models.Dictionaries
{
    public class RecordType : IMapFrom<DAL.RecordType>, IHaveCustomMappings
    {
        private RecordType()
        {
        }

        public RecordType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.RecordType, RecordType>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.RecordTypeId));
        }
    }
}
