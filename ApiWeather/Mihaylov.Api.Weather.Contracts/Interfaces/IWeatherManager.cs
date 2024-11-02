using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Api.Weather.Contracts.Models;

namespace Mihaylov.Api.Weather.Contracts.Interfaces
{
    public interface IWeatherManager
    {
        Task<IEnumerable<CurrentWeatherResponse>> GetCurrentWeatherAsync();

        Task<CurrentWeatherResponse> GetCurrentWeatherAsync(string city);

        Task<IEnumerable<ForecastWeatherResponse>> GetForecastWeatherAsync();

        Task<ForecastWeatherResponse> GetForecastWeatherAsync(string city);
    }
}
