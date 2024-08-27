using System.Text.Json.Serialization;

namespace Mihaylov.Api.Other.Contracts.Cluster.Models
{
    public class Application
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(2)]
        public string Name { get; set; }

        [JsonPropertyOrder(3)]
        public string ReleaseUrl { get; set; }

        [JsonPropertyOrder(4)]
        public string ResourceUrl { get; set; }

        [JsonPropertyOrder(5)]
        public DeploymentType Deployment { get; set; }

        [JsonPropertyOrder(6)]
        public string Notes { get; set; }
    }
}
