using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Database.Dictionaries;

namespace Mihaylov.Data.Models.Dictionaries
{
    public class Record : IMapFrom<DAL.Record>, IHaveCustomMappings
    {
        public Record()
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

        public int Id { get; set; }

        public int CourseId { get; set; }

        public int? ModuleNumber { get; set; }

        public string Original { get; set; }

        public string Translation { get; set; }

        public string Comment { get; set; }

        public ICollection<RecordType> RecordTypes { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.Record, Record>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.RecordId))
                .ForMember(x => x.RecordTypes, opt => opt.MapFrom(m => m.RecordTypes));
        }

        public DAL.Record Create()
        {
            var result = new DAL.Record()
            {
                CourseId = this.CourseId,
                ModuleNumber = this.ModuleNumber,
                Original = this.Original,
                Translation = this.Translation,
                Comment = this.Comment,
            };

            return result;
        }
    }
}
