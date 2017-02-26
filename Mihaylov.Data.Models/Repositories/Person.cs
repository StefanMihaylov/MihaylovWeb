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
                return person => ConvertToDTO(person);
            }
        }

        public static implicit operator Person(DAL.Person personDAL)
        {
            if (personDAL == null)
            {
                return null;
            }

            Person PersonDTO = ConvertToDTO(personDAL);
            return PersonDTO;
        }

        //public DAL.Person Create(PersonInfo personInfo)
        //{
        //    DAL.Person person = new DAL.Person()
        //    {
        //        Username = personInfo.Username,
        //        CreateDate = personInfo.CreateDate,
        //        AskDate = personInfo.AskDate,
        //        Age = personInfo.Age,
        //        CountryId = personInfo.CountryId,
        //        EthnicityTypeId = personInfo.EthnicityId,
        //        OrientationTypeId = personInfo.OrientationId,
        //    };

        //    return person;
        //}

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


        private static Person ConvertToDTO(DAL.Person person)
        {
            return new Person
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
}
