using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;
using Mihaylov.Api.Other.Data.Cluster.Models;
using Mihaylov.Common;

namespace Mihaylov.Api.Other.Data.Cluster
{
    public class NexusApiService : BaseHttpClient, INexusApiService
    {
        public const string NEXUS_CLIENT_NAME = "NexusHttpClientName";

        private readonly NexusConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly INugetClient _nugetClient;

        public NexusApiService(IHttpClientFactory httpFactory, ILoggerFactory loggerFactory, IOptions<NexusConfiguration> settings,
            IMemoryCache cache, INugetClient nugetClient)
            : base(httpFactory, loggerFactory, NEXUS_CLIENT_NAME,
                  new ApiConfig(settings.Value.BaseUrl, settings.Value.Username, settings.Value.Password, null))
        {
            _config = settings.Value;
            _cache = cache;
            _nugetClient = nugetClient;
        }

        public async Task<NexusImages> GetDockerImagesAsync()
        {
            var docker = await GetHostedRepositoryAsync("docker").ConfigureAwait(false);
            var dockerImages = await GetNexusItemsAsync(docker.Name, EnrichDockerImageAsync).ConfigureAwait(false);

            var runtimeData = await CalculateAppSizeAsync(dockerImages).ConfigureAwait(false);
            MarkLockedImages(dockerImages);

            var result = new NexusImages()
            {
                Images = GroupNexusItems(dockerImages),
                Runtimes = GetRuntimes(runtimeData),
            };

            return result;
        }

        public async Task<NugetPackages> GetNugetPackagesAsync()
        {
            var nuget = await GetHostedRepositoryAsync("nuget").ConfigureAwait(false);

            var packageMetadataDict = await GetNugetPackagesMetadataAsync(nuget.Url).ConfigureAwait(false);
            var nugetPackages = await GetNexusItemsAsync(nuget.Name, (image, _) => Task.FromResult(EnrichNugetImage(image, packageMetadataDict))).ConfigureAwait(false);

            var result = new NugetPackages()
            {
                LocalPackagePrefix = NugetClient.LOCAL_PACKAGE_PREFIX,
                Nugets = GroupNexusItems(nugetPackages),
            };

            return result;
        }

        public async Task<NugetPackage> GetNugetPackageAsync(string packageName, string version)
        {
            var repoUrl = NugetClient.NUGET_URL;
            if (_nugetClient.IsNugetLocal(packageName))
            {
                var nugetRepo = await GetHostedRepositoryAsync("nuget").ConfigureAwait(false);
                repoUrl = nugetRepo.Url;
            }

            var nuget = await _nugetClient.GetMetadataByIdAsync(repoUrl, packageName, version, CancellationToken.None).ConfigureAwait(false);

            return nuget;
        }

        public async Task ClearImages()
        {
            var imageResponse = await GetDockerImagesAsync().ConfigureAwait(false);

            var images = imageResponse.Images.Where(a => a.IsLocked == false)
                                             .ToList();

            foreach (var image in images)
            {
                var result = await DeleteDockerImageAsync(image.Id).ConfigureAwait(false);
                if (!result)
                {
                    _logger.LogWarning($"Failed to delete image with id: {image.Id}");
                }
            }
        }

        public async Task<bool> DeleteDockerImageAsync(string imageId)
        {
            var response = await GetResponse<string, string, string, string>(
                    HttpMethod.Delete, $"/service/rest/v1/components/{imageId}", null, "DeleteComponent",
                    x => x, e => e).ConfigureAwait(false);

            return response.IsSuccessful;
        }

        public async Task<NexusBlobs> GetBlobsAsync()
        {
            var response = await GetResponse<string, IEnumerable<NexusBlobResponse>, string, NexusBlobs>(
                    HttpMethod.Get, $"/service/rest/v1/blobstores", null, "GetBlobs",
                        x => new NexusBlobs(x), e => e).ConfigureAwait(false);

            return response.GetResponse();
        }

        public async Task RunTasksAsync()
        {
            var tasks = await GetTasksAsync("blobstore.compact").ConfigureAwait(false);

            foreach (var task in tasks.Items)
            {
                var result = await RunTaskAsync(task.Id).ConfigureAwait(false);
            }
        }


