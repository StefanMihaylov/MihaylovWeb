using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Mihaylov.Common.Validations;
using Mihaylov.Common.WebConfigSettings.Interfaces;
using Mihaylov.Common.WebConfigSettings.Interfaces.Models;
using Mihaylov.Common.WebConfigSettings.Models;
using Mihaylov.Common.WebConfigSettings.Models.ExternalDbConnections;

namespace Mihaylov.Common.WebConfigSettings.Providers
{
    public class CustomSettingsProvider : ICustomSettingsProvider
    {
        public const string ENVIRONMENT_KEY_NAME = "Environment";

        private readonly IExternalFileConfigurationProvider externalFileProvider;
        private readonly IWebConfigProvider webConfigProvider;

        private Lazy<Configuration> externalConfiguration;
        private Lazy<CustomSettingsConfigSection> webConfigSettings;
        private Lazy<IDictionary<string, string>> appSettings;

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

            this.appSettings = new Lazy<IDictionary<string, string>>(() =>
            {
                IDictionary<string, string> settings = this.webConfigProvider.GetAppSettings();
                return settings;
            });
        }

        public CustomSettingsModel GetAllSettingByEnvironment()
        {
            var environment = this.GetEnvironment();

            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            this.AddEndpoints(dictionary, environment);
            this.AddConnectionStrings(dictionary, environment);
            this.AddAppSettings(dictionary);

            var result = new CustomSettingsModel()
            {
                Environment = environment,
                LoggerPath = this.webConfigSettings.Value.Logger.Path,
                Settings = dictionary,
            };

            return result;
        }

        private void AddConnectionStrings(IDictionary<string, string> dictionary, string environment)
        {
            IDictionary<string, string> dbTemplateDictionary = GetDatabaseTemplates();

            string nameSufix = this.GetNameSufix(environment);
            ICollection<IDbConnection> dbConnections = this.webConfigSettings.Value.DbConnections
                                                                    .Select(c => (IDbConnection)new DbConnection(c))
                                                                    .ToList();

            foreach (IDbConnection connection in dbConnections)
            {
                IDbConnection dbConnection = connection;
                var dbConnectionName = dbConnection.Name;

                if (dbConnectionName.EndsWith(nameSufix, StringComparison.InvariantCultureIgnoreCase))
                {
                    var systemName = this.GetSystemName(dbConnectionName, nameSufix);

                    if (dictionary.ContainsKey(systemName))
                    {
                        throw new ArgumentException($"\"{systemName}_{environment}\" key in web.config is found in AppSetting and dbConnections areas");
                    }

                    string key = dbConnection.ExternalSettings;
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        string uriString = string.Empty;
                        if (this.appSettings.Value.ContainsKey(key))
                        {
                            uriString = this.appSettings.Value[key];
                        }
                        else
                        {
                            uriString = this.GetApplicationConfigSetting(key);
                            if (string.IsNullOrWhiteSpace(uriString))
                            {
                                throw new ArgumentException($"The Key \"{key}\" is found in web.config AppSetting area or external file");
                            }
                        }

                        dbConnection = ParseAppHarborConnectionString(systemName, dbConnection, uriString);
                    }

                    string connectionString = this.GetConnectionString(dbConnection);
                    if (dbTemplateDictionary.ContainsKey(systemName) && dbConnection.IsCodeFirst == false)
                    {
                        string template = dbTemplateDictionary[systemName];
                        connectionString = string.Format(template, connectionString);
                    }

                    dictionary.Add(systemName, connectionString);
                }
            }
        }

        private string GetConnectionString(IDbConnection dbConnection)
        {
            string dataSource = dbConnection.Endpoint?.IPAdress;
            string initialCatalog = dbConnection.Endpoint?.DbName;

            if (string.IsNullOrWhiteSpace(dataSource) || string.IsNullOrWhiteSpace(initialCatalog))
            {
                throw new ArgumentNullException("Connection string DataSource and InitialCatalog cannot be null");
            }

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = dataSource;
            builder.InitialCatalog = initialCatalog;
            builder.MultipleActiveResultSets = true;
            builder.ApplicationName = "EntityFramework";

            var encryptedPassword = dbConnection.Credential?.CipheredPassword;
            var userName = dbConnection.Credential?.UserName;

            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(encryptedPassword))
            {
                //var passwordPhrase = CreatePasswordPhrase(userName, dbConnection.Endpoint.DbName);
                //password = StringEncryptor.Decrypt(encryptedPassword, passwordPhrase);

                builder.UserID = userName;
                builder.Password = encryptedPassword;
            }
            else
            {
                builder.IntegratedSecurity = true;
            }

            string connectionString = builder.ConnectionString.ToString();
            return connectionString;
        }

        private void AddEndpoints(IDictionary<string, string> dictionary, string environment)
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

        private void AddAppSettings(Dictionary<string, string> dictionary)
        {
            foreach (var appSettingPair in appSettings.Value)
            {
                if (dictionary.ContainsKey(appSettingPair.Key))
                {
                    throw new ApplicationException($"The \"{appSettingPair.Key}\" appSettings key is duplicated");
                }

                string externalSettings = this.GetApplicationConfigSetting(appSettingPair.Key);
                if (!string.IsNullOrWhiteSpace(externalSettings))
                {
                    dictionary.Add(appSettingPair.Key, externalSettings);
                }
                else
                {
                    dictionary.Add(appSettingPair.Key, appSettingPair.Value);
                }
            }
        }

        private string GetEnvironment()
        {
            if (this.appSettings.Value.ContainsKey(ENVIRONMENT_KEY_NAME))
            {
                return this.appSettings.Value[ENVIRONMENT_KEY_NAME];
            }

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

        private IDictionary<string, string> GetDatabaseTemplates()
        {
            var dbTemplateDictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var dbTemplate in this.webConfigSettings.Value.DbConnectionTemplates)
            {
                dbTemplateDictionary.Add(dbTemplate.Name, dbTemplate.Template);
            }

            return dbTemplateDictionary;
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

        private string CreatePasswordPhrase(string username, string dbName)
        {
            var sha1 = new SHA1Managed();

            throw new NotImplementedException();

            string passwordPhraseAsString = string.Empty;
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(passwordPhraseAsString));
            string passwordPhrase = string.Join(string.Empty, hash.Select(b => b.ToString("x2")).ToArray());
            return passwordPhrase;
        }

        private IDbConnection ParseAppHarborConnectionString(string name, IDbConnection dbConnection, string uriString)
        {
            var uri = new Uri(uriString);

            string dataSource = uri.Host;
            string initialCatalog = uri.AbsolutePath.Trim('/');
            string userId = uri.UserInfo.Split(':').First();
            string password = uri.UserInfo.Split(':').Last();

            IDbConnection result = new DbConnection(
                name,
                dbConnection.ExternalSettings,
                dbConnection.IsCodeFirst,
                new DbEndpoint(initialCatalog, dataSource),
                new DbCredential(userId, password));

            return result;
        }
    }
}
