using System.Collections.Generic;

namespace Mihaylov.Common.Database.Models
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
                $"Data Source={ServerAddress}",
                $"Initial Catalog={DatabaseName}",
                "Integrated Security=False",
                $"User ID={UserName}",
                $"Password={Password}",
                "MultipleActiveResultSets=True",
                "TrustServerCertificate=True"
            };

            var result = string.Join(';', parts);

            return result;
        }
    }
}
