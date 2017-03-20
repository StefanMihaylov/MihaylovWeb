namespace Mihaylov.Common.WebConfigSettings.Interfaces.Models
{
    public interface IDbConnection
    {
        string Name { get; }

        string ExternalSettings { get; }

        bool IsCodeFirst { get; }

        IDbEndpoint Endpoint { get; }

        IDbCredential Credential { get; }
    }
}
