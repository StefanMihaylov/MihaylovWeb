namespace Mihaylov.Api.Weather.Data.Models
{
    public class ForecastDayModel
    {
        public string Date { get; set; }

        public DayModel Day { get; set; }
    }
}
