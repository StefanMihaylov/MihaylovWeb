using System.Text.Json.Serialization;

namespace Mihaylov.Api.Other.Contracts.Show.Models
{
    public class Band
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(2)]
        public string Name { get; set; }

        [JsonPropertyOrder(3)]
        public int? CountryId { get; set; }

        [JsonPropertyOrder(4)]
        public string Country { get; set; }
    }
}
