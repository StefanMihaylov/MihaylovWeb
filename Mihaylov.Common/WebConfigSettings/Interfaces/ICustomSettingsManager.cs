using Mihaylov.Common.WebConfigSettings.Models;

namespace Mihaylov.Common.WebConfigSettings
{
    public interface ICustomSettingsManager
    {
        CustomSettingsModel Settings { get; }

        string GetSettingByName(string systemName);
        T GetSettingByName<T>(string key);
    }
}