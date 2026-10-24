using System.Collections.Generic;
using Mihaylov.Api.Other.Client;

namespace Mihaylov.UI.Mvc.Other.Areas.Weather.Models
{
    public class WeatherModel
    {
        public IEnumerable<CurrentWeatherResponse> Current { get; set; } = [];

        public IEnumerable<ForecastWeatherResponse> Forecast { get; set; } = [];
    }
}