namespace Mihaylov.Api.Weather.Data.Models
{
    public class ForecastWeatherResponse : CurrentWeatherResponse
    {
        public ForecastModel Forecast { get; set; }
    }
}
