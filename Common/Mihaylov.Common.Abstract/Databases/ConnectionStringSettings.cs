using System.Data.SqlClient;

namespace Mihaylov.Common.Abstract.Databases
{
    public class ConnectionStringSettings
    {
        public string ServerAddress { get; set; }

        public string DatabaseName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }


        public string GetConnectionString()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = this.ServerAddress,
                InitialCatalog = this.DatabaseName,
                TrustServerCertificate = true,
                MultipleActiveResultSets = true,
            };

            if (string.IsNullOrWhiteSpace(UserName))
            {
                connectionStringBuilder.IntegratedSecurity = true;
            }
            else
            {
                connectionStringBuilder.UserID = this.UserName;
                connectionStringBuilder.Password = this.Password;
                connectionStringBuilder.IntegratedSecurity = false;
            }

            return connectionStringBuilder.ConnectionString;
        }
    }
}
