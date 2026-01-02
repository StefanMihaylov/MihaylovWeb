using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;
using Mihaylov.Common.Generic.Servises;
using Mihaylov.Common.Generic.Servises.Models;

namespace Mihaylov.Api.Other.Data.Cluster
{
    public class NexusApiService : HttpClientExtention, INexusApiService
    {
        public const string NEXUS_CLIENT_NAME = "NexusHttpClientName";

        private readonly NexusConfiguration _conifg;

        public NexusApiService(IHttpClientFactory httpFactory, ILoggerFactory loggerFactory,
            IOptions<NexusConfiguration> settings)
            : base(httpFactory, loggerFactory, NEXUS_CLIENT_NAME,
                  new ApiConfig(settings.Value.BaseUrl, settings.Value.Username, settings.Value.Password, null))
        {
            _conifg = settings.Value;
        }

        public async Task<NexusImages> GetDockerImagesAsync()
        {
            var images = new List<NexusImage>();

            string nextPage = null;
            do
            {
                var response = await GetDockerImagesAsync(nextPage).ConfigureAwait(false);

                var data = response.GetResponse();
                images.AddRange(data.Items.Select(a => new NexusImage()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Version = a.Version,
                    LastModified = a.Assets.FirstOrDefault()?.LastModified,
                    IsLocked = true,
                    Sha256 = a.Assets.FirstOrDefault()?.Checksum != null && a.Assets.FirstOrDefault().Checksum.ContainsKey("sha256") ?
                                a.Assets.FirstOrDefault().Checksum["sha256"] : null
                }));

                nextPage = response.Data.ContinuationToken;

            } while (!string.IsNullOrEmpty(nextPage));


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

            var result = images.GroupBy(a => new { a.Name, a.Sha256 })
                   .Select(g => new NexusImage()
                   {
                       Id = g.OrderByDescending(a => a.LastModified).First().Id,
                       Name = g.Key.Name,
                       Version = string.Join(" / ", g.Select(a => a.Version).OrderBy(a => a)),
                       LastModified = g.First().LastModified,
                       IsLocked = g.First().IsLocked,
                       Sha256 = g.First().Sha256
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

            return new NexusImages()
            {
                Images = result
            };
        }

        public async Task ClearImages()
        {
            var imageResponse = await GetDockerImagesAsync().ConfigureAwait(false);

            var images = imageResponse.Images.SelectMany(a => a.Value)
                                             .Where(a => a.IsLocked == false)
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

        private async Task<Response<NexusResponseWrapper>> GetDockerImagesAsync(string nextPage)
        {
            var request = new DockerImagesRequest(_conifg.RepositoryName, nextPage);

            var response = await GetResponse<DockerImagesRequest, NexusResponseWrapper, string, NexusResponseWrapper>(
                    HttpMethod.Get, $"/service/rest/v1/components", request, "GetDockerImages",
                                x => x, e => e).ConfigureAwait(false);

            return response;
        }
    }
}
