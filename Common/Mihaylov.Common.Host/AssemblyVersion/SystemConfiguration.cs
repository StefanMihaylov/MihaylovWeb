using System;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;
using Mihaylov.Common.Host.Configurations;

namespace Mihaylov.Common.Host.AssemblyVersion
{
    public class SystemConfiguration : ISystemConfiguration
    {
        public string GetVersion()
        {
            var version = Environment.Version?.ToString();

            return version;
        }

        public string GetGitCommit()
        {
            var commit = Config.GetEnvironmentVariable("GIT_COMMIT", string.Empty);

            return commit;
        }
    }
}
