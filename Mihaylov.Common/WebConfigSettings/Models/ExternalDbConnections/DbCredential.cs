using Mihaylov.Common.WebConfigSettings.Interfaces.Models;

namespace Mihaylov.Common.WebConfigSettings.Models.ExternalDbConnections
{
    public class DbCredential : IDbCredential
    {
        public DbCredential(IDbCredential credential)
            :this(credential.UserName, credential.CipheredPassword)
        {
        }

        public DbCredential(string userName, string password)
        {
            this.UserName = userName;
            this.CipheredPassword = password;
        }

        public string CipheredPassword { get; set; }

        public string UserName { get; set; }
    }
}
