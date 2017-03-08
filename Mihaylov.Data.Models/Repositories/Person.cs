using System;
using System.Linq.Expressions;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Models.Repositories
{
    public class Person
    {
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
                    Ethnicity = person.EthnicityType.Name,  // addit
                    OrientationId = person.OrientationType.OrientationTypeId,
                    Orientation = person.OrientationType.Name,  // addit
                    AnswerTypeId = person.AnswerType.AnswerTypeId,
                    AnswerType = person.AnswerType.Name,
                    Answer = person.Answer,
                    AnswerConverted = person.AnswerConverted,
                    AnswerUnitId = person.UnitType.UnitTypeId,
                    AnswerUnit = person.UnitType.Name,
                    Comments = person.Comments,
                    RecordsPath = person.RecordsPath,
                    IsAccountDisabled = person.IsAccountDisabled,
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
                Ethnicity = personDAL.EthnicityType.Name,  // addit
                OrientationId = personDAL.OrientationType.OrientationTypeId,
                Orientation = personDAL.OrientationType.Name,  // addit
                AnswerTypeId = personDAL.AnswerType.AnswerTypeId,
                AnswerType = personDAL.AnswerType.Name,
                Answer = personDAL.Answer,
                AnswerConverted = personDAL.AnswerConverted,
                AnswerUnitId = personDAL.UnitType.UnitTypeId,
                AnswerUnit = personDAL.UnitType.Name,
                Comments = personDAL.Comments,
                RecordsPath = personDAL.RecordsPath,
                IsAccountDisabled = personDAL.IsAccountDisabled,
            };

            return PersonDTO;
        }

        public DAL.Person Create()
        {
            DAL.Person person = new DAL.Person()
            {
                PersonId = this.Id,
                Username = this.Username,
                CreateDate = this.CreateDate,
                AskDate = this.AskDate,
                Age = this.Age,
                CountryId = this.CountryId,
                EthnicityTypeId = this.EthnicityId,
                OrientationTypeId = this.OrientationId,
                AnswerTypeId = this.AnswerTypeId,
                Answer = this.Answer,
                AnswerConverted = this.AnswerConverted,
                AnswerUnitTypeId = this.AnswerUnitId,
                Comments = this.Comments,
                RecordsPath = this.RecordsPath,
                IsAccountDisabled = this.IsAccountDisabled,
                LastBroadcastDate = this.LastBroadcastDate,                                  
            };

            return person;
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastBroadcastDate { get; set; }

        public DateTime? AskDate { get; set; }

        public int Age { get; set; }

        public int CountryId { get; set; }

        public string Country { get; set; }

        public int EthnicityId { get; set; }

        public string Ethnicity { get; set; }

        public int OrientationId { get; set; }

        public string Orientation { get; set; }

        public int AnswerTypeId { get; set; }

        public string AnswerType { get; set; }

        public decimal? Answer { get; set; }

        public int? AnswerUnitId { get; set; }

        public string AnswerUnit { get; set; }

        public decimal? AnswerConverted { get; set; }

        public string Comments { get; set; }

        public string RecordsPath { get; set; }

        public bool IsAccountDisabled { get; set; }
    }
}
