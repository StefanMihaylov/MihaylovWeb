using System.Collections.Generic;

namespace WeatherApi.Models
{
    public class ForecastWeatherModel
    {
        public CurrentWeatherModel Current { get; set; }

        public IEnumerable<ForecastDayModel> Forecast { get; set; }
    }

    public class ForecastDayModel
    {
        public string Date { get; set; }

        public string Condition { get; set; }

        public decimal MaxTemp { get; set; }

        public decimal MinTemp { get; set; }

        public int DailyChanceOfRain { get; set; }

        public int DailyChanceOfSnow { get; set; }
    }
}
