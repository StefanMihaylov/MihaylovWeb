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
        public string SiteUrl { get; set; }

        [JsonPropertyOrder(4)]
        public string ReleaseUrl { get; set; }

        [JsonPropertyOrder(5)]
        public string GithubVersionUrl { get; set; }

        [JsonPropertyOrder(6)]
        public string ResourceUrl { get; set; }

        [JsonPropertyOrder(7)]
        public DeploymentType Deployment { get; set; }

        [JsonPropertyOrder(8)]
        public string Notes { get; set; }

        [JsonPropertyOrder(9)]
        public int? Order { get; set; }
    }
}
