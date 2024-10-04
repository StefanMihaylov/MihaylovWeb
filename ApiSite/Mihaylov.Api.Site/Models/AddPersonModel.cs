using System;
using Mihaylov.Api.Site.Contracts.Models;

namespace Mihaylov.Api.Site.Models
{
    public class AddPersonModel
    {
        public long? Id { get; set; }

        public AddPersonDetailModel Detais { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateOfBirthType? DateOfBirthType { get; set; }

        public int? CountryId { get; set; }

        public AddPersonLocationModel Location { get; set; }

        public int? EthnicityId { get; set; }

        public int? OrientationId { get; set; }

        public string Comments { get; set; }
    }
}