        /// <summary>Pages through all components of a Nexus repository.</summary>
        private async Task<IEnumerable<NexusResponseItem>> GetComponentsAsync(string repository)
        {
            var result = new List<NexusResponseItem>();

            string nextPage = null;
            do
            {
                var response = await GetComponentsPageAsync(repository, nextPage).ConfigureAwait(false);
                var data = response.GetResponse();

                result.AddRange(data.Items);

                nextPage = data.ContinuationToken;
            }
            while (!string.IsNullOrEmpty(nextPage));

            return result;
        }

        /// <summary>
        /// Get Docker images and Nuget packages info
        /// </summary>
        /// <param name="repository">Nexus repository name</param>
        /// <param name="customUpdateAction">Custom Update Action</param>
        /// <returns></returns>
        private async Task<IEnumerable<T>> GetNexusItemsAsync<T>(string repository, Func<NexusBaseImage, NexusResponseAsset, Task<T>> enrich) where T : NexusBaseImage
        {
            var result = new List<T>();

            var components = await GetComponentsAsync(repository).ConfigureAwait(false);

            foreach (var component in components)
            {
                var asset = component.Assets?.FirstOrDefault();
                var baseImage = CreateBaseImage(component, asset);
                var image = await enrich(baseImage, asset).ConfigureAwait(false);

                result.Add(image);
            }

            return result;
        }

        private static NexusBaseImage CreateBaseImage(NexusResponseItem item, NexusResponseAsset asset)
        {
            var checksum = asset?.Checksum != null && asset.Checksum.ContainsKey("sha256") ? asset.Checksum["sha256"] : null;

            return new NexusBaseImage
            {
                Id = item.Id,
                Name = item.Name,
                Version = item.Version,
                LastModified = asset?.LastModified,
                Sha256 = checksum,
                Runtime = null,
                Size = asset?.FileSize ?? 0,

                IsLocked = true,
                IsFirst = false,
            };
        }

        /// <summary>
        /// Get additional data for Docker image
        /// </summary>
        /// <param name="asset">Nexus response asset</param>
        /// <param name="image">Docker image metadata</param>
        private async Task<NexusDockerImage> EnrichDockerImageAsync(NexusBaseImage inputImage, NexusResponseAsset asset)
        {
            var image = new NexusDockerImage(inputImage);

            var manifestUrl = asset.DownloadUrl;
            var manifest = await GetDockerManifestDataAsync(manifestUrl).ConfigureAwait(false);
            if (manifest == null)
            {
                return image;
            }

            image.Size = manifest.Layers.Sum(a => a.Size);
            image.LayersCount = manifest.Layers.Count();

            var envVariables = asset.Docker?.Env?.ToList();
            if (envVariables?.Count > 0)
            {
                var runtimeVariable = envVariables?.FirstOrDefault(a => a.StartsWith("DOTNET_VERSION="));
                if (!string.IsNullOrWhiteSpace(runtimeVariable))
                {
                    var runtime = runtimeVariable?.Split('=')?.LastOrDefault();

                    image.Runtime = runtime;
                }
            }
            else
            {
                var blobDigest = manifest.Config.Digest;
                var blobUrl = manifestUrl.Replace("/manifests/", "/blobs/");
                var slashIndex = blobUrl.LastIndexOf('/');
                if (slashIndex >= 0)
                {
                    blobUrl = $"{blobUrl[..slashIndex]}/{blobDigest}";
                }

                var blobManifest = await GetDockerBlobDataAsync(blobUrl).ConfigureAwait(false);
                var envVariablesFromBlob = blobManifest?.Config?.Env?.ToList();
                if (envVariablesFromBlob?.Count > 0)
                {
                    var runtimeVariable = envVariablesFromBlob?.FirstOrDefault(a => a.StartsWith("DOTNET_VERSION="));
                    if (!string.IsNullOrWhiteSpace(runtimeVariable))
                    {
                        var runtime = runtimeVariable?.Split('=')?.LastOrDefault();

                        image.Runtime = runtime;
                    }
                }
            }

            return image;
        }

