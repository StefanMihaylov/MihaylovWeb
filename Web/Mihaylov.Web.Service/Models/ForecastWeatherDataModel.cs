using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Weather.Client;

namespace Mihaylov.Web.Service.Models
{
    public class ForecastWeatherDataModel
    {
        [Display(Name = "Град")]
        public string Location { get; private set; }

        [Display(Name = "Дата")]
        public string Date { get; set; }

        [Display(Name = "Температури")]
        public string Temperatures { get; set; }

        [Display(Name = "Валежи")]
        public string Rains { get; set; }

        [Display(Name = "Условия")]
        public string Condition { get; private set; }

        [Display(Name = "Условия")]
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
