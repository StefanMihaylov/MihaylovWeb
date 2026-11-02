using Mihaylov.Common;

namespace Mihaylov.Users.Data.Helpers
{
    public class TokenIssuerSettings : TokenSettings
    {
        public int ExpiresIn { get; set; }

        public int ClaimTypes { get; set; }
    }
}