        private NexusNugetImage EnrichNugetImage(NexusBaseImage inputImage, IDictionary<string, NugetPackage> packageMetadataDict)
        {
            var image = new NexusNugetImage(inputImage);

            var key = $"{image.Name}_{image.Version}";
            if (packageMetadataDict.TryGetValue(key, out var packageMetadata))
            {
                image.Runtime = packageMetadata.Framework;
                image.Dependencies = packageMetadata.Dependencies;
            }

            return image;
        }

        /// <summary>
        /// Get Additional data - Application Size and Image layers count
        /// </summary>
        /// <param name="images">list of Nexus images</param>
        /// <returns>Runtime data list</returns>
        private async Task<IEnumerable<MicrosoftRuntimeData>> CalculateAppSizeAsync(IEnumerable<NexusDockerImage> images)
        {
            const string keyPrefix = "Nexus_Runtime_Data_";

            foreach (var image in images)
            {
                var runtime = image.Runtime;
                if (string.IsNullOrWhiteSpace(runtime))
                {
                    continue;
                }

                var key = $"{keyPrefix}{runtime}";
                if (!_cache.TryGetValue(key, out MicrosoftRuntimeData runtimeData))
                {
                    var manifestResponse = await GetMicrosoftManifestAsync(runtime).ConfigureAwait(false);
                    if (manifestResponse?.Manifests?.Any() != true)
                    {
                        continue;
                    }

                    var digest = manifestResponse.Manifests.Where(m => m.Platform?.Architecture == "amd64" && m.Platform?.Os == "linux").Select(m => m.Digest).SingleOrDefault();
                    if (string.IsNullOrWhiteSpace(digest))
                    {
                        continue;
                    }

                    var digestResponse = await GetMicrosoftManifestAsync(digest).ConfigureAwait(false);
                    var releaseDate = await GetRuntimeReleaseDateAsync(runtime).ConfigureAwait(false);

                    runtimeData = new MicrosoftRuntimeData()
                    {
                        Runtime = runtime,
                        ReleaseDate = releaseDate,
                        Layers = digestResponse.Layers ?? [],
                        Size = digestResponse.Layers?.Sum(l => l.Size) ?? 0,
                        LayersCount = digestResponse.Layers?.Count() ?? 0
                    };

                    _cache.Set(key, runtimeData, TimeSpan.FromHours(12));
                }

                image.AppSize = image.Size - runtimeData.Size;
                image.AppLayersCount = image.LayersCount - runtimeData.LayersCount;
            }

            var runtimes = images.Select(i => i.Runtime).Where(r => !string.IsNullOrWhiteSpace(r)).Distinct().ToList();
            var runtimeList = new List<MicrosoftRuntimeData>();

            foreach (var runtime in runtimes)
            {
                if (_cache.TryGetValue($"{keyPrefix}{runtime}", out MicrosoftRuntimeData data))
                {
                    runtimeList.Add(data);
                }
            }

            return runtimeList;
        }

        private void MarkLockedImages(IEnumerable<NexusBaseImage> images)
        {
            var imageDict = images.ToDictionary(a => a.Id, a => a);

            var skippedAge = DateTime.Now.Date.AddMonths(-_config.SkippedVersionMonthsAge);
            var ImagesForDeletion = images.GroupBy(a => a.Name)
                               .Select(g => new
                               {
                                   g.Key,
                                   Value = g.Where(a => a.Version != "latest")
                                            .OrderByDescending(a => a.LastModified)
                                            .ThenByDescending(a => a.Version)
                                            .Skip(_config.SkippedVersionCount)
                                            .Where(a => a.LastModified < skippedAge)
                               })
                               .SelectMany(g => g.Value);


            foreach (var image in ImagesForDeletion)
            {
                if (imageDict.ContainsKey(image.Id))
                {
                    imageDict[image.Id].IsLocked = false;
                }
            }
        }

