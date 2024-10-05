using System;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Models.Site
{
    public class PersonModel
    {
        public long? Id { get; set; }

        public PersonDetailModel Details { get; set; }

        public int? Age { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateOfBirthType? DateOfBirthType { get; set; }

        public int? CountryId { get; set; }

        public PersonLocationModel Location { get; set; }

        public int? EthnicityId { get; set; }

        public int? OrientationId { get; set; }

        public string Comments { get; set; }
    }
}
