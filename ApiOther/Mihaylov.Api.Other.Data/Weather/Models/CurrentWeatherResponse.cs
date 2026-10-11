namespace Mihaylov.Api.Other.Data.Weather.Models
{
    public class CurrentWeatherResponse
    {
        public LocationModel Location { get; set; }

        public CurrentModel Current { get; set; }
    }
}
