namespace Mihaylov.Api.Site.Contracts.Models
{
    public class PersonMerge
    {
        public long From { get; set; }

        public long To { get; set; }

        public bool? FirstName { get; set; }

        public bool? MiddleName { get; set; }

        public bool? LastName { get; set; }

        public bool? OtherNames { get; set; }

        public bool? DateOfBirth { get; set; }

        public bool? DateOfBirthType { get; set; }

        public bool? CountryId { get; set; }

        public bool? CountryStateId { get; set; }

        public bool? Region { get; set; }

        public bool? City { get; set; }

        public bool? Details { get; set; }

        public bool? EthnicityId { get; set; }

        public bool? OrientationId { get; set; }

        public bool? Comments { get; set; }

        public bool? Accounts { get; set; }

        public bool? Answers { get; set; }
    }
}
