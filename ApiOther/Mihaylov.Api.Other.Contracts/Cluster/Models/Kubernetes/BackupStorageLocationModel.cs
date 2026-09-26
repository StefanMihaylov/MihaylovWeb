namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class BackupStorageLocationModel
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public string AccessMode { get; set; }
        
        public string Provider { get; set; }

        public bool? Default { get; set; }

        public string ConfigPublicUrl { get; set; }

        public string ConfigS3Url { get; set; }

        public string ConfigRegion { get; set; }

        public string Bucket { get; set; }

        public string SecretName { get; set; }

        public string SecretKey { get; set; }
    }
}