namespace Mihaylov.Api.Weather.Data.Models
{
    public class CurrentModel
    {
        public decimal Temp_c { get; set; }

        public decimal Temp_f { get; set; }

        public decimal Wind_mph { get; set; }

        public decimal Wind_kph { get; set; }

        public decimal Feelslike_c { get; set; }

        public decimal Feelslike_f { get; set; }

        public ConditionModel Condition { get; set; }
    }
}
