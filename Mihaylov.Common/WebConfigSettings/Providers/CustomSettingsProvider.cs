using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Mihaylov.Common.Encryptions;
using Mihaylov.Common.Validations;
using Mihaylov.Common.WebConfigSettings.Interfaces;
using Mihaylov.Common.WebConfigSettings.Models;
using Mihaylov.Common.WebConfigSettings.Models.XmlElements;

namespace Mihaylov.Common.WebConfigSettings.Providers
{
    public class CustomSettingsProvider : ICustomSettingsProvider
    {
        public const string ENVIRONMENT_KEY_NAME = "Environment";

        private IExternalFileConfigurationProvider externalFileProvider;
        private IWebConfigProvider webConfigProvider;

        private Lazy<Configuration> externalConfiguration;
        private Lazy<CustomSettingsConfigSection> webConfigSettings;

        public CustomSettingsProvider()
            : this(new ExternalFileConfigurationProvider(), new WebConfigProvider())
        {
        }

        public CustomSettingsProvider(IExternalFileConfigurationProvider externalFileProvider, IWebConfigProvider webConfigProvider)
        {
            ParameterValidation.IsNotNull(externalFileProvider, nameof(externalFileProvider));
            ParameterValidation.IsNotNull(webConfigProvider, nameof(webConfigProvider));

            this.externalFileProvider = externalFileProvider;
            this.webConfigProvider = webConfigProvider;

            this.externalConfiguration = new Lazy<Configuration>(() =>
            {
                Configuration settings = this.externalFileProvider.GetSettings();
                return settings;
            });

            this.webConfigSettings = new Lazy<CustomSettingsConfigSection>(() =>
            {
                CustomSettingsConfigSection settings = this.webConfigProvider.GetCustomSettings();
                return settings;
            });
        }

        public CustomSettingsModel GetAllSettingByEnvironment()
        {
            var environment = this.GetEnvironment();

            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            this.GetEndpoints(dictionary, environment);
            this.GetConnectionStrings(dictionary, environment);

            var appSettings = this.webConfigProvider.GetAppSettings();
            foreach (var appSettingPair in appSettings)
            {
                if (dictionary.ContainsKey(appSettingPair.Key))
                {
                    throw new ApplicationException($"The \"{appSettingPair.Key}\" appSettings key is duplicated");
                }

                dictionary.Add(appSettingPair.Key, appSettingPair.Value);
            }

            return new CustomSettingsModel()
            {
                Environment = environment,
                LoggerPath = this.webConfigSettings.Value.Logger.Path,
                Settings = dictionary,
            };
        }

        private void GetConnectionStrings(IDictionary<string, string> dictionary, string environment)
        {
            Dictionary<string, string> dbTemplateDictionary = GetDatabaseTemplates();

            string nameSufix = this.GetNameSufix(environment);
            DbConnectionCollection dbConnections = this.webConfigSettings.Value.DbConnections;

            foreach (var dbConnection in dbConnections)
            {
                var dbConnectionName = dbConnection.Name;
                if (dbConnectionName.EndsWith(nameSufix, StringComparison.InvariantCultureIgnoreCase))
                {
                    var systemName = this.GetSystemName(dbConnectionName, nameSufix);

                    if (dictionary.ContainsKey(systemName))
                    {
                        throw new ArgumentException($"\"{systemName}_{environment}\" key in web.config is found in AppSetting and dbConnections areas");
                    }

                    var encryptedPassword = dbConnection.Credential.CipheredPassword;
                    var userName = dbConnection.Credential.UserName;

                    string password = string.Empty;
                    if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(encryptedPassword))
                    {
                        var passwordPhrase = CreatePasswordPhrase(userName, dbConnection.Endpoint.DbName);
                        password = StringEncryptor.Decrypt(encryptedPassword, passwordPhrase);
                    }

                    string connectionString = string.Empty;
                    if (dbTemplateDictionary.ContainsKey(systemName))
                    {
                        string template = dbTemplateDictionary[systemName];
                        connectionString = string.Format(template, dbConnection.Endpoint.IPAdress, dbConnection.Endpoint.Port, dbConnection.Endpoint.DbName, userName, password);
                    }
                    else
                    {
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                        builder.DataSource = dbConnection.Endpoint.IPAdress;
                        builder.InitialCatalog = dbConnection.Endpoint.DbName;
                        builder.MultipleActiveResultSets = true;

                        if (!string.IsNullOrWhiteSpace(password))
                        {
                            builder.UserID = userName;
                            builder.Password = password;
                        }
                        else
                        {
                            builder.IntegratedSecurity = true;
                        }

                        connectionString = builder.ConnectionString.ToString();
                    }

                    dictionary.Add(systemName, connectionString);
                }
            }
        }