        private static IEnumerable<MicrosoftRuntime> GetRuntimes(IEnumerable<MicrosoftRuntimeData> runtimeData)
        {
            var result = new List<MicrosoftRuntime>();
            runtimeData = runtimeData.OrderBy(r => Version.Parse(r.Runtime)).ToList();

            MicrosoftRuntimeData previous = null;
            foreach (var runtimeItem in runtimeData)
            {
                var runtime = new MicrosoftRuntime()
                {
                    Runtime = runtimeItem.Runtime,
                    ReleaseDate = runtimeItem.ReleaseDate,
                    Size = runtimeItem.Size,
                    LayersCount = runtimeItem.LayersCount,
                    SizeIncrease = previous != null ? (runtimeItem.Size - previous.Size) : null,
                    FootprintIncrease = GetFootprintIncrease(runtimeItem, previous)
                };

                result.Add(runtime);
                previous = runtimeItem;
            }

            return result.OrderByDescending(r => Version.Parse(r.Runtime)).ToList();

            static long? GetFootprintIncrease(MicrosoftRuntimeData runtimeItem, MicrosoftRuntimeData previous)
            {
                if (previous == null)
                {
                    return null;
                }

                var preciousLayers = new HashSet<string>(previous.Layers.Select(a => a.Digest), StringComparer.OrdinalIgnoreCase);
                var layers = runtimeItem.Layers.Select(a => new LayerExtended(a.Digest, a.Size, false));
                foreach (var layer in layers)
                {
                    if (preciousLayers.Contains(layer.Digest))
                    {
                        layer.IsReused = true;
                    }
                }

                return layers.Where(a => !a.IsReused).Sum(a => a.Size);
            }
        }

        private static IEnumerable<T> GroupNexusItems<T>(IEnumerable<T> images) where T : NexusBaseImage
        {
            var groups = images.GroupBy(i => i.Name)
                               .Select(byName => byName.GroupBy(i => i.Sha256 ?? i.Id)
                                                       .Select(MergeTags)
                                                       .OrderByDescending(i => i.LastModified)
                                                       .ThenByDescending(i => i.Version)
                                                       .ToList())
                               .OrderByDescending(g => g[0].LastModified)
                               .ToList();

            var result = new List<T>();
            foreach (var group in groups)
            {
                group[0].IsFirst = true;
                result.AddRange(group);
            }

            return result;
        }

        /// <summary>Keeps the newest item (so derived data survives) and joins the versions into it.</summary>
        private static T MergeTags<T>(IGrouping<string, T> sameContent) where T : NexusBaseImage
        {
            var main = sameContent.OrderByDescending(i => i.LastModified).First();

            main.Version = string.Join(" / ", sameContent.Select(i => i.Version).OrderBy(v => v, StringComparer.Ordinal));
            main.IsLocked = sameContent.Any(i => i.IsLocked);

            return main;
        }

        private async Task<NexusTasksWrapper> GetTasksAsync(string type)
        {
            var response = await GetResponse<string, NexusTasksWrapper, string, NexusTasksWrapper>(
                    HttpMethod.Get, $"/service/rest/v1/tasks?type={type}", null, "GetTasks",
                                x => x, e => e).ConfigureAwait(false);

            return response.GetResponse();
        }

        private async Task<bool> RunTaskAsync(string id)
        {
            var response = await GetResponse<string, string, string, string>(
                    HttpMethod.Post, $"/service/rest/v1/tasks/{id}/run", null, "RunTask",
                                x => x, e => e).ConfigureAwait(false);

            return response.IsSuccessful;
        }

        private async Task<Response<NexusComponentsResponse>> GetComponentsPageAsync(string repository, string nextPage)
        {
            var request = new DockerImagesRequest(repository, nextPage);

            var response = await GetResponse<DockerImagesRequest, NexusComponentsResponse, string, NexusComponentsResponse>(
                    HttpMethod.Get, $"/service/rest/v1/components", request, "GetDockerImages",
                                x => x, e => e).ConfigureAwait(false);

            return response;
        }

