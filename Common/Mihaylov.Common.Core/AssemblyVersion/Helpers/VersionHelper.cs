using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Mihaylov.Common
{
    /// <summary>
    /// Reads name, version and target framework from an assembly's metadata.
    /// </summary>
    public static class VersionHelper
    {
        /// <summary>
        /// Builds a <see cref="IModuleInfo"/> for the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to inspect.</param>
        /// <returns>
        /// A <see cref="IModuleInfo"/> with the product name (or the assembly name), the informational version without
        /// the <c>+</c> build suffix (or the assembly version), the target framework (or the assembly name), and the build
        /// date as <c>yyyy.MM.dd HH:mm</c> in UTC, taken from the last write time of the assembly file.
        /// <c>GitCommit</c> and <c>JenkinsBuildNumber</c> are <c>null</c>.
        /// </returns>
        public static IModuleInfo GetVersionInfo(Assembly assembly)
        {
            var assemblyName = assembly.GetName();

            var framework = assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName;
            if (string.IsNullOrEmpty(framework))
            {
                framework = assemblyName.Name;
            }

            var name = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? assemblyName.Name;

            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (string.IsNullOrWhiteSpace(version))
            {
                version = assemblyName.Version.ToString();
            }

            var delimiterIndex = version.IndexOf('+');
            if (delimiterIndex > -1)
            {
                version = version.Substring(0, delimiterIndex);
            }

            DateTime buildDate = new FileInfo(assembly.Location).LastWriteTimeUtc;

            var info = new ModuleInfo(name, version, framework, buildDate.ToString("yyyy.MM.dd HH:mm"), null, null);

            return info;
        }
    }
}
