using System;
using System.Collections.Generic;
using Mihaylov.Common.Abstract.Databases.Models;

namespace Mihaylov.Api.Site.Database.Models
{
    public class Person : Entity
    {
        public long PersonId { get; set; }


        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string OtherNames { get; set; }


        public DateTime? DateOfBirth { get; set; }

        public byte? DateOfBirthId { get; set; }

        public DateOfBirth DateOfBirthModel { get; set; }


        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }


        public int? CountryStateId { get; set; }

        public virtual CountryState CountryState { get; set; }

        public string Region { get; set; }

        public string City { get; set; }


        public int? EthnicityId { get; set; }

        public virtual Ethnicity Ethnicity { get; set; }

        public int? OrientationId { get; set; }

        public virtual Orientation Orientation { get; set; }


        public string Comments { get; set; }


        public IEnumerable<Account> Accounts { get; set; }

        public IEnumerable<QuizAnswer> Answers { get; set; }
    }
}
