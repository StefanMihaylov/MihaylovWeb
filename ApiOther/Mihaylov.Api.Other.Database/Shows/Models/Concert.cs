using System;
using System.Collections.Generic;
using Mihaylov.Common.Abstract.Databases.Models;

namespace Mihaylov.Api.Other.Database.Shows.Models
{
    public class Concert : Entity
    {
        public int ConcertId { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public int LocationId { get; set; }

        public Location Location { get; set; }

        public decimal Price { get; set; }

        public byte CurrencyId { get; set; }

        public Currency Currency { get; set; }

        public int TicketProviderId { get; set; }

        public TicketProvider TicketProvider { get; set; }


        public IEnumerable<Band> Bands { get; set; }

        public ICollection<ConcertBand> ConcertBands { get; set; }

        public Concert()
        {
            Bands = new List<Band>();
            ConcertBands = new List<ConcertBand>();
        }
    }
}
