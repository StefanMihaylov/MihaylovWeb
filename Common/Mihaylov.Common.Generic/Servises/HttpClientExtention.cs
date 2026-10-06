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

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides common HTTP request and response handling utilities for derived classes using IHttpClientFactory,
    /// ILogger, and System.Text.Json for serialization.
    /// </summary>
    public abstract class HttpClientExtention
    {
        private readonly IHttpClientFactory _httpClientFactory;        
        private readonly ApiConfig _config;
        private readonly string _httpClientName;

        /// <summary>
        /// Logger instance used by this class and its derived types.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// JsonSerializerOptions used for JSON serialization and deserialization by this class and its derived types.
        /// </summary>
        protected readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Initializes a new HttpClientExtention and configures its dependencies and JSON serializer options.
        /// </summary>
        /// <param name="httpClientFactory">Factory used to create HttpClient instances.</param>
        /// <param name="loggerFactory">Factory used to create an ILogger for logging.</param>
        /// <param name="httpClientName">Name of the HttpClient to request from the factory.</param>
        /// <param name="config">API configuration settings.</param>
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

        /// <summary>
        /// Sends an HTTP request to the configured base URL and returns a <![CDATA[Response<TResponse>]]> containing the mapped
        /// result or an error message.
        /// </summary>
        /// <typeparam name="TRequest">Type of the request body sent for POST/PUT or converted to a query string for GET/DELETE.</typeparam>
        /// <typeparam name="TResS">Type used to deserialize a successful response payload.</typeparam>
        /// <typeparam name="TResF">Type used to deserialize an error response payload.</typeparam>
        /// <typeparam name="TResponse">Type of the mapped result returned inside <![CDATA[Response<TResponse>]]>.</typeparam>
        /// <param name="method">HTTP method to use for the request.</param>
        /// <param name="relUrl">Relative URL appended to the configured BaseUrl.</param>
        /// <param name="requestBody">Request body instance or null; serialized as JSON for POST/PUT or converted to a query string for
        /// GET/DELETE.</param>
        /// <param name="methodName">Operation name used for logging.</param>
        /// <param name="outputMap">Function that maps a deserialized successful response (TResS) to the returned TResponse.</param>
        /// <param name="errorMap">Function that maps a deserialized error response (TResF) to an error message string.</param>
        /// <returns>A task that resolves to a <![CDATA[Response<TResponse>]]> containing the mapped result on success or an error message on
        /// failure.</returns>
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

        /// <summary>
        /// Downloads a file from a relative URL and returns its content as a stream wrapped in a <![CDATA[Response<Stream>]]>.
        /// </summary>
        /// <typeparam name="TRequest">Type of the request body used to construct query string parameters.</typeparam>
        /// <param name="relUrl">Relative URL to append to the configured base URL.</param>
        /// <param name="requestBody">Request object whose properties are converted to query string parameters; may be null.</param>
        /// <param name="methodName">Calling method name used for logging.</param>
        /// <returns>A <![CDATA[Response<Stream>]]> containing the downloaded file stream on success; otherwise contains an error message.</returns>
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
