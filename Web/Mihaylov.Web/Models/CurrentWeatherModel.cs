using System.Collections.Generic;
using Mihaylov.Web.Service.Models;

namespace Mihaylov.Web.Models
{
    public class WeatherModel
    {
        public IEnumerable<CurrentWeatherResponse> Current {  get; set; }

        public IEnumerable<ForecastWeatherResponse> Forecast { get; set; }
    }
}
