using System;
using System.Collections.Generic;
using Mihaylov.Common.Generic.Extensions;

namespace Mihaylov.Api.Site.Contracts.Models
{
    public class Person
    {
        public const int CommentsMaxLength = 4000;

        public long Id { get; set; }

        public PersonDetail Details { get; set; }


        public DateTime? DateOfBirth { get; set; }

        public DateOfBirthType? DateOfBirthType { get; set; }

        public int? Age => DateOfBirth.GetAge();

        public string AgeDisplay
        {
            get
            {
                if (!DateOfBirth.HasValue || !DateOfBirthType.HasValue)
                {
                    return string.Empty;
                }

                return DateOfBirthType switch
                {
                    Models.DateOfBirthType.Full => $"{Age} ({DateOfBirth.Value:dd.MM.yyyy})",
                    Models.DateOfBirthType.YearAndMonth => $"{Age} (**.{DateOfBirth.Value:MM.yyyy})",
                    Models.DateOfBirthType.YearOnly => $"{Age} (**.**.{DateOfBirth.Value:yyyy})",
                    Models.DateOfBirthType.YearCalculated => $"{Age}",
                    _ => string.Empty,
                };
            }
        }

        public int? CountryId { get; set; }

        public string Country { get; set; }

        public PersonLocation Location { get; set; }

        public int? EthnicityId { get; set; }

        public string Ethnicity { get; set; }

        public int? OrientationId { get; set; }

        public string Orientation { get; set; }


        public string Comments { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<Account> Accounts { get; set; }

        public int AnswersCount { get; set; }


        public Person()
        {
            Accounts = new List<Account>();
        }
    }
}
