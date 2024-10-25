using System;

namespace Mihaylov.Api.Site.Data.Models
{
    public class PersonInfo
    {
        public string Username { get; set; }

        public bool IsDeleted { get; set; }

        public string Comments { get; set; }


        public int? Age { get; set; }

        public string City { get; set; }

        public int? CountryId { get; set; }

        public int? EthnicityId { get; set; }

        public int? OrientationId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastOnlineDate { get; set; }
    }
}
