using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Other.Contracts.Base;
using Mihaylov.Api.Other.Contracts.Show.Models;

namespace Mihaylov.Api.Other.Models.Concerts
{
    public class ConcertModel
    {
        public int? Id { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [MaxLength(ModelConstants.ConcertNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public int? LocationId { get; set; }

        [Required]
        public decimal? Price { get; set; }
        
        [Required]
        public CurrencyType? Currency { get; set; }

        [Required]
        public int? TicketProviderId { get; set; }

        public IEnumerable<int> BandIds { get; set; }
    }
}
