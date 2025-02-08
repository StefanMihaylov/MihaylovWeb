namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Kubernetes
{
    public enum BackupPhaseType
    {
        New = 1,
        FailedValidation = 2,
        InProgress = 3,
        WaitingForPluginOperations = 4,
        WaitingForPluginOperationsPartiallyFailed = 5,
        Finalizing = 6,
        FinalizingPartiallyFailed = 7,
        Completed = 8,
        PartiallyFailed = 9,
        Failed = 10,
        Deleting = 11,
    }
}
