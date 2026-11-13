using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;
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
        public const string LOCAL_PACKAGE_PREFIX = "Mihaylov";

        public async Task<IEnumerable<NugetPackage>> GetMetadataAsync(string repositoryUrl, CancellationToken ct)
        {
            var logger = NullLogger.Instance;
            using var cache = new SourceCacheContext();
            var source = new PackageSource(repositoryUrl);

            var repo = Repository.Factory.GetCoreV3(source);

            var search = await repo.GetResourceAsync<PackageSearchResource>();
            var results = await search.SearchAsync("", new SearchFilter(includePrerelease: true), 0, 100, logger, ct);
            var packageIdentities = results.Select(p => p.Identity).ToList();

            var result = new List<NugetPackage>();

            var metadataResource = await repo.GetResourceAsync<PackageMetadataResource>();
            foreach (var packageIdentity in packageIdentities)
            {
                var packages = await metadataResource.GetMetadataAsync(packageIdentity.Id, false, false, cache, logger, ct);

                foreach (var package in packages)
                {
                    var nuget = GetPackageData(package);
                    if (nuget == null)
                    {
                        continue;
                    }

                    result.Add(nuget);
                }
            }

            return result;
        }

        public async Task<NugetPackage> GetMetadataByIdAsync(string repositoryUrl, string packageName, string version, CancellationToken ct)
        {
            var logger = NullLogger.Instance;
            using var cache = new SourceCacheContext();
            var source = new PackageSource(repositoryUrl);

            var repo = Repository.Factory.GetCoreV3(source);
            var metadataResource = await repo.GetResourceAsync<PackageMetadataResource>();

            var identity = new PackageIdentity(packageName, NuGetVersion.Parse(version));

            var package = await metadataResource.GetMetadataAsync(identity, cache, logger, ct);
            var nuget = GetPackageData(package);

            return nuget;
        }

        public bool IsNugetLocal(string packageName)
        {
            return packageName?.StartsWith(LOCAL_PACKAGE_PREFIX) ?? false;
        }


        private NugetPackage GetPackageData(IPackageSearchMetadata package)
        {
            var dependencySetCount = package.DependencySets.Count();
            if (dependencySetCount == 0)
            {
                return null;
            }


            PackageDependencyGroup dependencySet;
            if (dependencySetCount == 1)
            {
                dependencySet = package.DependencySets.First();
            }
            else
            {
                var frameworkList = new[]
                {
                    new { Framework = "net12.0", Order = 1 },
                    new { Framework = "net10.0", Order = 2 },
                    new { Framework = "netstandard2.1", Order = 3 },
                    new { Framework = "netstandard2.0", Order = 4 },
                };

                dependencySet = package.DependencySets.Select(d => new
                {
                    DependencySet = d,
                    Framework = GetFrameworkName(d.TargetFramework)
                })
                .Join(frameworkList, d => d.Framework, f => f.Framework, (d, f) => new
                {
                    d.DependencySet,
                    f.Order,
                })
                .OrderBy(a => a.Order)
                .Select(a => a.DependencySet)
                .FirstOrDefault();

                if (dependencySet == null)
                {
                    return null;
                }
            }

            var dependencies = dependencySet.Packages
                        .Select(d => new NugetPackageDependency(d.Id, GetVersion(d.VersionRange), IsNugetLocal(d.Id)))
                        .ToList();

            var line = new NugetPackage()
            {
                Title = package.Identity.Id,
                Version = package.Identity.Version.OriginalVersion,
                Framework = GetFrameworkName(dependencySet.TargetFramework),
                Dependencies = dependencies,
            };

            return line;
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

        private static string GetFrameworkName(NuGetFramework nugetFramework)
        {
            string framework = string.Empty;

            var frameworkName = nugetFramework.Framework;
            if (frameworkName.Contains("netstandard", StringComparison.OrdinalIgnoreCase))
            {
                framework = "netstandard";
            }
            else if (frameworkName.Contains("netcoreapp", StringComparison.OrdinalIgnoreCase))
            {
                framework = "net";
            }

            string version = nugetFramework.Version.ToString(2);
            var result = $"{framework}{version}";

            return result;
        }
    }
}
