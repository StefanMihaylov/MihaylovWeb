using System.Collections.Generic;

namespace Mihaylov.Api.Other.Data.Weather.Models
{
    public class ForecastModel
    {
        public IEnumerable<ForecastDayModel> ForecastDay { get; set; }
    }
}
