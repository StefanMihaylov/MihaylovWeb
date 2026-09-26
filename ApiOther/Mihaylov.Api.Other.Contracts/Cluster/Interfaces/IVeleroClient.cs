namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IVeleroClient
    {
        string GetVersion();

        string CreateBackup(string scheduleName);

        public string DeleteBackup(string backupName);
    }
}