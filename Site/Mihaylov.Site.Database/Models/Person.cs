using System;
using System.Collections.Generic;
using Mihaylov.Common.Databases.Models;

namespace Mihaylov.Site.Database.Models
{
    public class Person : Entity
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string OtherNames { get; set; }


        public DateTime? DateOfBirth { get; set; }

        public DateOfBirthType? DateOfBirthType { get; set; }

        public virtual DateOfBirthModel DateOfBirthModel { get; set; }

        public int? Age { get; set; }


        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        public int? StateId { get; set; }

        public virtual State State { get; set; }

        public string City { get; set; }


        public int? EthnicityTypeId { get; set; }

        public virtual EthnicityType EthnicityType { get; set; }

        public int? OrientationTypeId { get; set; }

        public virtual OrientationType OrientationType { get; set; }


        public string Comments { get; set; }

        public IEnumerable<Account> Accounts { get; set; }


        //public virtual AnswerType AnswerType { get; set; }
        //public virtual UnitType AnswerUnitType { get; set; }

        //public DateTime? AskDate { get; set; }
        //public int AnswerTypeId { get; set; }
        //public decimal? Answer { get; set; }
        //public int? AnswerUnitTypeId { get; set; }
        //public decimal? AnswerConverted { get; set; }

        //public string RecordsPath { get; set; }
    }
}
