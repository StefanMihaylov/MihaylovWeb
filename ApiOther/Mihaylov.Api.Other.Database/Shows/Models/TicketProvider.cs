using System.Collections.Generic;
using Mihaylov.Common.Database.Models;

namespace Mihaylov.Api.Other.Database.Shows.Models
{
    public class TicketProvider : Entity
    {
        public int TickerProviderId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public IEnumerable<Concert> Concerts { get; set; }

        public TicketProvider()
        {
            Concerts = new List<Concert>();
        }
    }
}
