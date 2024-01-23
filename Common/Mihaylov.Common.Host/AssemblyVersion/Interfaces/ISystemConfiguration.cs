namespace Mihaylov.Common.Host.AssemblyVersion.Interfaces
{
    public interface ISystemConfiguration
    {
        string GetGitCommit();
        string GetVersion();
    }
}