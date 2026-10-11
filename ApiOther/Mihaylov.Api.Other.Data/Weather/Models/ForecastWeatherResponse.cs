namespace Mihaylov.Api.Other.Data.Weather.Models
{
    public class ForecastWeatherResponse : CurrentWeatherResponse
    {
        public ForecastModel Forecast { get; set; }
    }
}
