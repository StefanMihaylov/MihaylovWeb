namespace Mihaylov.Api.Weather.Data.Models
{
    public class CurrentWeatherResponse
    {
        public LocationModel Location { get; set; }

        public CurrentModel Current { get; set; }
    }
}
