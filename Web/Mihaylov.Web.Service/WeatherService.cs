using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Weather.Client;
using Mihaylov.Web.Service.Interfaces;
using Mihaylov.Web.Service.Models;

namespace Mihaylov.Web.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherApiClient _weatherApiClient;
        private readonly IMemoryCache _cache;
        private readonly WeatherConfig _config;

        public WeatherService(IWeatherApiClient weatherApiClient, IMemoryCache cache, IOptions<WeatherConfig> settings)
        {
            _weatherApiClient = weatherApiClient;
            _cache = cache;
            _config = settings.Value;
        }

        public async Task<CurrentWeatherResponse> GetCurrentWeatherAsync(string city)
        {
            if (!_cache.TryGetValue(city, out CurrentWeatherResponse currentWeather))
            {
                try
                {
                    var response = await _weatherApiClient.CurrentAsync(city, _config.MetricUnits, _config.Language).ConfigureAwait(false);
                    var result = new CurrentWeatherResponse(response);

                    _cache.Set(city, result, TimeSpan.FromMinutes(30));

                    return result;
                }
                catch (SwaggerException ex)
                {
                    return new CurrentWeatherResponse(city, ex.Message);
                }
                catch (Exception ex)
                {
                    return new CurrentWeatherResponse(city, ex.Message);
                }
            }

            return currentWeather;
        }

        public async Task<IEnumerable<CurrentWeatherResponse>> GetCurrentWeatherAsync()
        {
            var result = new List<CurrentWeatherResponse>();

            foreach (var city in _config.Cities)
            {
                var response = await GetCurrentWeatherAsync(city).ConfigureAwait(false);
                if (response != null)
                {
                    result.Add(response);
                }
            }

            return result;
        }

        public async Task<ForecastWeatherResponse> GetForecastWeatherAsync(string city)
        {
            var key = $"{city}_forecast";

            if (!_cache.TryGetValue(key, out ForecastWeatherResponse weather))
            {
                try
                {
                    var response = await _weatherApiClient.ForecastAsync(city, _config.MetricUnits, _config.Language).ConfigureAwait(false);
                    var result = new ForecastWeatherResponse(response);

                    _cache.Set(key, result, TimeSpan.FromMinutes(30));

                    return result;
                }
                catch (SwaggerException ex)
                {
                    return new ForecastWeatherResponse(city, ex.Message);
                }
                catch (Exception ex)
                {
                    return new ForecastWeatherResponse(city, ex.Message);
                }
            }

            return weather;
        }


        public async Task<IEnumerable<ForecastWeatherResponse>> GetForecastWeatherAsync()
        {
            var result = new List<ForecastWeatherResponse>();

            foreach (var city in _config.Cities)
            {
                var response = await GetForecastWeatherAsync(city).ConfigureAwait(false);
                if (response != null)
                {
                    result.Add(response);
                }
            }

            return result;
        }
    }
}
