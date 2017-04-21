using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Models.Repositories
{
    public class Person
    {
        public Person()
        {
        }

        public Person(Person person)
        {
            Id = person.Id;
            Username = person.Username;
            CreateDate = person.CreateDate;
            LastBroadcastDate = person.LastBroadcastDate;
            AskDate = person.AskDate;
            Age = person.Age;
            CountryId = person.CountryId;
            Country = person.Country;
            EthnicityId = person.EthnicityId;
            Ethnicity = person.Ethnicity;
            OrientationId = person.OrientationId;
            Orientation = person.Orientation;
            AnswerTypeId = person.AnswerTypeId;
            AnswerType = person.AnswerType;
            Answer = person.Answer;
            AnswerConverted = person.AnswerConverted;
            AnswerUnitId = person.AnswerUnitId;
            AnswerUnit = person.AnswerUnit;
            Comments = person.Comments;
            RecordsPath = person.RecordsPath;
            IsAccountDisabled = person.IsAccountDisabled;
            UpdatedDate = person.UpdatedDate;
        }

        public static Expression<Func<DAL.Person, Person>> FromDb
        {
            get
            {
                return person => new Person
                {
                    Id = person.PersonId,
                    Username = person.Username,
                    CreateDate = person.CreateDate, // addit
                    LastBroadcastDate = person.LastBroadcastDate,
                    AskDate = person.AskDate,
                    Age = person.Age, //  // addit
                    CountryId = person.CountryId,
                    Country = person.Country.Name,  // addit
                    EthnicityId = person.EthnicityType.EthnicityTypeId,
                    Ethnicity = person.EthnicityType.Description,  // addit
                    OrientationId = person.OrientationType.OrientationTypeId,
                    Orientation = person.OrientationType.Description,  // addit
                    AnswerTypeId = person.AnswerType.AnswerTypeId,
                    AnswerType = person.AnswerType.Description,
                    Answer = person.Answer,
                    AnswerConverted = person.AnswerConverted,
                    AnswerUnitId = person.UnitType.UnitTypeId,
                    AnswerUnit = person.UnitType.Description,
                    Comments = person.Comments,
                    RecordsPath = person.RecordsPath,
                    IsAccountDisabled = person.IsAccountDisabled,
                    UpdatedDate = person.UpdatedDate,
                };
            }
        }

        public static implicit operator Person(DAL.Person personDAL)
        {
            if (personDAL == null)
            {
                return null;
            }

            Person PersonDTO = new Person
            {
                Id = personDAL.PersonId,
                Username = personDAL.Username,
                CreateDate = personDAL.CreateDate, // addit
                LastBroadcastDate = personDAL.LastBroadcastDate,
                AskDate = personDAL.AskDate,
                Age = personDAL.Age, //  // addit
                CountryId = personDAL.CountryId,
                Country = personDAL.Country.Name,  // addit
                EthnicityId = personDAL.EthnicityType.EthnicityTypeId,
                Ethnicity = personDAL.EthnicityType.Description,  // addit
                OrientationId = personDAL.OrientationType.OrientationTypeId,
                Orientation = personDAL.OrientationType.Description,  // addit
                AnswerTypeId = personDAL.AnswerType.AnswerTypeId,
                AnswerType = personDAL.AnswerType.Description,
                Answer = personDAL.Answer,
                AnswerConverted = personDAL.AnswerConverted,
                AnswerUnitId = personDAL.UnitType.UnitTypeId,
                AnswerUnit = personDAL.UnitType.Name,
                Comments = personDAL.Comments,
                RecordsPath = personDAL.RecordsPath,
                IsAccountDisabled = personDAL.IsAccountDisabled,
                UpdatedDate = personDAL.UpdatedDate,
            };

            return PersonDTO;
        }

        public void Update(DAL.Person person)
        {
            person.Username = this.Username;
            person.Age = this.Age;
            person.CountryId = this.CountryId;
            person.IsAccountDisabled = this.IsAccountDisabled;
            person.LastBroadcastDate = this.LastBroadcastDate;

            person.Comments = this.Comments;
            person.RecordsPath = this.RecordsPath;

            if (person.PersonId == 0)
            {
                person.CreateDate = this.CreateDate;
                person.EthnicityTypeId = this.EthnicityId;
                person.OrientationTypeId = this.OrientationId;
            }

            if (!person.Answer.HasValue)
            {
                person.AnswerTypeId = this.AnswerTypeId;
                person.Answer = this.Answer;
                person.AnswerConverted = this.AnswerConverted;
                person.AnswerUnitTypeId = this.AnswerUnitId;
                person.AskDate = this.AskDate;
            }

            person.UpdatedDate = this.UpdatedDate;
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

        public int EthnicityId { get; set; }

        [Display(Name = "Ethnicity:")]
        public string Ethnicity { get; set; }

        public int OrientationId { get; set; }

        [Display(Name = "Orientation:")]
        public string Orientation { get; set; }

        [Display(Name = "AnswerType:")]
        public int AnswerTypeId { get; set; }

        [Display(Name = "AnswerType:")]
        public string AnswerType { get; set; }

        [Display(Name = "Answer:")]
        public decimal? Answer { get; set; }

        public int? AnswerUnitId { get; set; }

        public string AnswerUnit { get; set; }

        public decimal? AnswerConverted { get; set; }

        [Display(Name = "Comments:")]
        public string Comments { get; set; }

        public string RecordsPath { get; set; }

        [Display(Name = "Disabled:")]
        public bool IsAccountDisabled { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
