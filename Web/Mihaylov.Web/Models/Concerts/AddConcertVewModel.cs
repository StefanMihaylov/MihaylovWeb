using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.Web.Models.Concerts
{
    public class AddConcertVewModel
    {
        [Required]
        public DateTime? Date { get; set; }

        public string Name { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public int? Currency { get; set; }

        [Required]
        public int? Location { get; set; }

        [Required]
        public int? TicketProvider { get; set; } 

        public IEnumerable<int?> BandIds { get; set; }


        public IEnumerable<SelectListItem> Currencies { get; set; }

        public IEnumerable<BandExtended> Bands { get; set; }

        public IEnumerable<Location> Locations { get; set; }

        public IEnumerable<TicketProvider> TicketProviders { get; set; }
    }
}
