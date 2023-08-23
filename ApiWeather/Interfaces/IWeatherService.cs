using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Interfaces
{
    public interface IWeatherService
    {
        Task<CurrentWeatherModel> CurrentAsync(string city, bool metricUnits = true);

        Task<ForecastWeatherModel> ForecastAsync(string city, int days, bool metricUnits = true);
    }
}