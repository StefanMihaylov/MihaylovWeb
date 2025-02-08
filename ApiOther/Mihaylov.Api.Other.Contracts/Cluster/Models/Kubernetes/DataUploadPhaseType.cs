namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public enum DataUploadPhaseType
    {
        New = 1,
        Accepted = 2,
        Prepared = 3,
        InProgress = 4,
        Canceling = 5,
        Canceled = 6,
        Completed = 7,
        Failed = 8,
    }
}
