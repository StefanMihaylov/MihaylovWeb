using System.Collections.Generic;

namespace Mihaylov.Api.Weather.Contracts.Models
{
    public class ForecastWeatherModel
    {
        public CurrentWeatherModel Current { get; set; }

        public IEnumerable<ForecastDayModel> Forecast { get; set; }
    }
}
