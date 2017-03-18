using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using Mihaylov.Common.WebConfigSettings.Interfaces;

namespace Mihaylov.Common.WebConfigSettings.Providers
{
    public class ExternalFileConfigurationProvider : IExternalFileConfigurationProvider
    {
        public const string APPLICATION_CONFIG_FILE_NAME = "Application.config";

        public Configuration GetSettings()
        {
            string settingsFilePath = this.GetFilePath(APPLICATION_CONFIG_FILE_NAME);
            if (string.IsNullOrEmpty(settingsFilePath))
            {
                return null;
            }

            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = settingsFilePath;
            Configuration applicationConfigSettings = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return applicationConfigSettings;
        }

        private string GetFilePath(string fileName)
        {
            string startPath = this.GetAssemblyDirectory();
            var directory = new DirectoryInfo(startPath);
            FileInfo applicationConfigInfo = this.GetFilePath(directory, fileName);
            if (applicationConfigInfo != null)
            {
                return applicationConfigInfo.FullName;
            }
            else
            {
                return string.Empty;
            }
        }

        private FileInfo GetFilePath(DirectoryInfo directory, string fileName)
        {
            if (directory == null)
            {
                return null;
            }

            FileInfo[] serverConfigFiles = directory.GetFiles(fileName);
            if (serverConfigFiles.Length == 1)
            {
                return serverConfigFiles[0];
            }
            else
            {
                return this.GetFilePath(directory.Parent, fileName);
            }
        }

        private string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
