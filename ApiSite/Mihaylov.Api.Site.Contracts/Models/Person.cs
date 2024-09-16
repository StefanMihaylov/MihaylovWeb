using System;

namespace Mihaylov.Api.Site.Contracts.Models
{
    public class Person
    {
        public const int NameMaxLength = 50;
        public const int OtherNamesMaxLength = 200;
        public const int RegionMaxLength = 100;
        public const int CityMaxLength = 50;
        public const int CommentsMaxLength = 4000;

        public long Id { get; set; }


        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string OtherNames { get; set; }


        public DateTime? DateOfBirth { get; set; }

        public DateOfBirthType? DateOfBirthType { get; set; }

        public int? Age { get; set; }

        public int? CountryId { get; set; }

        public string Country { get; set; }


        public int? CountryStateId { get; set; }

        public string CountryState { get; set; }

        public string Region { get; set; }

        public string City { get; set; }


        public int? EthnicityId { get; set; }

        public string Ethnicity { get; set; }

        public int? OrientationId { get; set; }

        public string Orientation { get; set; }


        public string Comments { get; set; }


        //public void Update(DAL.Person person)
        //{
        //    //person.Username = this.Username;
        //    person.CreatedOn = this.CreatedOn;
        //    //person.LastBroadcastDate = this.LastBroadcastDate;
        //    //person.AskDate = this.AskDate;
        //    person.Age = this.Age;
        //    person.CountryId = this.CountryId;
        //    person.EthnicityTypeId = this.EthnicityTypeId;
        //    person.OrientationTypeId = this.OrientationTypeId;
        //    //person.AnswerTypeId = this.AnswerTypeId;
        //    //person.Answer = this.Answer;
        //    //person.AnswerConverted = this.AnswerConverted;
        //    //person.AnswerUnitTypeId = this.AnswerUnitTypeId;
        //    //person.Comments = this.Comments;
        //    //person.RecordsPath = this.RecordsPath;
        //    //person.IsAccountDisabled = this.IsAccountDisabled;
        //    person.ModifiedOn = this.ModifiedOn;
        //}

        //public void CreateMappings(IMapperConfigurationExpression configuration)
        //{
        //    configuration.CreateMap<DAL.Person, Person>()
        //        .ForMember(x => x.Id, opt => opt.MapFrom(m => m.Id))
        //        .ForMember(x => x.Country, opt => opt.MapFrom(m => m.Country.Name))
        //        .ForMember(x => x.Ethnicity, opt => opt.MapFrom(m => m.EthnicityType.Description))
        //        .ForMember(x => x.Orientation, opt => opt.MapFrom(m => m.OrientationType.Description));
        //    //.ForMember(x => x.AnswerType, opt => opt.MapFrom(m => m.AnswerType.Description))
        //    //.ForMember(x => x.AnswerUnit, opt => opt.MapFrom(m => m.AnswerUnitType.Name));
        //}

        //public string AnswerDisplay
        //{
        //    get
        //    {
        //        if (!this.Answer.HasValue || !this.AnswerConverted.HasValue)
        //        {
        //            return string.Empty;
        //        }

        //        if (this.Answer.Value == this.AnswerConverted.Value)
        //        {
        //            return $"{this.AnswerConverted.Value} {this.AnswerUnit}";
        //        }
        //        else
        //        {
        //            return $"{this.Answer.Value} {this.AnswerUnit} ({this.AnswerConverted.Value} {this.AnswerConvertedUnit})";
        //        }
        //    }
        //}
    }
}
