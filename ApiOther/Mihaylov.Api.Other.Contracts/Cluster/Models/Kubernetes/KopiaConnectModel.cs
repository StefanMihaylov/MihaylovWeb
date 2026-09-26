namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class KopiaConnectModel
    {
        public string LocationName { get; set; }

        public string Bucket { get; set; }

        public string StorageUrl { get; set; }

        public string VolumeNamespace { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Password { get; set; }
    }
}
