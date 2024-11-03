namespace Mihaylov.Common.Host.Authorization
{
    public class WebHostSettings
    {
        public string CookieName { get; set; }

        public ClaimType? UsernameClaimType { get; set; }

        public string LoginUrl { get; set; }
    }
}
