using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using Mihaylov.Common.Generic.Servises.Models;

namespace Mihaylov.Common.Generic.Servises
{
    public abstract class HttpClientExtention
    {
        private readonly IHttpClientFactory _httpClientFactory;
        protected readonly ILogger _logger;

        private readonly ApiConfig _config;
        private readonly string _httpClientName;

        protected readonly JsonSerializerOptions _jsonOptions;

        public HttpClientExtention(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory,
            string httpClientName, ApiConfig config)
        {
            _httpClientFactory = httpClientFactory;
            _logger = loggerFactory.CreateLogger(this.GetType());
            _config = config;
            _httpClientName = httpClientName;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<Response<TResponse>> GetResponse<TRequest, TResS, TResF, TResponse>
            (HttpMethod method, string relUrl, TRequest requestBody, string methodName, Func<TResS, TResponse> outputMap,
            Func<TResF, string> errorMap) where TRequest : class where TResponse : class
        {
            try
            {
                _logger.LogInformation($"Calling {methodName}...");

                var url = $"{_config.BaseUrl.TrimEnd('/')}{relUrl}";

                if ((method == HttpMethod.Get || method == HttpMethod.Delete) && requestBody != null)
                {
                    url += $"?{GetQueryString(requestBody)}";
                }

                var request = new HttpRequestMessage(method, url);
                request.Headers.Add("Accept", "application/json");

                if (!string.IsNullOrEmpty(_config.Username) && !string.IsNullOrEmpty(_config.Password))
                {
                    var authenticationString = $"{_config.Username}:{_config.Password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                }
                else if (!string.IsNullOrEmpty(_config.ApiKey))
                {
                    request.Headers.Add("x-api-key", _config.ApiKey);
                }

                if ((method == HttpMethod.Post || method == HttpMethod.Put) && requestBody != null)
                {
                    var requestString = JsonSerializer.Serialize(requestBody);
                    request.Content = new StringContent(requestString, Encoding.UTF8, "application/json");
                }

                BaseHttpResponse response = await GetResposeAsync(request).ConfigureAwait(false);

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
                    if (typeof(TResS) == typeof(string) && typeof(TResponse) == typeof(string))
                    {
                        result = response.Body as TResponse;
                    }
                    else
                    {
                        TResS resultResponse = JsonSerializer.Deserialize<TResS>(response.Body, _jsonOptions);
                        result = outputMap(resultResponse);
                    }
                }

                return new Response<TResponse>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{methodName}.failed. Error message: {ex.Message}");
                return new Response<TResponse>(ex.Message);
            }
        }

        public async Task<Response<Stream>> DownloadFileAsync<TRequest>(string relUrl, TRequest requestBody, string methodName) where TRequest : class
        {
            try
            {
                _logger.LogInformation($"Calling {methodName}...");

                var url = $"{_config.BaseUrl.TrimEnd('/')}{relUrl}";

                if (requestBody != null)
                {
                    url += $"?{GetQueryString(requestBody)}";
                }

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Accept", "application/octet-stream");

                if (!string.IsNullOrEmpty(_config.Username) && !string.IsNullOrEmpty(_config.Password))
                {
                    var authenticationString = $"{_config.Username}:{_config.Password}";
                    var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                }
                else if (!string.IsNullOrEmpty(_config.ApiKey))
                {
                    request.Headers.Add("x-api-key", _config.ApiKey);
                }

                using HttpResponseMessage responseMessage = await GetHttpResponseAsync(request).ConfigureAwait(false);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    string errorMessage = responseMessage.StatusCode.ToString();
                    // string responseBody = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return new Response<Stream>(errorMessage);
                }

                var responseStream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);

                var fileStream = new MemoryStream();
                responseStream.CopyTo(fileStream);
                fileStream.Position = 0;

                return new Response<Stream>(fileStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{methodName}.failed. Error message: {ex.Message}");
                return new Response<Stream>(ex.Message);
            }
        }

        private async Task<BaseHttpResponse> GetResposeAsync(HttpRequestMessage request)
        {
            using HttpResponseMessage responseMessage = await GetHttpResponseAsync(request).ConfigureAwait(false);
            string response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            request.Dispose();

            return new BaseHttpResponse(response, responseMessage);
        }

        private async Task<HttpResponseMessage> GetHttpResponseAsync(HttpRequestMessage request)
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient(_httpClientName);
            HttpResponseMessage responseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);

            return responseMessage;
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
