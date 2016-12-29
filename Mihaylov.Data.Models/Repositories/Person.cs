using System;
using System.Linq.Expressions;
using Mihaylov.Data.Models.Helpers;
using Mihaylov.Database.Models.Enums;
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
                    CreateDate = person.CreateDate,
                    AskDate = person.AskDate,
                    Age = person.Age,
                   // CountryId = person.CountryId,
                    Country = person.Country.Name,
                    Ethnicity = person.Ethnicity.Type,
                    SexPreference = person.SexPreference.Type,
                    AnswerType = person.Answer1.Type,
                    Answer = person.Answer,
                    AnswerConverted = person.AnswerConverted,
                    AnswerUnit = person.Unit.Type,
                    Comments = person.Comments,
                    RecordsPath = person.RecordsPath,
                };
            }
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime AskDate { get; set; }

        public int Age { get; set; }

      //  public int CountryId { get; set; }

        public string Country { get; set; }

        public EthnicityType Ethnicity { get; set; }

        public SexPreferenceType SexPreference { get; set; }

        public AnswerType AnswerType { get; set; }

        public decimal? Answer { get; set; }

        public UnitType AnswerUnit { get; set; }

        public decimal? AnswerConverted { get; set; }

        public string Comments { get; set; }

        public string RecordsPath { get; set; }
        

        //public void SaveToDb(DAL.Person person)
        //{
        //    person.PersonId = this.Id;

        //}

        public DAL.Person Create(PersonInfo personInfo)
        {
            DAL.Person person = new DAL.Person()
            {               
                Username = personInfo.Username,
                CreateDate = personInfo.CreateDate,
                AskDate = personInfo.AskDate,
                Age = personInfo.Age,
                CountryId = personInfo.CountryId,
                EthnicityId = personInfo.EthnicityId,
                SexPreferenceId = personInfo.SexPreferenceId,
            };

            return person;
        }
    }
}
