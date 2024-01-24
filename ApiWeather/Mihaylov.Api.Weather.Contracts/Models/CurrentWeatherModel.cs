namespace Mihaylov.Api.Weather.Contracts.Models
{
    public class CurrentWeatherModel
    {
        public string Location { get; set; }

        public string Country { get; set; }

        public string CurrentDate { get; set; }

        public string Condition { get; set; }

        public decimal Temperature { get; set; }

        public decimal FeelsLike { get; set; }

        public string TemperatureUnit { get; set; }

        public decimal Wind { get; set; }

        public string WindUnit { get; set; }
    }
}
