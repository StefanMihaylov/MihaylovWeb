using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using AutoMapper;
using Mihaylov.Common.Mapping;
using DAL = Mihaylov.Site.Database;

namespace Mihaylov.Site.Data.Models
{
    public class Person : IMapFrom<DAL.Person>, IHaveCustomMappings
    {
        public Person()
        {
        }

        public Person(int id, string username, DateTime createDate, DateTime lastBroadcastDate, DateTime? askDate,
            int age, int countryId, string country, int ethnicityId, string ethnicity, int orientationId, string orientation,
            int answerTypeId, string answerType, decimal? answer, decimal? answerConverted, int? answerUnitId, string answerUnit,
            string comments, string recordsPath, bool isAccountDisabled, DateTime? updatedDate)
        {
            Id = id;
            Username = username;
            CreateDate = createDate;
            LastBroadcastDate = lastBroadcastDate;
            AskDate = askDate;
            Age = age;
            CountryId = countryId;
            Country = country;
            EthnicityTypeId = ethnicityId;
            Ethnicity = ethnicity;
            OrientationTypeId = orientationId;
            Orientation = orientation;
            AnswerTypeId = answerTypeId;
            AnswerType = answerType;
            Answer = answer;
            AnswerConverted = answerConverted;
            AnswerUnitTypeId = answerUnitId;
            AnswerUnit = answerUnit;
            Comments = comments;
            RecordsPath = recordsPath;
            IsAccountDisabled = isAccountDisabled;
            UpdatedDate = updatedDate;
        }

        public Person(Person person)
            : this(person.Id, person.Username, person.CreateDate, person.LastBroadcastDate, person.AskDate,
            person.Age, person.CountryId, person.Country, person.EthnicityTypeId, person.Ethnicity, person.OrientationTypeId,
            person.Orientation, person.AnswerTypeId, person.AnswerType, person.Answer, person.AnswerConverted,
            person.AnswerUnitTypeId, person.AnswerUnit, person.Comments, person.RecordsPath, person.IsAccountDisabled,
            person.UpdatedDate)
        {
        }

        public void Update(DAL.Person person)
        {
            person.Username = this.Username;
            person.CreateDate = this.CreateDate;
            person.LastBroadcastDate = this.LastBroadcastDate;
            person.AskDate = this.AskDate;
            person.Age = this.Age;
            person.CountryId = this.CountryId;
            person.EthnicityTypeId = this.EthnicityTypeId;
            person.OrientationTypeId = this.OrientationTypeId;
            person.AnswerTypeId = this.AnswerTypeId;
            person.Answer = this.Answer;
            person.AnswerConverted = this.AnswerConverted;
            person.AnswerUnitTypeId = this.AnswerUnitTypeId;
            person.Comments = this.Comments;
            person.RecordsPath = this.RecordsPath;
            person.IsAccountDisabled = this.IsAccountDisabled;
            person.UpdatedDate = this.UpdatedDate;
        }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<DAL.Person, Person>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => m.PersonId))
                .ForMember(x => x.Country, opt => opt.MapFrom(m => m.Country.Name))
                .ForMember(x => x.Ethnicity, opt => opt.MapFrom(m => m.EthnicityType.Description))
                .ForMember(x => x.Orientation, opt => opt.MapFrom(m => m.OrientationType.Description))
                .ForMember(x => x.AnswerType, opt => opt.MapFrom(m => m.AnswerType.Description))
                .ForMember(x => x.AnswerUnit, opt => opt.MapFrom(m => m.UnitType.Name));
        }

        public int Id { get; set; }

        [Display(Name = "Username:")]
        public string Username { get; set; }

        [Display(Name = "Create:")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Broadcast:")]
        public DateTime LastBroadcastDate { get; set; }

        [Display(Name = "Asked:")]
        public DateTime? AskDate { get; set; }

        [Display(Name = "Age:")]
        public int Age { get; set; }

        public int CountryId { get; set; }

        [Display(Name = "Country:")]
        public string Country { get; set; }

        public int EthnicityTypeId { get; set; }

        [Display(Name = "Ethnicity:")]
        public string Ethnicity { get; set; }

        public int OrientationTypeId { get; set; }

        [Display(Name = "Orientation:")]
        public string Orientation { get; set; }

        [Display(Name = "AnswerType:")]
        public int AnswerTypeId { get; set; }

        [Display(Name = "AnswerType:")]
        public string AnswerType { get; set; }

        public decimal? Answer { get; set; }

        public int? AnswerUnitTypeId { get; set; }

        public string AnswerUnit { get; set; }

        public decimal? AnswerConverted { get; set; }

        public string AnswerConvertedUnit { get; set; } // TODO

        [Display(Name = "Comments:")]
        public string Comments { get; set; }

        public string RecordsPath { get; set; }

        [Display(Name = "Disabled:")]
        public bool IsAccountDisabled { get; set; }

        public DateTime? UpdatedDate { get; set; }


        [Display(Name = "Answer:")]
        public string AnswerDisplay
        {
            get
            {
                if (!this.Answer.HasValue || !this.AnswerConverted.HasValue)
                {
                    return string.Empty;
                }

                if (this.Answer.Value == this.AnswerConverted.Value)
                {
                    return $"{this.AnswerConverted.Value} {this.AnswerUnit}";
                }
                else
                {
                    return $"{this.Answer.Value} {this.AnswerUnit} ({this.AnswerConverted.Value} {this.AnswerConvertedUnit})";
                }
            }
        }
    }
}
