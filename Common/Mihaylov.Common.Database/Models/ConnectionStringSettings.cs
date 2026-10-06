using System.Collections.Generic;

namespace Mihaylov.Common;

/// <summary>
/// Represents connection string configuration for a SQL Server database.
/// </summary>
public class ConnectionStringSettings
{
    /// <summary>
    /// Gets or sets the server address used to establish connections to the target service.
    /// </summary>
    public string ServerAddress { get; set; }

    /// <summary>
    /// Gets or sets the database name.
    /// </summary>
    public string DatabaseName { get; set; }

    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Builds a SQL Server connection string using ServerAddress, DatabaseName, UserName, and Password.
    /// </summary>
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
