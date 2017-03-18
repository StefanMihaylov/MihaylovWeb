using System.Collections.Generic;

namespace Mihaylov.Common.WebConfigSettings.Models
{
    public class CustomSettingsModel
    {
        public string Environment { get; set; }

        public string LoggerPath { get; set; }

        public IDictionary<string, string> Settings { get; set; }
    }
}
