namespace Mihaylov.Common.WebConfigSettings.Interfaces.Models
{
    public interface IDbCredential
    {
        string UserName { get; }

        string CipheredPassword { get; }
    }
}
