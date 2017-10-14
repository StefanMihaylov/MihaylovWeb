using System.Collections.Generic;
using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Models.Dictionaries
{
    public class Record : IMapFrom<DAL.Record>, IHaveCustomMappings
    {
        private Record()
        {
        }

        public Record(int id, int courseId, int? moduleNumber, string original, string translation, string comment,
            ICollection<RecordType> recordTypes)
        {
            this.Id = id;
            this.CourseId = courseId;
            this.ModuleNumber = moduleNumber;
            this.Original = original;
            this.Translation = translation;
            this.Comment = comment;
            this.RecordTypes = recordTypes;
        }

        public int Id { get; private set; }

        public int CourseId { get; private set; }

        public int? ModuleNumber { get; private set; }

        public string Original { get; private set; }

        public string Translation { get; private set; }

        public string Comment { get; private set; }

        public ICollection<RecordType> RecordTypes { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.Record, Record>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.RecordId));
        }
    }
}
