using System;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Models
{
    public class AddPersonModel
    {
        public long? Id { get; set; }

        public AddPersonDetailModel Details { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateOfBirthType? DateOfBirthType { get; set; }

        public int? Age { get; set; }

        public int? CountryId { get; set; }

        public AddPersonLocationModel Location { get; set; }

        public int? EthnicityId { get; set; }

        public int? OrientationId { get; set; }

        public string Comments { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
