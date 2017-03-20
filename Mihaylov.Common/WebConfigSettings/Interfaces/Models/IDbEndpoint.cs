namespace Mihaylov.Common.WebConfigSettings.Interfaces.Models
{
    public interface IDbEndpoint
    {
        string IPAdress { get; }

        int? Port { get; }

        string DbName { get; }
    }
}
