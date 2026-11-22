using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Common;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Mihaylov.Api.Other.Data.Cluster
{
    public class NugetClient : INugetClient
    {
        public const string NUGET_URL = "https://api.nuget.org/v3/index.json";

        public async Task<IEnumerable<DependencyNode>> GetMetadataAsync(string repositoryUrl, CancellationToken ct)
        {
            var logger = NullLogger.Instance;
            using var cache = new SourceCacheContext();
            var source = new PackageSource(repositoryUrl);

            var repo = Repository.Factory.GetCoreV3(source);

            var search = await repo.GetResourceAsync<PackageSearchResource>();
            var results = await search.SearchAsync("", new SearchFilter(includePrerelease: true), 0, 100, logger, ct);
            var packageIdentities = results.Select(p => p.Identity).ToList();

            var result = new List<DependencyNode>();

            var metadataResource = await repo.GetResourceAsync<PackageMetadataResource>();
            foreach (var packageIdentity in packageIdentities)
            {
                var packages = await metadataResource.GetMetadataAsync(packageIdentity.Id, false, false, cache, logger, ct);

                foreach (var package in packages)
                {
                    var nuget = CreateNode(package, package.DependencySets.FirstOrDefault());
                    if (nuget == null)
                    {
                        continue;
                    }

                    result.Add(nuget);
                }
            }

            return result;
        }

        public async Task<DependencyNode> GetMetadataByIdAsync(string repositoryUrl, string packageName, string version,
            string framework, CancellationToken ct)
        {
            var logger = NullLogger.Instance;
            using var cache = new SourceCacheContext();
            var source = new PackageSource(repositoryUrl);

            var repo = Repository.Factory.GetCoreV3(source);
            var metadataResource = await repo.GetResourceAsync<PackageMetadataResource>();

            var identity = new PackageIdentity(packageName, NuGetVersion.Parse(version));

            var package = await metadataResource.GetMetadataAsync(identity, cache, logger, ct);
            var nugetFramework = ParseFramework(framework);
            var group = NuGetFrameworkUtility.GetNearest(package.DependencySets, nugetFramework);
            var nuget = CreateNode(package, group);        

            return nuget;
        }

        private static DependencyNode CreateNode(IPackageSearchMetadata package, PackageDependencyGroup group)
        {
            if (group == null)
            {
                return null;   // no dependency group (or none compatible)
            }

            var dependencies = group.Packages
                .Select(d => new DependencyNode(d.Id, GetVersion(d.VersionRange), null, DependencyKind.Package))
                .ToList();

            return new DependencyNode(
                package.Identity.Id,
                package.Identity.Version.OriginalVersion,
                group.TargetFramework.GetShortFolderName(),
                DependencyKind.Package,
                dependencies);
        }

        private static string GetVersion(VersionRange range)
        {
            if (range.MaxVersion != null)
            {
                return range.MaxVersion.OriginalVersion;
            }
            else if (range.MinVersion != null)
            {
                return range.MinVersion.OriginalVersion;
            }

            return range.OriginalString;
        }

        private static NuGetFramework ParseFramework(string framework)
        {
            if (framework.Contains("Version="))  // ".NETCoreApp,Version=v8.0"
            {
                return NuGetFramework.ParseFrameworkName(framework, DefaultFrameworkNameProvider.Instance);
            }
            else
            {
                return NuGetFramework.Parse(framework);  // "net8.0", "netstandard2.1"
            }
        }
    }
}
