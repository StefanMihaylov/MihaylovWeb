using System.Collections.Generic;
using Mihaylov.Common.WebConfigSettings.Models;

namespace Mihaylov.Common.WebConfigSettings.Interfaces
{
    public interface IWebConfigProvider
    {
        CustomSettingsConfigSection GetCustomSettings();

        IDictionary<string, string> GetAppSettings();
    }
}