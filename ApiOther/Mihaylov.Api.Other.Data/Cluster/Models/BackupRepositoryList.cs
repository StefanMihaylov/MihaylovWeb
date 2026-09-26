using k8s.Models;

namespace Mihaylov.Api.Other.Data.Cluster.Models;

public class BackupRepositoryList
{
    public string ApiVersion { get; set; }

    public string Kind { get; set; }

    public BackupRepositoryModelInternal[] Items { get; set; }

    public V1ListMeta Metadata { get; set; }
}

public class BackupRepositoryModelInternal
{
    public V1ObjectMeta Metadata { get; set; }

    public BackupRepositoryModelSpec Spec { get; set; }

    public BackupRepositoryModelStatus Status { get; set; }
}

public class BackupRepositoryModelSpec
{
    public string BackupStorageLocation { get; set; }

    public string VolumeNamespace { get; set; }

    public string RepositoryType { get; set; }
}

public class BackupRepositoryModelStatus
{
    public string Phase { get; set; }
}
