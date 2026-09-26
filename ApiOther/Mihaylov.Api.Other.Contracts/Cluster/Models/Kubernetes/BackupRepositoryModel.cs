namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public class BackupRepositoryModel
    {
        public string Name { get; set; }

        public string BackupStorageLocation { get; set; }

        public string VolumeNamespace { get; set; }

        public string RepositoryType { get; set; }
    }
}
