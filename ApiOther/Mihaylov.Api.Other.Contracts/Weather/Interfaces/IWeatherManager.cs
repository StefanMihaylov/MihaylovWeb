using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Weather.Models;

namespace Mihaylov.Api.Other.Contracts.Weather.Interfaces
{
    public interface IWeatherManager
    {
        Task<IEnumerable<CurrentWeatherResponse>> GetCurrentWeatherAsync();

        Task<CurrentWeatherResponse> GetCurrentWeatherAsync(string city);

        Task<IEnumerable<ForecastWeatherResponse>> GetForecastWeatherAsync();

        Task<ForecastWeatherResponse> GetForecastWeatherAsync(string city);
    }
}
