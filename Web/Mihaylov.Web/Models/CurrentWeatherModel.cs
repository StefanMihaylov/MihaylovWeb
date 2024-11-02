using System.Collections.Generic;
using Mihaylov.Api.Weather.Client;

namespace Mihaylov.Web.Models
{
    public class WeatherModel
    {
        public IEnumerable<CurrentWeatherResponse> Current {  get; set; }

        public IEnumerable<ForecastWeatherResponse> Forecast { get; set; }
    }
}
