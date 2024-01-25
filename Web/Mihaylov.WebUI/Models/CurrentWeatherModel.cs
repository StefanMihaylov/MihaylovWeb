using System.Collections.Generic;
using Mihaylov.Web.Service.Models;

namespace Mihaylov.WebUI.Models
{
    public class CurrentWeatherModel
    {
        public IEnumerable<CurrentWeatherResponse> Current {  get; set; }
    }
}
