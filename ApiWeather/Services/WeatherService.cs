using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApi.Interfaces;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly string _baseUrl;
        private readonly string _appId;
        private HttpClient _httpClient;

        public WeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Weather");
            _baseUrl = _httpClient.BaseAddress?.AbsoluteUri.TrimEnd('/') ?? string.Empty;

            _appId = "7c51c35505764326af9190913231104";
        }

        public async Task<CurrentWeatherModel> CurrentAsync(string city, bool metricUnits = true)
        {
            var queryString = $"key={_appId}&q={city}&aqi=no";
            var response = await GetResponseAsync<CurrentWeatherResponse>("v1/current.json", queryString).ConfigureAwait(false);

            var result = MapCurrentWeather(response, metricUnits);

            return result;
        }


        public async Task<ForecastWeatherModel> ForecastAsync(string city, int days, bool metricUnits = true)
        {
            var queryString = $"key={_appId}&q={city}&days={days}&aqi=no&alerts=no";
            var response = await GetResponseAsync<ForecastWeatherResponse>("v1/forecast.json", queryString).ConfigureAwait(false);

            var result = new ForecastWeatherModel
            {
                Current = MapCurrentWeather(response, metricUnits),
                Forecast = response.Forecast.ForecastDay.Select(f => new Models.ForecastDayModel()
                {
                    Date = f.Date,
                    Condition = f.Day.Condition.Text,
                    MinTemp = metricUnits ? f.Day.MinTemp_c : f.Day.MinTemp_f,
                    MaxTemp = metricUnits ? f.Day.MaxTemp_c : f.Day.MaxTemp_f,
                    DailyChanceOfRain = f.Day.Daily_Chance_of_Rain,
                    DailyChanceOfSnow = f.Day.Daily_Chance_of_Snow
                })
            };

            return result;
        }

        private async Task<T> GetResponseAsync<T>(string endpoint, string queryString)
        {
            var url = $"{_baseUrl}/{endpoint}?{queryString}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new ApplicationException("Error");
            }

            var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var response = JsonSerializer.Deserialize<T>(responseString, options);

            return response;
        }

        private CurrentWeatherModel MapCurrentWeather(CurrentWeatherResponse response, bool metricUnits)
        {
            return new CurrentWeatherModel
            {
                Location = response.Location.Name,
                Country = response.Location.Country,
                CurrentDate = response.Location.LocalTime,
                Condition = response.Current.Condition.Text,
                Temperature = metricUnits ? response.Current.Temp_c : response.Current.Temp_f,
                Wind = metricUnits ? response.Current.Wind_kph : response.Current.Wind_mph,
                FeelsLike = metricUnits ? response.Current.Feelslike_c : response.Current.Feelslike_f,
            };
        }
    }
}
