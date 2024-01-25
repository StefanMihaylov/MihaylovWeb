using System.ComponentModel.DataAnnotations;
using Mihaylov.Api.Weather.Client;

namespace Mihaylov.Web.Service.Models
{
    public class CurrentWeatherResponse
    {
        [Display(Name = "Град")]
        public string Location { get; private set; }

        [Display(Name = "Температура")]
        public string Temperature { get; private set; }

        [Display(Name = "Чувства се като")]
        public string FeelsLike { get; private set; }

        [Display(Name = "Вятър")]
        public string Wind { get; private set; }

        [Display(Name = "Локална Дата")]
        public string CurrentDate { get; private set; }

        [Display(Name = "Условия")]
        public string Condition { get; private set; }

        [Display(Name = "Условия")]
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
