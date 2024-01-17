using System.Collections.Generic;

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
            var parts = new List<string>()
            {
                $"Data Source={this.ServerAddress}",
                $"Initial Catalog={this.DatabaseName}",
                "Integrated Security=False",
                $"User ID={this.UserName}",
                $"Password={this.Password}",
                "MultipleActiveResultSets=True",
                "TrustServerCertificate=True"
            };

            var result = string.Join(';',parts);

            return result;
        }
    }
}
