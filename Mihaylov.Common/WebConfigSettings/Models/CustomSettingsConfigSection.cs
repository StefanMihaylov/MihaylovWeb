using System.Configuration;
using Mihaylov.Common.WebConfigSettings.Models.XmlElements;

namespace Mihaylov.Common.WebConfigSettings.Models
{
    public class CustomSettingsConfigSection : ConfigurationSection
    {
        public const string LOGGER_TAG_NAME = "logger";
        public const string ENVIRONMENT_TAG_NAME = "environment";
        public const string ENDPOINTS_TAG_NAME = "endpoints";
        public const string DB_CONNECTIONS_TAG_NAME = "dbConnections";
        public const string DB_CONNECTION_TEMPLATES_TAG_NAME = "dbConnectionTemplates";

        [ConfigurationProperty(ENVIRONMENT_TAG_NAME)]
        public EnvironmentElement Environment
        {
            get { return (EnvironmentElement)this[ENVIRONMENT_TAG_NAME]; }
        }

        [ConfigurationProperty(LOGGER_TAG_NAME)]
        public LoggerElement Logger
        {
            get { return (LoggerElement)this[LOGGER_TAG_NAME]; }
        }

        [ConfigurationProperty(ENDPOINTS_TAG_NAME)]
        public EndpointCollection Endpoints
        {
            get { return (EndpointCollection)this[ENDPOINTS_TAG_NAME]; }
        }

        [ConfigurationProperty(DB_CONNECTIONS_TAG_NAME)]
        public DbConnectionCollection DbConnections
        {
            get { return (DbConnectionCollection)this[DB_CONNECTIONS_TAG_NAME]; }
        }

        [ConfigurationProperty(DB_CONNECTION_TEMPLATES_TAG_NAME)]
        public DbTemplateCollection DbConnectionTemplates
        {
            get { return (DbTemplateCollection)this[DB_CONNECTION_TEMPLATES_TAG_NAME]; }
        }
    }
}
