using System;

namespace Mihaylov.Api.Site.Contracts.Models
{
    public class Account
    {
        public const int NameMaxLength = 50;
        public const int DetailsMaxLength = 1000;

        public Guid Id { get; set; }

        public AccountType AccountType { get; set; }

        public string Username { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime LastOnlineDate { get; set; }

        public bool IsAccountDisabled { get; set; }


        //public DateTime? AskDate { get; set; }
        //public int AnswerTypeId { get; set; }
        //public string AnswerType { get; set; }
        //public decimal? Answer { get; set; }
        //public int? AnswerUnitTypeId { get; set; }
        //public string AnswerUnit { get; set; }
        //public decimal? AnswerConverted { get; set; }
        //public string AnswerConvertedUnit { get; set; } // TODO
        //public DateTime? ModifiedOn { get; set; }
    }
}
