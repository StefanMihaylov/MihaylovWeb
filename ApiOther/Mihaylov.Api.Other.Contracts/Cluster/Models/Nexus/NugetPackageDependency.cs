namespace Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus
{
    public class NugetPackageDependency
    {
        public string Title { get; set; }

        public string Version { get; set; }

        public bool IsLocal { get; set; }

        public NugetPackageDependency()
        {
        }

        public NugetPackageDependency(string title, string version, bool isLocal)
        {
            Title = title;
            Version = version;
            IsLocal = isLocal;
        }

        public override string ToString()
        {
            return $"{Title} {Version} ({(IsLocal ? "Local" : "Nuget")})";
        }
    }
}
