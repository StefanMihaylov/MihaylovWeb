using System.Collections.Generic;

namespace Mihaylov.Api.Weather.Data.Models
{
    public class ForecastModel
    {
        public IEnumerable<ForecastDayModel> ForecastDay { get; set; }
    }
}
