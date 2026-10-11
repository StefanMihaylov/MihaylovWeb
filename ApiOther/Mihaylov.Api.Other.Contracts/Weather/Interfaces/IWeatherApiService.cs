using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Weather.Models;

namespace Mihaylov.Api.Other.Contracts.Weather.Interfaces
{
    public interface IWeatherApiService
    {
        Task<CurrentWeatherModel> CurrentAsync(string city, bool metricUnits, string language);

        Task<ForecastWeatherModel> ForecastAsync(string city, int days, bool metricUnits, string language);
    }
}
