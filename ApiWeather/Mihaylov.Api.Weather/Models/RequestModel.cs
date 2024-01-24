using System.ComponentModel.DataAnnotations;

namespace Mihaylov.Api.Weather.Models
{
    public class RequestModel
    {
        [Required]
        public string City { get; set; }

        public bool? MetricUnits { get; set; }

        public string Language { get; set; }

        public void AddDefault()
        {
            if (string.IsNullOrEmpty(Language))
            {
                Language = "bg";
            }

            if (!MetricUnits.HasValue)
            {
                MetricUnits = true;
            }
        }
    }
}
