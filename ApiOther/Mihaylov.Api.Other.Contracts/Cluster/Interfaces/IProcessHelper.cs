namespace Mihaylov.Api.Other.Contracts.Cluster.Interfaces
{
    public interface IProcessHelper
    {
        string ExecuteCommand(string exePath, string command);
    }
}