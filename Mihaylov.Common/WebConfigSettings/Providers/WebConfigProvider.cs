using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Configuration;
using Mihaylov.Common.WebConfigSettings.Interfaces;
using Mihaylov.Common.WebConfigSettings.Models;

namespace Mihaylov.Common.WebConfigSettings.Providers
{
    public class WebConfigProvider : IWebConfigProvider
    {
        public const string CUSTOM_SETTINGS_TAG_NAME = "customSettings";

        public CustomSettingsConfigSection GetCustomSettings()
        {
            var settings = WebConfigurationManager.GetSection(CUSTOM_SETTINGS_TAG_NAME) as CustomSettingsConfigSection;
            if (settings == null)
            {
                throw new ApplicationException($"{CUSTOM_SETTINGS_TAG_NAME} tag not found in web.config");
            }

            return settings;
        }

        public IDictionary<string, string> GetAppSettings()
        {
            var result = new Dictionary<string, string>();

            NameValueCollection appSettings = WebConfigurationManager.AppSettings;

            foreach (var key in appSettings.AllKeys)
            {
                result.Add(key, appSettings[key]);
            }

            return result;
        }
    }
}