        private Dictionary<string, string> GetDatabaseTemplates()
        {
            var dbTemplateDictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var dbTemplate in this.webConfigSettings.Value.DbConnectionTemplates)
            {
                dbTemplateDictionary.Add(dbTemplate.Name, dbTemplate.Template);
            }

            return dbTemplateDictionary;
        }

        private void GetEndpoints(IDictionary<string, string> dictionary, string environment)
        {
            string nameSufix = this.GetNameSufix(environment);

            foreach (var endpoint in this.webConfigSettings.Value.Endpoints)
            {
                var endpointName = endpoint.Name;
                if (endpointName.EndsWith(nameSufix, StringComparison.InvariantCultureIgnoreCase))
                {
                    var systemName = this.GetSystemName(endpointName, nameSufix);

                    string address;
                    if (endpoint.IPAddress.StartsWith("http"))
                    {
                        address = endpoint.IPAddress;
                    }
                    else
                    {
                        address = $"http://{endpoint.IPAddress}";
                    }

                    string port = string.Empty;
                    if (endpoint.Port > 0)
                    {
                        port = $":{endpoint.Port}";
                    }

                    string endpointAddress = $"{address}{port}";

                    dictionary.Add(systemName, endpointAddress);
                }
            }
        }

        private string GetEnvironment()
        {
            string applicationConfigEnvironment = this.GetApplicationConfigSetting(ENVIRONMENT_KEY_NAME);
            if (!string.IsNullOrWhiteSpace(applicationConfigEnvironment))
            {
                return applicationConfigEnvironment;
            }

            string webConfigEnvironment = this.webConfigSettings.Value.Environment.Name;
            if (string.IsNullOrWhiteSpace(webConfigEnvironment))
            {
                throw new ArgumentException($"\"{ENVIRONMENT_KEY_NAME}\" value in web.config cannot be empty");
            }

            return webConfigEnvironment;
        }

        private string GetApplicationConfigSetting(string settingKeyName)
        {
            Configuration applicationConfigSettings = this.externalConfiguration.Value;
            if (applicationConfigSettings != null)
            {
                var settingPair = applicationConfigSettings.AppSettings.Settings[settingKeyName];
                if (settingPair != null)
                {
                    return settingPair.Value;
                }
            }

            return string.Empty;
        }

        private string GetSystemName(string endpointName, string nameSufix)
        {
            int nameLengh = endpointName.IndexOf(nameSufix, StringComparison.InvariantCultureIgnoreCase);
            string systemName = endpointName.Substring(0, nameLengh);
            return systemName;
        }

        private string GetNameSufix(string environment)
        {
            return $"_{environment}";
        }

        public static string CreatePasswordPhrase(string username, string dbName)
        {
            var sha1 = new SHA1Managed();
            var passwordPhraseAsString = string.Format("{0}{1}{0}", dbName.ToLower(), username.ToLower());
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(passwordPhraseAsString));
            var passwordPhrase = string.Join(string.Empty, hash.Select(b => b.ToString("x2")).ToArray());
            return passwordPhrase;
        }
    }
}
