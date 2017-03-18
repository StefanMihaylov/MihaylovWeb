using Mihaylov.Common.WebConfigSettings.Models;

namespace Mihaylov.Common.WebConfigSettings.Interfaces
{
    public interface ICustomSettingsProvider
    {
        CustomSettingsModel GetAllSettingByEnvironment();
    }
}