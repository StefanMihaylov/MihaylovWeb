namespace Mihaylov.Common.Host.AssemblyVersion.Interfaces
{
    public interface ISystemConfiguration
    {
        string GetVersion();

        string GetGitCommit();

        string GetJenkinsBuildNumber();
    }
}