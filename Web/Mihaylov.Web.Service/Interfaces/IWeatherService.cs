using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Web.Service.Models;

namespace Mihaylov.Web.Service.Interfaces
{
    public interface IWeatherService
    {
        Task<IEnumerable<CurrentWeatherResponse>> GetCurrentWeatherAsync();

        Task<CurrentWeatherResponse> GetCurrentWeatherAsync(string city);

        Task<IEnumerable<ForecastWeatherResponse>> GetForecastWeatherAsync();

        Task<ForecastWeatherResponse> GetForecastWeatherAsync(string city);
    }
}