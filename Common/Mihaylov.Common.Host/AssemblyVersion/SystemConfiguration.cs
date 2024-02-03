using System;
using Mihaylov.Common.Host.Abstract.Configurations;
using Mihaylov.Common.Host.AssemblyVersion.Interfaces;

namespace Mihaylov.Common.Host.AssemblyVersion
{
    public class SystemConfiguration : ISystemConfiguration
    {
        public string GetVersion()
        {
            var version = Environment.Version?.ToString();

            return $".NET {version}";
        }

        public string GetGitCommit()
        {
            var commit = Config.GetEnvironmentVariable("GIT_COMMIT", string.Empty);

            return commit;
        }

        public string GetJenkinsBuildNumber()
        {
            var build = Config.GetEnvironmentVariable("Jenkins_Build", string.Empty);

            return build;
        }
    }
}
