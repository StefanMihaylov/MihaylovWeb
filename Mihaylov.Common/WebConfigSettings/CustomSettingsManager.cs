using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using Mihaylov.Common.Validations;
using Mihaylov.Common.WebConfigSettings.Interfaces;
using Mihaylov.Common.WebConfigSettings.Models;
using Mihaylov.Common.WebConfigSettings.Providers;

namespace Mihaylov.Common.WebConfigSettings
{
    public class CustomSettingsManager : ICustomSettingsManager
    {
        private ICustomSettingsProvider provider;
        private Lazy<CustomSettingsModel> settings;
        private Lazy<IDictionary<string, string>> customSettings;

        public CustomSettingsManager()
            : this(new CustomSettingsProvider())
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

            this.customSettings = new Lazy<IDictionary<string, string>>(() =>
            {
                return new Dictionary<string, string>(this.settings.Value.Settings, StringComparer.InvariantCultureIgnoreCase);
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
            if (!this.customSettings.Value.ContainsKey(systemName))
            {
                throw new ConfigurationErrorsException($" The \"{systemName}\" or \"{systemName}_{this.settings.Value.Environment}\" key in web.config is not found in CustomSettings area");
            }

            return this.customSettings.Value[systemName];
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
