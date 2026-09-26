using k8s.Models;

namespace Mihaylov.Api.Other.Data.Cluster.Models;

public class BackupStorageLocationList
{
    public string ApiVersion { get; set; }

    public string Kind { get; set; }

    public BackupStorageLocationModelInternal[] Items { get; set; }

    public V1ListMeta Metadata { get; set; }
}

public class BackupStorageLocationModelInternal
{
    public V1ObjectMeta Metadata { get; set; }

    public BackupStorageLocationModelSpec Spec { get; set; }

    public BackupStorageLocationModelStatus Status { get; set; }
}

public class BackupStorageLocationModelSpec
{
    public string AccessMode { get; set; }

    public string Provider { get; set; }

    public bool? Default { get; set; }

    public BackupStorageLocationConfig Config { get; set; }

    public BackupStorageLocationObjectStorage ObjectStorage { get; set; }

    public BackupStorageLocationCredential Credential { get; set; }
}

public class BackupStorageLocationConfig
{
    public string PublicUrl { get; set; }
    
    public string S3Url { get; set; }

    public string Region { get; set; }
}

public class BackupStorageLocationObjectStorage
{
    public string Bucket { get; set; }
}

public class BackupStorageLocationCredential
{
    public string Name { get; set; }

    public string Key { get; set; }
}

public class BackupStorageLocationModelStatus
{
    public string Phase { get; set; }
}