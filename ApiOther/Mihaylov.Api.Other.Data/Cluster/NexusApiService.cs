using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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
    public class NexusApiService : HttpClientExtention, INexusApiService
    {
        public const string NEXUS_CLIENT_NAME = "NexusHttpClientName";

        private readonly NexusConfiguration _conifg;
        private readonly IMemoryCache _cache;

        public NexusApiService(IHttpClientFactory httpFactory, ILoggerFactory loggerFactory, IOptions<NexusConfiguration> settings, IMemoryCache cache)
            : base(httpFactory, loggerFactory, NEXUS_CLIENT_NAME,
                  new ApiConfig(settings.Value.BaseUrl, settings.Value.Username, settings.Value.Password, null))
        {
            _conifg = settings.Value;
            _cache = cache;
        }

        public async Task<NexusImages> GetDockerImagesAsync()
        {
            var images = await GetAllDockerImagesAsync(_conifg.DockerRepositoryName, (asset, image) => CustomImageUpdates(asset, image)).ConfigureAwait(false);
            await CalculateAppSizeAsync(images).ConfigureAwait(false);
            MarkLockedImages(images);

            var nugets = await GetAllDockerImagesAsync(_conifg.NugetRepositoryName, null).ConfigureAwait(false);

            var result = new NexusImages()
            {
                Images = GroupNexusItems(images),
                Nugets = GroupNexusItems(nugets),
            };

            return result;
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
                    HttpMethod.Delete, $"/service/rest/v1/components/{imageId}", null, "DeleteDockerImages",
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


        private async Task<IEnumerable<NexusImage>> GetAllDockerImagesAsync(string repository, Action<NexusResponseAsset, NexusImage> customUpdateAction)
        {
            var images = new List<NexusImage>();

            string nextPage = null;
            do
            {
                var response = await GetDockerImagesAsync(repository, nextPage).ConfigureAwait(false);

                var data = response.GetResponse();
                images.AddRange(data.Items.Select(a =>
                {
                    var asset = a.Assets.FirstOrDefault();
                    var checksum = asset?.Checksum != null && asset.Checksum.ContainsKey("sha256") ? asset.Checksum["sha256"] : null;

                    var image = new NexusImage()
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Version = a.Version,
                        LastModified = asset?.LastModified,
                        Sha256 = checksum,
                        Runtime = null,
                        Size = asset?.FileSize ?? 0,
                        LayersCount = null,
                        AppSize = null,
                        AppLayersCount = null,

                        IsLocked = true,
                        IsFirst = false,
                    };

                    if (asset != null && customUpdateAction != null)
                    {
                        customUpdateAction(asset, image);
                    }

                    return image;
                }));

                nextPage = response.Data.ContinuationToken;

            } while (!string.IsNullOrEmpty(nextPage));

            return images;
        }

        private void CustomImageUpdates(NexusResponseAsset asset, NexusImage image)
        {
            var manifestUrl = asset.DownloadUrl;
            var manifest = GetDockerManifestDataAsync(manifestUrl).GetAwaiter().GetResult();
            if (manifest == null)
            {
                return;
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

                var blobManifest = GetDockerBlobDataAsync(blobUrl).GetAwaiter().GetResult();
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
        }

        private async Task CalculateAppSizeAsync(IEnumerable<NexusImage> images)
        {
            string keypPrefix = "Nexus_Runtime_Data_";

            var runtimes = images.Select(i => i.Runtime).Where(r => !string.IsNullOrWhiteSpace(r)).Distinct().ToList();

            foreach (var image in images)
            {
                var runtime = image.Runtime;
                if (string.IsNullOrWhiteSpace(runtime))
                {
                    continue;
                }

                var key = $"{keypPrefix}{runtime}";
                if (!_cache.TryGetValue(key, out RuntimeData data))
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

                    var size = digestResponse.Layers?.Sum(l => l.Size) ?? 0;
                    var layerCount = digestResponse.Layers?.Count() ?? 0;

                    data = new RuntimeData(size, layerCount);
                    _cache.Set(key, data, TimeSpan.FromHours(12));
                }

                image.AppSize = image.Size - data.Size;
                image.AppLayersCount = image.LayersCount - data.LayersCount;
            }
        }

        private void MarkLockedImages(IEnumerable<NexusImage> images)
        {
            var imageDict = images.ToDictionary(a => a.Id, a => a);

            var skippedAge = DateTime.Now.Date.AddMonths(-_conifg.SkippedVersionMonthsAge);
            var ImagesForDeletion = images.GroupBy(a => a.Name)
                               .Select(g => new
                               {
                                   g.Key,
                                   Value = g.Where(a => a.Version != "latest")
                                            .OrderByDescending(a => a.LastModified)
                                            .ThenByDescending(a => a.Version)
                                            .Skip(_conifg.SkippedVersionCount)
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

        private static IEnumerable<NexusImage> GroupNexusItems(IEnumerable<NexusImage> images)
        {
            var dictionary = images.GroupBy(a => new { a.Name, a.Sha256 })
                   .Select(g => new NexusImage()
                   {
                       Id = g.First().Id,
                       Name = g.Key.Name,
                       Version = string.Join(" / ", g.Select(a => a.Version).OrderBy(a => a)),
                       LastModified = g.First().LastModified,
                       IsLocked = g.First().IsLocked,
                       Sha256 = g.First().Sha256,
                       Runtime = g.First().Runtime,
                       Size = g.First().Size,
                       LayersCount = g.First().LayersCount,
                       AppSize = g.First().AppSize,
                       AppLayersCount = g.First().AppLayersCount,
                   })
                   .GroupBy(a => a.Name)
                   .Select(g => new
                   {
                       Name = g.Key,
                       Value = g.OrderByDescending(a => a.LastModified)
                                .ThenByDescending(a => a.Version)
                                .AsEnumerable(),
                       LastModified = g.Max(a => a.LastModified)
                   })
                   .OrderByDescending(g => g.LastModified)
                   .ToDictionary(a => a.Name, a => a.Value);


            var result = new List<NexusImage>();

            foreach (var itemKey in dictionary.Keys)
            {
                var values = dictionary[itemKey];
                values.First().IsFirst = true;

                result.AddRange(values);
            }

            return result;
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

        private async Task<Response<NexusComponentsResponse>> GetDockerImagesAsync(string repository, string nextPage)
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
            var responseMessage = await base.GetResposeAsync(request).ConfigureAwait(false);

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
            var responseMessage = await base.GetResposeAsync(request).ConfigureAwait(false);

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

            var responseMessage = await base.GetResposeAsync(manifestRequest).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var manifestResponse = JsonSerializer.Deserialize<NexusManifestResponse>(responseMessage.Body, base._jsonOptions);
            return manifestResponse;
        }

        private record RuntimeData(long Size, int LayersCount);
    }
}
