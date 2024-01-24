namespace Mihaylov.Api.Weather.Contracts.Models
{
    public class ForecastDayModel
    {
        public string Date { get; set; }

        public string Condition { get; set; }

        public string ConditionIcon { get; set; }

        public decimal MaxTemp { get; set; }

        public decimal MinTemp { get; set; }

        public int DailyChanceOfRain { get; set; }

        public int DailyChanceOfSnow { get; set; }
    }
}
