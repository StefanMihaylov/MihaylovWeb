using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using Mihaylov.Common.Validations;
using Mihaylov.Common.WebConfigSettings.Models;
using Mihaylov.Common.WebConfigSettings.Providers;

namespace Mihaylov.Common.WebConfigSettings.Interfaces
{
    public class CustomSettingsManager : ICustomSettingsManager
    {
        private ICustomSettingsProvider provider;
        private Lazy<CustomSettingsModel> settings;

        public CustomSettingsManager()
            :this(new CustomSettingsProvider())
        {
        }

        public CustomSettingsManager(ICustomSettingsProvider customSettingsProvider)
        {
            ParameterValidation.IsNotNull(customSettingsProvider, nameof(customSettingsProvider));

            this.provider = customSettingsProvider;

            this.settings = new Lazy<CustomSettingsModel>(() =>
            {
                CustomSettingsModel settings = this.provider.GetAllSettingByEnvironment();
                return settings;
            });
        }

        public CustomSettingsModel Settings
        {
            get
            {
                return this.settings.Value;
            }
        }

        public string GetSettingByName(string systemName)
        {
            IDictionary<string, string> customSettings = this.settings.Value.Settings;
            if (!customSettings.ContainsKey(systemName))
            {
                throw new ConfigurationErrorsException($" The \"{systemName}\" or \"{systemName}_{this.settings.Value.Environment}\" key in web.config is not found in CustomSettings area");
            }

            return customSettings[systemName];
        }

        public T GetSettingByName<T>(string key)
        {
            string setting = this.GetSettingByName(key);

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            T result = (T)converter.ConvertFromInvariantString(setting);
            return result;
        }
    }
}
