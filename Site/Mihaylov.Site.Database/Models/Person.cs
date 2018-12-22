using System;
using System.Collections.Generic;

namespace Mihaylov.Site.Database.Models
{
    public partial class Person
    {
        public int PersonId { get; set; }
        public string Username { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastBroadcastDate { get; set; }
        public DateTime? AskDate { get; set; }
        public int Age { get; set; }
        public int CountryId { get; set; }
        public int EthnicityTypeId { get; set; }
        public int OrientationTypeId { get; set; }
        public int AnswerTypeId { get; set; }
        public decimal? Answer { get; set; }
        public int? AnswerUnitTypeId { get; set; }
        public decimal? AnswerConverted { get; set; }
        public string Comments { get; set; }
        public string RecordsPath { get; set; }
        public bool IsAccountDisabled { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual AnswerType AnswerType { get; set; }
        public virtual UnitType AnswerUnitType { get; set; }
        public virtual Country Country { get; set; }
        public virtual EthnicityType EthnicityType { get; set; }
        public virtual OrientationType OrientationType { get; set; }
    }
}
