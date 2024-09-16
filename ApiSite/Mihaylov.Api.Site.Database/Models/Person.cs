using System;
using System.Collections.Generic;
using Mihaylov.Common.Abstract.Databases.Models;

namespace Mihaylov.Api.Site.Database.Models
{
    public class Person : Entity
    {
        public long PersonId { get; set; }

        public PersonDetail Details { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public byte? DateOfBirthId { get; set; }

        public DateOfBirth DateOfBirthModel { get; set; }


        public int? CountryId { get; set; }

        public Country Country { get; set; }

        public PersonLocation Location { get; set; }


        public int? EthnicityId { get; set; }

        public Ethnicity Ethnicity { get; set; }

        public int? OrientationId { get; set; }

        public Orientation Orientation { get; set; }


        public string Comments { get; set; }


        public IEnumerable<Account> Accounts { get; set; }

        public IEnumerable<QuizAnswer> Answers { get; set; }
    }
}
