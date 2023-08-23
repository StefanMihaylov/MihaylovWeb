using System;
using System.Collections.Generic;

namespace WeatherApi.Interfaces
{
    public class ForecastWeatherResponse : CurrentWeatherResponse
    {
        public ForecastModel Forecast { get; set; }
    }

    public class CurrentWeatherResponse
    {
        public LocationModel Location { get; set; }

        public CurrentModel Current { get; set; }
    }

    public class LocationModel
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public string LocalTime { get; set; }
    }

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

    public class ConditionModel
    {
        public string Text { get; set; }
    }

    public class ForecastModel
    {
        public IEnumerable<ForecastDayModel> ForecastDay { get; set; }
    }

    public class ForecastDayModel
    {
        public string Date { get; set; }

        public DayModel Day { get; set; }
    }

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
