namespace Mihaylov.Api.Weather.Contracts.Models
{
    public class CurrentWeatherResponse
    {
        public string Location { get; private set; }

        public string Temperature { get; private set; }

        public string FeelsLike { get; private set; }

        public string Wind { get; private set; }

        public string CurrentDate { get; private set; }

        public string Condition { get; private set; }

        public string ConditionIcon { get; private set; }


        public CurrentWeatherResponse(CurrentWeatherModel input) 
            : this($"{input.Location} / {input.Country}", 
                   $"{input.Temperature}{input.TemperatureUnit}",
                   $"{input.FeelsLike}{input.TemperatureUnit}",
                   $"{input.Wind} {input.WindUnit}",
                   input.CurrentDate,
                   input.Condition,
                   input.ConditionIcon)
        {
        }

        public CurrentWeatherResponse(string city, string errorMessage) 
            : this(city, string.Empty, string.Empty, string.Empty, errorMessage, string.Empty, string.Empty)
        {
        }

        private CurrentWeatherResponse(string location, string temperature, string feelsLike, string wind, string currentDate,
            string condition, string conditionIcon)
        {
            Location = location;
            Temperature = temperature;
            FeelsLike = feelsLike;
            Wind = wind;
            CurrentDate = currentDate;
            Condition = condition;
            ConditionIcon = conditionIcon;
        }
    }
}
