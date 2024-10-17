using System.Collections.Generic;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class PersonExtended : Person
    {
        public PersonExtended(Person person)
        {
            Id = person.Id;
            Details = person.Details;
            DateOfBirth = person.DateOfBirth;
            DateOfBirthType = person.DateOfBirthType;
            CountryId = person.CountryId;
            Country = person.Country;
            Location = person.Location;
            EthnicityId = person.EthnicityId;
            Ethnicity = person.Ethnicity;
            OrientationId = person.OrientationId;
            Orientation = person.Orientation;
            Comments = person.Comments;
            CreatedOn = person.CreatedOn;
            Accounts = person.Accounts;
            AnswersCount = person.AnswersCount;
        }

        public AnswersExtended AnswersExtended { get; set; }


        public IEnumerable<Country> Countries { get; set; }

        public IEnumerable<CountryState> CountryStates { get; set; }

        public IEnumerable<Ethnicity> Ethnicities { get; set; }

        public IEnumerable<Orientation> Orientations { get; set; }
    }
}