using System.Collections.Generic;
using System.Linq;
using Mihaylov.Api.Weather.Client;

namespace Mihaylov.Web.Service.Models
{
    public class ForecastWeatherResponse
    {
        public CurrentWeatherResponse Current { get; set; }

        public IEnumerable<ForecastWeatherDataModel> Forecast { get; set; }
        

        public ForecastWeatherResponse(ForecastWeatherModel input)
        {
            Current = new CurrentWeatherResponse(input.Current);
            Forecast = input.Forecast.Select(f => new ForecastWeatherDataModel(f, input.Current));
        }

        public ForecastWeatherResponse(string city, string errorMessage)
        {
            Current = new CurrentWeatherResponse(city, errorMessage);
            Forecast = new List<ForecastWeatherDataModel>();
        }
    }
}
