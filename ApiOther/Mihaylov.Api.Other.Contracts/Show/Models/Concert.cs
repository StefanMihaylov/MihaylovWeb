using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mihaylov.Api.Other.Contracts.Show.Models
{
    public class Concert
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(2)]
        public DateTime Date { get; set; }

        [JsonPropertyOrder(3)]
        public string Name { get; set; }

        [JsonPropertyOrder(4)]
        public int LocationId { get; set; }

        [JsonPropertyOrder(5)]
        public decimal Price { get; set; }

        [JsonPropertyOrder(6)]
        public CurrencyType Currency { get; set; }

        [JsonPropertyOrder(7)]
        public int TicketProviderId { get; set; }

        [JsonPropertyOrder(8)]
        public IEnumerable<Band> Bands { get; set; }
    }
}
