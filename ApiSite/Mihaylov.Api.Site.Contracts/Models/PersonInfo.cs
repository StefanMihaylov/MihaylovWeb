using System;

namespace Mihaylov.Api.Site.Contracts.Models
{
    public class PersonInfo
    {
        public string Username { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime AskDate { get; set; }

        public int Age { get; set; }

        public int CountryId { get; set; }

        public int EthnicityId { get; set; }

        public int OrientationId { get; set; }
    }
}
