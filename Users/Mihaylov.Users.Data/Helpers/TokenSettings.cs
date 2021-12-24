namespace Mihaylov.Users.Data
{
    public class TokenSettings
    {
        public string Secret { get; set; }

        public int ExpiresIn { get; set; }

        public int ClaimTypes { get; set; }
    }
}