        private async Task<NexusManifestResponse> GetDockerManifestDataAsync(string manifestUrl)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, manifestUrl);
            var responseMessage = await base.GetResponseAsync(request).ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<NexusManifestResponse>(responseMessage.Body, base._jsonOptions);
                return response;
            }

            return null;
        }

        private async Task<NexusBlobManifestResponse> GetDockerBlobDataAsync(string blobUrl)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, blobUrl);
            var responseMessage = await base.GetResponseAsync(request).ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                var response = JsonSerializer.Deserialize<NexusBlobManifestResponse>(responseMessage.Body, base._jsonOptions);
                return response;
            }

            return null;
        }

        private async Task<NexusManifestResponse> GetMicrosoftManifestAsync(string runtime)
        {
            var headerList = new string[]
            {
                "application/vnd.oci.image.index.v1+json",
                "application/vnd.oci.image.manifest.v1+json",
                "application/vnd.docker.distribution.manifest.list.v2+json",
                "application/vnd.docker.distribution.manifest.v2+json"
            };

            var manifestUrl = $"https://mcr.microsoft.com/v2/dotnet/aspnet/manifests/{runtime}";
            using var manifestRequest = new HttpRequestMessage(HttpMethod.Get, manifestUrl);
            manifestRequest.Headers.Add("Accept", string.Join(',', headerList));

            var responseMessage = await base.GetResponseAsync(manifestRequest).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var manifestResponse = JsonSerializer.Deserialize<NexusManifestResponse>(responseMessage.Body, base._jsonOptions);
            return manifestResponse;
        }

        private async Task<NexusRepositoryResponse> GetHostedRepositoryAsync(string repositoryType)
        {
            var repositories = await GetRepositoriesAsync().ConfigureAwait(false);

            var repository = repositories.FirstOrDefault(r => r.Type.Equals("hosted", StringComparison.OrdinalIgnoreCase)
                                                            && r.Format.Equals(repositoryType, StringComparison.OrdinalIgnoreCase));
            return repository;
        }

        private async Task<IEnumerable<NexusRepositoryResponse>> GetRepositoriesAsync()
        {
            const string key = "Nexus_Repository";
            if (!_cache.TryGetValue(key, out IEnumerable<NexusRepositoryResponse> repositories))
            {
                var response = await GetResponse<string, IEnumerable<NexusRepositoryResponse>, string, IEnumerable<NexusRepositoryResponse>>(
                        HttpMethod.Get, $"/service/rest/v1/repositories", null, "GetRepositories",
                        x => x, e => e).ConfigureAwait(false);

                repositories = response.GetResponse().Where(r => r.Online).ToList();

                _cache.Set(key, repositories, TimeSpan.FromHours(12));
            }

            return repositories;
        }

        private async Task<IDictionary<string, NugetPackage>> GetNugetPackagesMetadataAsync(string nugetBaseUrl)
        {
            var packagesMetadata = await _nugetClient.GetMetadataAsync(nugetBaseUrl, CancellationToken.None);

            var packageMetadataDict = new Dictionary<string, NugetPackage>(StringComparer.OrdinalIgnoreCase);
            foreach (var package in packagesMetadata)
            {
                var key = $"{package.Title}_{package.Version}";
                if (!packageMetadataDict.ContainsKey(key))
                {
                    packageMetadataDict.Add(key, package);
                }
            }

            return packageMetadataDict;
        }

        private async Task<DateTime?> GetRuntimeReleaseDateAsync(string runtime)
        {
            var parts = runtime.Split('.');
            if (parts.Length < 2)
            {
                return null;
            }

            var channel = $"{parts[0]}.{parts[1]}";    // "6.0"
            var key = $"Nexus_Runtime_Release_Dates_{channel}";

            if (!_cache.TryGetValue(key, out Dictionary<string, DateTime> dates))
            {
                using var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://builds.dotnet.microsoft.com/dotnet/release-metadata/{channel}/releases.json");

                var response = await base.GetResponseAsync(request).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var data = JsonSerializer.Deserialize<DotnetReleasesResponse>(response.Body, base._jsonOptions);

                dates = new Dictionary<string, DateTime>();
                foreach (var release in data?.Releases ?? new List<DotnetRelease>())
                {
                    dates.TryAdd(release.ReleaseVersion, release.ReleaseDate.ToDateTime(TimeOnly.MinValue));
                }

                _cache.Set(key, dates, TimeSpan.FromHours(12));
            }

            return dates.TryGetValue(runtime, out var date) ? date : null;
        }
    }

    internal class LayerExtended
    {
        public string Digest { get; }

        public long Size { get; }

        public bool IsReused { get; set; }

        public LayerExtended(string digest, long size, bool isReused)
        {
            Digest = digest;
            Size = size;
            IsReused = isReused;
        }
    }
}
