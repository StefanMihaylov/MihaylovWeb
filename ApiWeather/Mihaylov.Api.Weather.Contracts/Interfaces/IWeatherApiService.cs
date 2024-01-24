using System.Threading.Tasks;
using Mihaylov.Api.Weather.Contracts.Models;

namespace Mihaylov.Api.Weather.Contracts.Interfaces
{
    public interface IWeatherApiService
    {
        Task<CurrentWeatherModel> CurrentAsync(string city, bool metricUnits = true);

        Task<ForecastWeatherModel> ForecastAsync(string city, int days, bool metricUnits = true);
    }
}
