using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Other.Contracts.Cluster.Interfaces;
using Mihaylov.Api.Other.Contracts.Cluster.Models.Nexus;

namespace Mihaylov.Api.Other.Data.Cluster
{
    public class NexusApiService : INexusApiService
    {
        public const string NEXUS_CLIENT_NAME = "NexusHttpClientName";

        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly NexusConfiguration _conifg;

        public NexusApiService(ILoggerFactory loggerFactory, IHttpClientFactory httpFactory,
            IOptions<NexusConfiguration> settings)
        {
            _logger = loggerFactory.CreateLogger<NexusApiService>();
            _httpClientFactory = httpFactory;
            _conifg = settings.Value;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
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

            var result = images.GroupBy(a => a.Name)
                               .Select(g => new
                               {
                                   g.Key,
                                   Value = g.OrderByDescending(a => a.LastModified)
                                            .ThenByDescending(a => a.Version)
                                            .AsEnumerable(),
                                   LastModified = g.Max(a => a.LastModified)
                               })
                               .OrderByDescending(g => g.LastModified)
                               .ToDictionary(a => a.Key, a => a.Value);

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


        private async Task<Response<TResponse>> GetResponse<TRequest, TResS, TResF, TResponse>
            (HttpMethod method, string relUrl, TRequest requestBody, string methodName, Func<TResS, TResponse> outputMap,
            Func<TResF, string> errorMap) where TRequest : class where TResponse : class
        {
            try
            {
                _logger.LogInformation($"Calling {methodName}...");

                var url = $"{_conifg.BaseUrl.TrimEnd('/')}{relUrl}";

                if ((method == HttpMethod.Get || method == HttpMethod.Delete) && requestBody != null)
                {
                    url += $"?{GetQueryString(requestBody)}";
                }

                var request = new HttpRequestMessage(method, url);
                request.Headers.Add("Accept", "application/json");

                if (!string.IsNullOrEmpty(_conifg.Username) && !string.IsNullOrEmpty(_conifg.Password))
                {
                    var authenticationString = $"{_conifg.Username}:{_conifg.Password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                }

                if ((method == HttpMethod.Post || method == HttpMethod.Put) && requestBody != null)
                {
                    var requestString = JsonSerializer.Serialize(requestBody);
                    request.Content = new StringContent(requestString, Encoding.UTF8, "application/json");
                }

                BaseHttpResponse response = await GetRespose(request).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = response.StatusCode.ToString();
                    if (!string.IsNullOrEmpty(response.Body))
                    {
                        TResF errorResponse = JsonSerializer.Deserialize<TResF>(response.Body, _jsonOptions);
                        errorMessage = errorMap(errorResponse) ?? response.Body;
                    }

                    return new Response<TResponse>(errorMessage);
                }

                TResponse result = default;
                if (!string.IsNullOrEmpty(response.Body))
                {
                    TResS resultResponse = JsonSerializer.Deserialize<TResS>(response.Body, _jsonOptions);
                    result = outputMap(resultResponse);
                }

                return new Response<TResponse>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{methodName}.failed. Error message: {ex.Message}");
                return new Response<TResponse>(ex.Message);
            }
        }

        private async Task<BaseHttpResponse> GetRespose(HttpRequestMessage request)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(NEXUS_CLIENT_NAME);
            HttpResponseMessage responseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);
            string response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            request.Dispose();

            return new BaseHttpResponse(response, responseMessage);
        }

        private string GetQueryString<T>(T request) where T : class
        {
            var queries = new List<string>();

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                string name = JsonNamingPolicy.CamelCase.ConvertName(property.Name);
                string value = property.GetValue(request, null)?.ToString();

                if (!string.IsNullOrEmpty(value))
                {
                    queries.Add($"{name}={HttpUtility.UrlEncode(value)}");
                }
            }

            return string.Join("&", queries);
        }
    }
}
