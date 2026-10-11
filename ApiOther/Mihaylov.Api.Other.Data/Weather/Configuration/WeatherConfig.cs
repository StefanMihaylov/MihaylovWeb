using System.Collections.Generic;

namespace Mihaylov.Api.Other.Data.Weather.Configuration
{
    public class WeatherConfig
    {
        public string BaseUrl { get; set; }

        public string AppId { get; set; }

        public IEnumerable<string> Cities { get; set; }

        public bool MetricUnits { get; set; }

        public string Language { get; set; } 
    }
}
