namespace Mihaylov.Api.Weather.Contracts.Models
{
    public class ForecastWeatherDataModel
    {
        public string Location { get; private set; }

        public string Date { get; set; }

        public string Temperatures { get; set; }

        public string Rains { get; set; }

        public string Condition { get; private set; }

        public string ConditionIcon { get; private set; }


        public ForecastWeatherDataModel(ForecastDayModel input, CurrentWeatherModel current)
        {
            Location = new CurrentWeatherResponse(current).Location;
            Date = input.Date;
            Temperatures = $"от {input.MinTemp}{current.TemperatureUnit} до {input.MaxTemp}{current.TemperatureUnit}";
            Rains = $"Дъжд: {input.DailyChanceOfRain}% Сняг: {input.DailyChanceOfSnow}%";
            Condition = input.Condition;
            ConditionIcon = input.ConditionIcon;
        }
    }
}
