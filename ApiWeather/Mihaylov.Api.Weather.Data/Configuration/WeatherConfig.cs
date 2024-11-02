using System.Collections.Generic;

namespace Mihaylov.Api.Weather.Data.Configuration
{
    public class WeatherConfig
    {
        public IEnumerable<string> Cities { get; set; }

        public bool MetricUnits { get; set; }

        public string Language { get; set; } 
    }
}
