namespace Mihaylov.Api.Weather.Data.Models
{
    public class DayModel
    {
        public decimal MaxTemp_c { get; set; }

        public decimal MaxTemp_f { get; set; }

        public decimal MinTemp_c { get; set; }

        public decimal MinTemp_f { get; set; }

        public int Daily_Chance_of_Rain { get; set; }

        public int Daily_Chance_of_Snow { get; set; }

        public ConditionModel Condition { get; set; }
    }
}
