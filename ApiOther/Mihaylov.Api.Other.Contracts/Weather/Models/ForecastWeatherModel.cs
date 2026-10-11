using System.Collections.Generic;

namespace Mihaylov.Api.Other.Contracts.Weather.Models
{
    public class ForecastWeatherModel
    {
        public CurrentWeatherModel Current { get; set; }

        public IEnumerable<ForecastDayModel> Forecast { get; set; }
    }
}
