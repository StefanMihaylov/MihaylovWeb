using System.Configuration;

namespace Mihaylov.Common.WebConfigSettings.Interfaces
{
    public interface IExternalFileConfigurationProvider
    {
        Configuration GetSettings();
    }
}