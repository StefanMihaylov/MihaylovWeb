using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Weather.Contracts.Interfaces;
using Mihaylov.Api.Weather.Data.Configuration;
using DTO = Mihaylov.Api.Weather.Contracts.Models;

namespace Mihaylov.Api.Weather.Data
{
    public class WeatherManager : IWeatherManager
    {
        private readonly IWeatherApiService _service;
        private readonly IMemoryCache _cache;
        private readonly WeatherConfig _config;

        public WeatherManager(IWeatherApiService service, IMemoryCache cache, IOptions<WeatherConfig> settings)
        {
            _service = service;
            _cache = cache;
            _config = settings.Value;
        }

        public async Task<IEnumerable<DTO.CurrentWeatherResponse>> GetCurrentWeatherAsync()
        {
            var result = new List<DTO.CurrentWeatherResponse>();

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

        public async Task<DTO.CurrentWeatherResponse> GetCurrentWeatherAsync(string city)
        {
            if (!_cache.TryGetValue(city, out DTO.CurrentWeatherResponse currentWeather))
            {
                try
                {
                    var response = await _service.CurrentAsync(city, _config.MetricUnits, _config.Language).ConfigureAwait(false);
                    currentWeather = new DTO.CurrentWeatherResponse(response);

                    _cache.Set(city, currentWeather, TimeSpan.FromMinutes(30));
                }
                catch (Exception ex)
                {
                    return new DTO.CurrentWeatherResponse(city, ex.Message);
                }
            }

            return currentWeather;
        }

        public async Task<IEnumerable<DTO.ForecastWeatherResponse>> GetForecastWeatherAsync()
        {
            var result = new List<DTO.ForecastWeatherResponse>();

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

        public async Task<DTO.ForecastWeatherResponse> GetForecastWeatherAsync(string city)
        {
            var key = $"{city}_forecast";

            if (!_cache.TryGetValue(key, out DTO.ForecastWeatherResponse weather))
            {
                try
                {
                    var response = await _service.ForecastAsync(city, 3 ,_config.MetricUnits, _config.Language).ConfigureAwait(false);
                    var result = new DTO.ForecastWeatherResponse(response);

                    _cache.Set(key, result, TimeSpan.FromMinutes(30));

                    return result;
                }
                catch (Exception ex)
                {
                    return new DTO.ForecastWeatherResponse(city, ex.Message);
                }
            }

            return weather;
        }
    }
}
