using System;
using System.Collections.Generic;
using System.Text;

namespace Mihaylov.Web.Service.Models
{
    public class WeatherConfig
    {
        public IEnumerable<string> Cities { get; set; }

        public bool MetricUnits { get; set; }

        public string Language { get; set; } 
    }
}
